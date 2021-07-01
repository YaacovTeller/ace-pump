Namespace UI
    Public Class SyncSummaryForm        
        Public Sub SetText(summary As String)
            SummaryTextBox.Text = summary
        End Sub

        Private Sub CloseFormButton_Click(sender As Object, e As System.EventArgs) Handles CloseFormButton.Click
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End Namespace