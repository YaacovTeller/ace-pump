Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class QbAuthorizationInstructions
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
            Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
            Me.QbInstructionsTextBox = New System.Windows.Forms.TextBox()
            Me.RetryQbConnectButton = New System.Windows.Forms.Button()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.CancelQbConnectButton = New System.Windows.Forms.Button()
            Me.ShowHelpButton = New System.Windows.Forms.Button()
            CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SplitContainer1.Panel1.SuspendLayout()
            Me.SplitContainer1.Panel2.SuspendLayout()
            Me.SplitContainer1.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'SplitContainer1
            '
            Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
            Me.SplitContainer1.Name = "SplitContainer1"
            '
            'SplitContainer1.Panel1
            '
            Me.SplitContainer1.Panel1.Controls.Add(Me.ShowHelpButton)
            Me.SplitContainer1.Panel1.Controls.Add(Me.QbInstructionsTextBox)
            Me.SplitContainer1.Panel1.Controls.Add(Me.CancelQbConnectButton)
            Me.SplitContainer1.Panel1.Controls.Add(Me.RetryQbConnectButton)
            '
            'SplitContainer1.Panel2
            '
            Me.SplitContainer1.Panel2.Controls.Add(Me.PictureBox1)
            Me.SplitContainer1.Size = New System.Drawing.Size(1122, 516)
            Me.SplitContainer1.SplitterDistance = 373
            Me.SplitContainer1.TabIndex = 0
            '
            'QbInstructionsTextBox
            '
            Me.QbInstructionsTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.QbInstructionsTextBox.Location = New System.Drawing.Point(12, 12)
            Me.QbInstructionsTextBox.Multiline = True
            Me.QbInstructionsTextBox.Name = "QbInstructionsTextBox"
            Me.QbInstructionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.QbInstructionsTextBox.Size = New System.Drawing.Size(345, 243)
            Me.QbInstructionsTextBox.TabIndex = 3
            '
            'RetryQbConnectButton
            '
            Me.RetryQbConnectButton.Location = New System.Drawing.Point(180, 265)
            Me.RetryQbConnectButton.Name = "RetryQbConnectButton"
            Me.RetryQbConnectButton.Size = New System.Drawing.Size(75, 23)
            Me.RetryQbConnectButton.TabIndex = 1
            Me.RetryQbConnectButton.Text = "Retry"
            Me.RetryQbConnectButton.UseVisualStyleBackColor = True
            '
            'PictureBox1
            '
            Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PictureBox1.Image = Global.AcePump.SyncForQuickBooks.My.Resources.Resources.QbAuthorizationDialog
            Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(743, 514)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.PictureBox1.TabIndex = 0
            Me.PictureBox1.TabStop = False
            '
            'CancelQbConnectButton
            '
            Me.CancelQbConnectButton.Location = New System.Drawing.Point(261, 265)
            Me.CancelQbConnectButton.Name = "CancelQbConnectButton"
            Me.CancelQbConnectButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelQbConnectButton.TabIndex = 2
            Me.CancelQbConnectButton.Text = "Cancel"
            Me.CancelQbConnectButton.UseVisualStyleBackColor = True
            '
            'ShowHelpButton
            '
            Me.ShowHelpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ShowHelpButton.Location = New System.Drawing.Point(144, 481)
            Me.ShowHelpButton.Name = "ShowHelpButton"
            Me.ShowHelpButton.Size = New System.Drawing.Size(213, 23)
            Me.ShowHelpButton.TabIndex = 4
            Me.ShowHelpButton.Text = "Show QuickBooks Authorization Help"
            Me.ShowHelpButton.UseVisualStyleBackColor = True
            '
            'QbAuthorizationInstructions
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1122, 516)
            Me.Controls.Add(Me.SplitContainer1)
            Me.Name = "QbAuthorizationInstructions"
            Me.Text = "Could not connect to QuickBooks"
            Me.SplitContainer1.Panel1.ResumeLayout(False)
            Me.SplitContainer1.Panel1.PerformLayout()
            Me.SplitContainer1.Panel2.ResumeLayout(False)
            Me.SplitContainer1.Panel2.PerformLayout()
            CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.SplitContainer1.ResumeLayout(False)
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents RetryQbConnectButton As System.Windows.Forms.Button
        Friend WithEvents QbInstructionsTextBox As System.Windows.Forms.TextBox
        Friend WithEvents ShowHelpButton As System.Windows.Forms.Button
        Friend WithEvents CancelQbConnectButton As System.Windows.Forms.Button
    End Class
End Namespace