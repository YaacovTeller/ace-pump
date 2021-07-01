Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.Qb.Models

Namespace UI

    Public Class ChoosePartForm
        Public Property PartName As String
        Public Property DeliveryticketID As Integer
        Public Property ChosenQbPartID As String

        Sub SetValues(partNameNotFound As String, currentDeliveryticketID As Integer)
            PartName = partNameNotFound
            DeliveryticketID = currentDeliveryticketID

            SetupForm()
        End Sub

        Private Sub SetupForm()
            ChooseInstructionLabel.Text = String.Format("We could not find part {0} for ticket {1} in Quickbooks. " & vbNewLine & vbNewLine & _
                                                        "You can 1) Lookup a single part number in Quickbooks or " & "2) Load entire part list from Quickbooks.", PartName, DeliveryticketID)
            LookupSingleInstructionLabel.Text = String.Format("Do you know the missing part number or did you just create it in Quickbooks? " & vbNewLine & _
                                                           "Click here to look it up directly in Quickbooks:")
        End Sub

        Private Sub PopulateParts()
            ChooseGridView.DataSource = QbCacheService.GetAllQbParts()

            ChooseGridView.Columns("ListID").Visible = False

            ChooseGridView.Columns("Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            ChooseGridView.Columns("Description").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            ChooseGridView.Rows(1).Selected = True

            ChooseOkButton.Visible = True
            PopulateButton.Visible = False
        End Sub

        Private Sub ChooseOkButton_Click(sender As System.Object, e As System.EventArgs) Handles ChooseOkButton.Click
            If ChooseGridView.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = ChooseGridView.SelectedRows(0)

                Dim selected = DirectCast(selectedRow.DataBoundItem, QbItemModel)
                ChosenQbPartID = selected.ListID
                DialogResult = DialogResult.OK

                Close()
            End If
        End Sub

        Private Sub ChooseGridView_SelectionChanged(sender As Object, e As System.EventArgs) Handles ChooseGridView.SelectionChanged
            Dim gridView = DirectCast(sender, DataGridView)
            If gridView.SelectedRows.Count > 0 Then

                Dim selectedRow As DataGridViewRow = gridView.SelectedRows(0)
                Dim selected = DirectCast(selectedRow.DataBoundItem, QbItemModel)
                SelectedLabel.Text = selected.Name & vbNewLine & vbNewLine & selected.Description
            Else
                SelectedLabel.Text = "Nothing selected"
            End If
        End Sub

        Private Sub CheckSpecificPartButton_Click(sender As System.Object, e As System.EventArgs) Handles CheckSpecificPartButton.Click
            Using findForm As New FindPartInQbForm
                findForm.SetupForm(PartName)
                Dim result As DialogResult = findForm.ShowDialog()
                If result = DialogResult.OK Then
                    ChosenQbPartID = findForm.FoundQbPartID
                    DialogResult = DialogResult.OK
                    Close()
                End If
            End Using
        End Sub

        Private Sub PopulateButton_Click(sender As System.Object, e As System.EventArgs) Handles PopulateButton.Click
            Cursor.Current = Cursors.WaitCursor
            PopulateParts()
            Cursor.Current = Cursors.Default
        End Sub

        Private Sub ChoosePartForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If ChosenQbPartID Is Nothing Then
                Dim result As DialogResult = MessageBox.Show("You have not chosen a matching part in Quickbooks. If you close now, this part will not be transferred to the invoice in Quickbooks." & vbNewLine & _
                                                             "Are you sure you want to close?", "Choosing a part", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    e.Cancel = True
                End If
            End If
        End Sub
    End Class
End Namespace