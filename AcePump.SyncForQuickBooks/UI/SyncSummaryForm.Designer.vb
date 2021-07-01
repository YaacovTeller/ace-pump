Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SyncSummaryForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
        Me.CloseFormButton = New System.Windows.Forms.Button()
            Me.SummaryTextBox = New System.Windows.Forms.TextBox()
            Me.SuspendLayout()
            '
            'CloseFormButton
            '
            Me.CloseFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CloseFormButton.Location = New System.Drawing.Point(728, 320)
            Me.CloseFormButton.Name = "CloseFormButton"
            Me.CloseFormButton.Size = New System.Drawing.Size(75, 23)
            Me.CloseFormButton.TabIndex = 9
            Me.CloseFormButton.Text = "OK"
            Me.CloseFormButton.UseVisualStyleBackColor = True
            '
            'SummaryTextBox
            '
            Me.SummaryTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.SummaryTextBox.Location = New System.Drawing.Point(32, 32)
            Me.SummaryTextBox.Multiline = True
            Me.SummaryTextBox.Name = "SummaryTextBox"
            Me.SummaryTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.SummaryTextBox.Size = New System.Drawing.Size(753, 264)
            Me.SummaryTextBox.TabIndex = 10
            '
            'SyncSummaryForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(815, 355)
            Me.Controls.Add(Me.SummaryTextBox)
            Me.Controls.Add(Me.CloseFormButton)
            Me.Name = "SyncSummaryForm"
        Me.Text = "Sync Summary"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
        Friend WithEvents CloseFormButton As System.Windows.Forms.Button
        Friend WithEvents SummaryTextBox As System.Windows.Forms.TextBox
    End Class
End Namespace