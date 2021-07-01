Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.Qb.Models

Namespace UI

    Public Class ChooseCustomerForm
        Public Property CustomerName As String
        Public Property DeliveryticketID As Integer
        Public Property ChosenCustomerQbListID As String

        Sub SetValues(customerNameNotFound As String, currentDeliveryticketID As Integer)
            CustomerName = customerNameNotFound
            DeliveryticketID = currentDeliveryticketID

            SetupForm()
        End Sub

        Private Sub SetupForm()
            ChooseInstructionLabel.Text = String.Format("We could not find {0} {1} for ticket {2} in Quickbooks. " & vbNewLine & "Please choose the correct {0} from the list.", "customer", CustomerName, DeliveryticketID)
            CreateNewInstructionLabel.Text = String.Format("Couldn't find the customer you're looking for? Click here to create a new one in Quickbooks.")

            PopulateCustomers()

            ChooseGridView.Rows(1).Selected = True
        End Sub

        Private Sub PopulateCustomers()
            ChooseGridView.DataSource = QbCacheService.GetAllQbCustomers()

            ChooseGridView.Columns("ListID").Visible = False
            ChooseGridView.Columns("Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End Sub

        Private Sub ChooseOkButton_Click(sender As System.Object, e As System.EventArgs) Handles ChooseOkButton.Click
            If ChooseGridView.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = ChooseGridView.SelectedRows(0)
                Dim selected = DirectCast(selectedRow.DataBoundItem, QbCustomerModel)
                ChosenCustomerQbListID = selected.ListID
                DialogResult = DialogResult.OK

                Close()
            End If
        End Sub

        Private Sub ChooseGridView_SelectionChanged(sender As Object, e As System.EventArgs) Handles ChooseGridView.SelectionChanged
            Dim gridView = DirectCast(sender, DataGridView)
            If gridView.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = gridView.SelectedRows(0)
                Dim selected = DirectCast(selectedRow.DataBoundItem, QbCustomerModel)
                SelectedLabel.Text = selected.Name
            Else
                SelectedLabel.Text = "Nothing selected"
            End If
        End Sub

        Private Sub CreateNewButton_Click(sender As System.Object, e As System.EventArgs) Handles CreateNewButton.Click
            Using createDialog As New CreateNewQbCustomerForm
                createDialog.SetupForm(CustomerName)
                Dim result As DialogResult = createDialog.ShowDialog()
                If result = DialogResult.OK Then
                    ChosenCustomerQbListID = createDialog.CreatedQbCustomerID
                    DialogResult = DialogResult.OK
                    Close()
                End If
            End Using
        End Sub

        Private Sub ChooseCustomerForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If ChosenCustomerQbListID Is Nothing Then
                Dim result As DialogResult = MessageBox.Show("You have not chosen a matching customer from Quickbooks. If you close now, this ticket will not be transferred to the invoice in Quickbooks." & vbNewLine & _
                                                             "Are you sure you want to close?", "Choosing a part", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    e.Cancel = True
                End If
            End If
        End Sub

        Private Sub ChooseCustomerForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        End Sub
    End Class
End Namespace