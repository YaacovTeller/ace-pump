Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.Qb.Models

Namespace UI

    Public Class FindPartInQbForm
        Public Property FoundQbPartID() As String

        Public Sub SetupForm(partName As String)
            NameTextBox.Text = partName
        End Sub

        Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
            If ValidateForm() Then
                Cursor.Current = Cursors.WaitCursor
                Dim qbPartLookupList As List(Of QbItemModel) = QbPartIDManager.FindPartInQb(NameTextBox.Text)
                Cursor.Current = Cursors.Default
                If qbPartLookupList IsNot Nothing AndAlso qbPartLookupList.Count > 0 Then
                    Dim exactMatch = qbPartLookupList.FirstOrDefault(Function(x) x.Name = NameTextBox.Text)
                    If exactMatch IsNot Nothing Then
                        FoundQbPartID = exactMatch.ListID
                        DialogResult = DialogResult.OK
                    Else
                        MessageBox.Show("There was more than one part that matched. Please enter a more specific name to refine your search.")
                    End If
                Else
                    MessageBox.Show("Could not find that part in Quickbooks. Please try again.")
                End If
                Close()
            End If
        End Sub

        Private Sub CancelButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelFormButton.Click
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
    End Class
End Namespace