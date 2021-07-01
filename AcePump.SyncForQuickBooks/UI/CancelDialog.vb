Imports System.Windows.Forms

Namespace UI
    Public Class CancelDialog
        Private WithEvents _formsTimer As New Timer

        Private Sub CancelButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelButton.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub ContinueButton_Click(sender As System.Object, e As System.EventArgs) Handles ContinueButton.Click
            DialogResult = DialogResult.OK
            Close()
        End Sub

        Private Sub StartTimer()
            _formsTimer.Interval = 5000
            _formsTimer.Start()
        End Sub

        Private Sub timer_Tick(sender As Object, e As EventArgs) Handles _formsTimer.Tick
            DialogResult = DialogResult.OK
            Me.Close()
        End Sub

        Private Sub CancelDialog_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
            StartTimer()
        End Sub
    End Class
End Namespace