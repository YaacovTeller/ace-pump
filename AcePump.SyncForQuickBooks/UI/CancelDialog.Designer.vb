Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CancelDialog
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
            Me.InstructionLabel1 = New System.Windows.Forms.Label()
            Me.InstructionLabel2 = New System.Windows.Forms.Label()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.ContinueButton = New System.Windows.Forms.Button()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'InstructionLabel1
            '
            Me.InstructionLabel1.Dock = System.Windows.Forms.DockStyle.Top
            Me.InstructionLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.InstructionLabel1.Location = New System.Drawing.Point(0, 0)
            Me.InstructionLabel1.Name = "InstructionLabel1"
            Me.InstructionLabel1.Padding = New System.Windows.Forms.Padding(5)
            Me.InstructionLabel1.Size = New System.Drawing.Size(508, 39)
            Me.InstructionLabel1.TabIndex = 0
            Me.InstructionLabel1.Text = "The sync is still running."
            Me.InstructionLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'InstructionLabel2
            '
            Me.InstructionLabel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.InstructionLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.InstructionLabel2.Location = New System.Drawing.Point(0, 39)
            Me.InstructionLabel2.Name = "InstructionLabel2"
            Me.InstructionLabel2.Padding = New System.Windows.Forms.Padding(5)
            Me.InstructionLabel2.Size = New System.Drawing.Size(508, 51)
            Me.InstructionLabel2.TabIndex = 1
            Me.InstructionLabel2.Text = "To cancel, please click ""Really Cancel"", otherwise click ""Continue with Sync"". " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
        "This dialog will close in a few seconds if no response is received."
            Me.InstructionLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'CancelButton
            '
            Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(70, 116)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(120, 23)
            Me.CancelButton.TabIndex = 2
            Me.CancelButton.Text = "Really Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'ContinueButton
            '
            Me.ContinueButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ContinueButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.ContinueButton.Location = New System.Drawing.Point(318, 116)
            Me.ContinueButton.Name = "ContinueButton"
            Me.ContinueButton.Size = New System.Drawing.Size(120, 23)
            Me.ContinueButton.TabIndex = 3
            Me.ContinueButton.Text = "Continue with Sync"
            Me.ContinueButton.UseVisualStyleBackColor = True
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = Global.AcePump.SyncForQuickBooks.My.Resources.Resources.loading_image
            Me.PictureBox1.Location = New System.Drawing.Point(231, 102)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
            Me.PictureBox1.TabIndex = 4
            Me.PictureBox1.TabStop = False
            '
            'CancelDialog
            '
            Me.AcceptButton = Me.ContinueButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.CancelButton
            Me.ClientSize = New System.Drawing.Size(508, 169)
            Me.Controls.Add(Me.PictureBox1)
            Me.Controls.Add(Me.ContinueButton)
            Me.Controls.Add(Me.CancelButton)
            Me.Controls.Add(Me.InstructionLabel2)
            Me.Controls.Add(Me.InstructionLabel1)
            Me.Name = "CancelDialog"
            Me.Text = "Cancel Sync"
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents InstructionLabel1 As System.Windows.Forms.Label
        Friend WithEvents InstructionLabel2 As System.Windows.Forms.Label
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents ContinueButton As System.Windows.Forms.Button
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    End Class
End Namespace