Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CreateNewQbCustomerForm
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
            Me.NameRequiredLabel = New System.Windows.Forms.Label()
            Me.CancelCreateButton = New System.Windows.Forms.Button()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.NameLabel = New System.Windows.Forms.Label()
            Me.NameTextBox = New System.Windows.Forms.TextBox()
            Me.InstructionsLabel = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'NameRequiredLabel
            '
            Me.NameRequiredLabel.AutoSize = True
            Me.NameRequiredLabel.ForeColor = System.Drawing.Color.Red
            Me.NameRequiredLabel.Location = New System.Drawing.Point(419, 49)
            Me.NameRequiredLabel.Name = "NameRequiredLabel"
            Me.NameRequiredLabel.Size = New System.Drawing.Size(90, 13)
            Me.NameRequiredLabel.TabIndex = 15
            Me.NameRequiredLabel.Text = "*Name is required"
            Me.NameRequiredLabel.Visible = False
            '
            'CancelCreateButton
            '
            Me.CancelCreateButton.Location = New System.Drawing.Point(338, 91)
            Me.CancelCreateButton.Name = "CancelCreateButton"
            Me.CancelCreateButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelCreateButton.TabIndex = 14
            Me.CancelCreateButton.Text = "Cancel"
            Me.CancelCreateButton.UseVisualStyleBackColor = True
            '
            'OkButton
            '
            Me.OkButton.Location = New System.Drawing.Point(257, 91)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 13
            Me.OkButton.Text = "OK"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'NameLabel
            '
            Me.NameLabel.AutoSize = True
            Me.NameLabel.Location = New System.Drawing.Point(105, 56)
            Me.NameLabel.Name = "NameLabel"
            Me.NameLabel.Size = New System.Drawing.Size(38, 13)
            Me.NameLabel.TabIndex = 12
            Me.NameLabel.Text = "Name:"
            '
            'NameTextBox
            '
            Me.NameTextBox.Location = New System.Drawing.Point(149, 49)
            Me.NameTextBox.Name = "NameTextBox"
            Me.NameTextBox.Size = New System.Drawing.Size(264, 20)
            Me.NameTextBox.TabIndex = 11
            '
            'InstructionsLabel
            '
            Me.InstructionsLabel.AutoSize = True
            Me.InstructionsLabel.Location = New System.Drawing.Point(12, 16)
            Me.InstructionsLabel.Name = "InstructionsLabel"
            Me.InstructionsLabel.Size = New System.Drawing.Size(132, 13)
            Me.InstructionsLabel.TabIndex = 10
            Me.InstructionsLabel.Text = "Create new customer with:"
            '
            'CreateNewQbCustomerForm
            '
            Me.ClientSize = New System.Drawing.Size(609, 131)
            Me.Controls.Add(Me.NameRequiredLabel)
            Me.Controls.Add(Me.CancelCreateButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.NameLabel)
            Me.Controls.Add(Me.NameTextBox)
            Me.Controls.Add(Me.InstructionsLabel)
            Me.Name = "CreateNewQbCustomerForm"
            Me.Text = "Create New Quickbooks Customer"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents NameRequiredLabel As System.Windows.Forms.Label
        Friend WithEvents CancelCreateButton As System.Windows.Forms.Button
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents NameLabel As System.Windows.Forms.Label
        Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
        Friend WithEvents InstructionsLabel As System.Windows.Forms.Label
    End Class
End Namespace