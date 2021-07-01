Imports AcePump.SyncForQuickBooks.UI

Module Program
    Public Sub Main()

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)


        Using login As New SyncLogin
            If (login.ShowDialog() = DialogResult.OK) Then
                Application.Run(New SyncForm(login.UsernameTextBox.Text))
            Else
                Application.Exit()
            End If
        End Using
    End Sub
End Module
