Namespace UI
    Public Class QbAuthorizationInstructions
        Public Property FileName As String
        Public Property QbResponseMessage As String
        Private originalPanel1Width As Integer
        Private originalFormWidth As Integer
        Private originalFormHeight As Integer

        Private Sub RetryQbConnectButton_Click(sender As System.Object, e As System.EventArgs) Handles RetryQbConnectButton.Click
            DialogResult = DialogResult.Retry
            Close()
        End Sub

        Private Sub CancelQbConnectButton_Click(sender As System.Object, e As System.EventArgs) Handles CancelQbConnectButton.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub QbAuthorizationInstructions_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
            SetDialogText()
            originalFormWidth = Me.Width
            originalPanel1Width = SplitContainer1.Panel1.Width
            originalFormHeight = Me.Height
            ToggleHelpImage()
        End Sub

        Private Sub SetDialogText()
            QbInstructionsTextBox.Text = "Could not establish connection to: " & FileName & vbNewLine & vbNewLine & _
                                    "Please ensure the company file is open and you've given this app permission to connect in the dialog that appears (see help button below). " & vbNewLine & vbNewLine & _
                                    "The following message was received by Quickbooks while trying to connect:" & vbNewLine & vbNewLine & _
                                    QbResponseMessage
        End Sub

        Private Sub ShowHelpButton_Click(sender As System.Object, e As System.EventArgs) Handles ShowHelpButton.Click
            ToggleHelpImage()
        End Sub

        Private Sub ToggleHelpImage()
            Dim isCollapsed As Boolean = SplitContainer1.Panel2Collapsed
            Me.Height = originalFormHeight
            If isCollapsed Then
                ShowHelpButton.Text = "Hide QuickBooks authorization help"
                Me.Width = originalFormWidth
                SplitContainer1.Panel2Collapsed = False
            Else
                ShowHelpButton.Text = "Show QuickBooks authorization help"
                Me.Width = originalPanel1Width
                SplitContainer1.Panel2Collapsed = True
            End If
        End Sub
    End Class
End Namespace