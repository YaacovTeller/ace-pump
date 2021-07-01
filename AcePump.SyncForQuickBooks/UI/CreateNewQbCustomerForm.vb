Imports AcePump.SyncForQuickBooks.Qb

Namespace UI

    Public Class CreateNewQbCustomerForm

        Public Property CreatedQbCustomerID As String

        Public Sub SetupForm(customerName As String)
            NameTextBox.Text = customerName
        End Sub

        Private Sub CancelButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelCreateButton.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Function ValidateForm() As Boolean
            Dim isValid As Boolean = True
            If String.IsNullOrWhiteSpace(NameTextBox.Text) Then
                isValid = False
                NameRequiredLabel.Visible = True
            Else
                NameRequiredLabel.Visible = False
            End If
            Return isValid
        End Function

        Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
            If ValidateForm() Then
                Dim createResponse As QbCreateResponse = QbCustomerIDManager.CreateNewQbCustomer(NameTextBox.Text)
                If String.IsNullOrWhiteSpace(createResponse.StatusMessage) Then
                    CreatedQbCustomerID = createResponse.CreatedID
                    DialogResult = DialogResult.OK
                    Close()
                Else
                    MessageBox.Show(createResponse.StatusMessage & vbNewLine & "Please make sure that this name doesn't already exist in QuickBooks " & _
                                    "in a different list (for example Vendors) as all names need to be unique.")
                End If
            End If
        End Sub
    End Class
End Namespace