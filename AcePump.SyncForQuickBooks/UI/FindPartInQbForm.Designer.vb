Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FindPartInQbForm
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
            Me.InstructionsLabel = New System.Windows.Forms.Label()
            Me.NameTextBox = New System.Windows.Forms.TextBox()
            Me.NameLabel = New System.Windows.Forms.Label()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelFormButton = New System.Windows.Forms.Button()
            Me.NameRequiredLabel = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'InstructionsLabel
            '
            Me.InstructionsLabel.AutoSize = True
            Me.InstructionsLabel.Location = New System.Drawing.Point(13, 13)
            Me.InstructionsLabel.Name = "InstructionsLabel"
            Me.InstructionsLabel.Size = New System.Drawing.Size(159, 13)
            Me.InstructionsLabel.TabIndex = 0
            Me.InstructionsLabel.Text = "Check Quickbooks for part with:"
            '
            'NameTextBox
            '
            Me.NameTextBox.Location = New System.Drawing.Point(150, 46)
            Me.NameTextBox.Name = "NameTextBox"
            Me.NameTextBox.Size = New System.Drawing.Size(264, 20)
            Me.NameTextBox.TabIndex = 1
            '
            'NameLabel
            '
            Me.NameLabel.AutoSize = True
            Me.NameLabel.Location = New System.Drawing.Point(106, 53)
            Me.NameLabel.Name = "NameLabel"
            Me.NameLabel.Size = New System.Drawing.Size(38, 13)
            Me.NameLabel.TabIndex = 2
            Me.NameLabel.Text = "Name:"
            '
            'OkButton
            '
            Me.OkButton.Location = New System.Drawing.Point(258, 88)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 7
            Me.OkButton.Text = "OK"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelFormButton
            '
            Me.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelFormButton.Location = New System.Drawing.Point(339, 88)
            Me.CancelFormButton.Name = "CancelFormButton"
            Me.CancelFormButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelFormButton.TabIndex = 8
            Me.CancelFormButton.Text = "Cancel"
            Me.CancelFormButton.UseVisualStyleBackColor = True
            '
            'NameRequiredLabel
            '
            Me.NameRequiredLabel.AutoSize = True
            Me.NameRequiredLabel.ForeColor = System.Drawing.Color.Red
            Me.NameRequiredLabel.Location = New System.Drawing.Point(420, 46)
            Me.NameRequiredLabel.Name = "NameRequiredLabel"
            Me.NameRequiredLabel.Size = New System.Drawing.Size(90, 13)
            Me.NameRequiredLabel.TabIndex = 9
            Me.NameRequiredLabel.Text = "*Name is required"
            Me.NameRequiredLabel.Visible = False
            '
            'FindPartInQbForm
            '
            Me.AcceptButton = Me.OkButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.CancelFormButton
            Me.ClientSize = New System.Drawing.Size(562, 125)
            Me.Controls.Add(Me.NameRequiredLabel)
            Me.Controls.Add(Me.CancelFormButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.NameLabel)
            Me.Controls.Add(Me.NameTextBox)
            Me.Controls.Add(Me.InstructionsLabel)
            Me.Name = "FindPartInQbForm"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents InstructionsLabel As System.Windows.Forms.Label
        Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
        Friend WithEvents NameLabel As System.Windows.Forms.Label
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelFormButton As System.Windows.Forms.Button
        Friend WithEvents NameRequiredLabel As System.Windows.Forms.Label
    End Class
End Namespace