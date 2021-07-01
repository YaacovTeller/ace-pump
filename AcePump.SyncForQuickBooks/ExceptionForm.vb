Public Class ExceptionForm

    Private Sub CloseFormButton_Click(sender As System.Object, e As System.EventArgs) Handles CloseFormButton.Click
        Close()
    End Sub

    Public Sub SetExceptionTextAndPath(exceptionText As String, linkPath As String)
        ExceptionLabel.Text = exceptionText
        PathToLogFileLink.Text = System.IO.Path.GetFileName(linkPath)
        Dim logLink As LinkLabel.Link = New LinkLabel.Link()
        logLink.LinkData = linkPath
        PathToLogFileLink.Links.Add(logLink)
    End Sub

    Private Sub PathToLogFileLink_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles PathToLogFileLink.LinkClicked
        Process.Start(DirectCast(e.Link.LinkData, String))
    End Sub
End Class