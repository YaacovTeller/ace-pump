Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ChooseCustomerForm
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
            Me.ChooseInstructionLabel = New System.Windows.Forms.Label()
            Me.ChooseGroupBox = New System.Windows.Forms.GroupBox()
            Me.ChooseGridView = New System.Windows.Forms.DataGridView()
            Me.SelectedLabel = New System.Windows.Forms.Label()
            Me.SelectedTitleLabel = New System.Windows.Forms.Label()
            Me.ChooseOkButton = New System.Windows.Forms.Button()
            Me.CreateNewGroupBox = New System.Windows.Forms.GroupBox()
            Me.CreateNewInstructionLabel = New System.Windows.Forms.Label()
            Me.CreateNewButton = New System.Windows.Forms.Button()
            Me.ChooseGroupBox.SuspendLayout()
            CType(Me.ChooseGridView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CreateNewGroupBox.SuspendLayout()
            Me.SuspendLayout()
            '
            'ChooseInstructionLabel
            '
            Me.ChooseInstructionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.ChooseInstructionLabel.Location = New System.Drawing.Point(13, 13)
            Me.ChooseInstructionLabel.Name = "ChooseInstructionLabel"
            Me.ChooseInstructionLabel.Size = New System.Drawing.Size(557, 51)
            Me.ChooseInstructionLabel.TabIndex = 0
            Me.ChooseInstructionLabel.Text = "We could not find customer XYZ for ticket 123 in Quickbooks. Please choose the co" & _
        "rrect customer from the list or create a new one to add to Quickbooks."
            '
            'ChooseGroupBox
            '
            Me.ChooseGroupBox.Controls.Add(Me.ChooseGridView)
            Me.ChooseGroupBox.Controls.Add(Me.SelectedLabel)
            Me.ChooseGroupBox.Controls.Add(Me.SelectedTitleLabel)
            Me.ChooseGroupBox.Controls.Add(Me.ChooseOkButton)
            Me.ChooseGroupBox.Location = New System.Drawing.Point(16, 67)
            Me.ChooseGroupBox.Name = "ChooseGroupBox"
            Me.ChooseGroupBox.Size = New System.Drawing.Size(554, 263)
            Me.ChooseGroupBox.TabIndex = 2
            Me.ChooseGroupBox.TabStop = False
            Me.ChooseGroupBox.Text = "Choose"
            '
            'ChooseGridView
            '
            Me.ChooseGridView.AllowUserToAddRows = False
            Me.ChooseGridView.AllowUserToDeleteRows = False
            Me.ChooseGridView.AllowUserToResizeRows = False
            Me.ChooseGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ChooseGridView.ColumnHeadersVisible = False
            Me.ChooseGridView.Location = New System.Drawing.Point(11, 22)
            Me.ChooseGridView.MultiSelect = False
            Me.ChooseGridView.Name = "ChooseGridView"
            Me.ChooseGridView.ReadOnly = True
            Me.ChooseGridView.RowHeadersVisible = False
            Me.ChooseGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChooseGridView.Size = New System.Drawing.Size(381, 226)
            Me.ChooseGridView.TabIndex = 5
            '
            'SelectedLabel
            '
            Me.SelectedLabel.Location = New System.Drawing.Point(401, 44)
            Me.SelectedLabel.Name = "SelectedLabel"
            Me.SelectedLabel.Size = New System.Drawing.Size(147, 144)
            Me.SelectedLabel.TabIndex = 7
            Me.SelectedLabel.Text = "Selected:"
            '
            'SelectedTitleLabel
            '
            Me.SelectedTitleLabel.AutoSize = True
            Me.SelectedTitleLabel.Location = New System.Drawing.Point(401, 22)
            Me.SelectedTitleLabel.Name = "SelectedTitleLabel"
            Me.SelectedTitleLabel.Size = New System.Drawing.Size(52, 13)
            Me.SelectedTitleLabel.TabIndex = 6
            Me.SelectedTitleLabel.Text = "Selected:"
            '
            'ChooseOkButton
            '
            Me.ChooseOkButton.Location = New System.Drawing.Point(473, 191)
            Me.ChooseOkButton.Name = "ChooseOkButton"
            Me.ChooseOkButton.Size = New System.Drawing.Size(75, 23)
            Me.ChooseOkButton.TabIndex = 5
            Me.ChooseOkButton.Text = "OK"
            Me.ChooseOkButton.UseVisualStyleBackColor = True
            '
            'CreateNewGroupBox
            '
            Me.CreateNewGroupBox.Controls.Add(Me.CreateNewInstructionLabel)
            Me.CreateNewGroupBox.Controls.Add(Me.CreateNewButton)
            Me.CreateNewGroupBox.Location = New System.Drawing.Point(16, 337)
            Me.CreateNewGroupBox.Name = "CreateNewGroupBox"
            Me.CreateNewGroupBox.Size = New System.Drawing.Size(554, 58)
            Me.CreateNewGroupBox.TabIndex = 4
            Me.CreateNewGroupBox.TabStop = False
            Me.CreateNewGroupBox.Text = "Create New"
            '
            'CreateNewInstructionLabel
            '
            Me.CreateNewInstructionLabel.AutoSize = True
            Me.CreateNewInstructionLabel.Location = New System.Drawing.Point(8, 24)
            Me.CreateNewInstructionLabel.Name = "CreateNewInstructionLabel"
            Me.CreateNewInstructionLabel.Size = New System.Drawing.Size(440, 13)
            Me.CreateNewInstructionLabel.TabIndex = 5
            Me.CreateNewInstructionLabel.Text = "Couldn't find the customer you're looking for? Click here to create a new one in " & _
        "Quickbooks:"
            '
            'CreateNewButton
            '
            Me.CreateNewButton.Location = New System.Drawing.Point(473, 19)
            Me.CreateNewButton.Name = "CreateNewButton"
            Me.CreateNewButton.Size = New System.Drawing.Size(75, 23)
            Me.CreateNewButton.TabIndex = 4
            Me.CreateNewButton.Text = "Create New"
            Me.CreateNewButton.UseVisualStyleBackColor = True
            '
            'ChooseCustomerForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(583, 407)
            Me.Controls.Add(Me.CreateNewGroupBox)
            Me.Controls.Add(Me.ChooseGroupBox)
            Me.Controls.Add(Me.ChooseInstructionLabel)
            Me.Name = "ChooseCustomerForm"
            Me.Text = "Choose  customer"
            Me.ChooseGroupBox.ResumeLayout(False)
            Me.ChooseGroupBox.PerformLayout()
            CType(Me.ChooseGridView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CreateNewGroupBox.ResumeLayout(False)
            Me.CreateNewGroupBox.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ChooseInstructionLabel As System.Windows.Forms.Label
        Friend WithEvents ChooseGroupBox As System.Windows.Forms.GroupBox
        Friend WithEvents SelectedTitleLabel As System.Windows.Forms.Label
        Friend WithEvents ChooseOkButton As System.Windows.Forms.Button
        Friend WithEvents CreateNewGroupBox As System.Windows.Forms.GroupBox
        Friend WithEvents CreateNewInstructionLabel As System.Windows.Forms.Label
        Friend WithEvents CreateNewButton As System.Windows.Forms.Button
        Friend WithEvents SelectedLabel As System.Windows.Forms.Label
        Friend WithEvents ChooseGridView As System.Windows.Forms.DataGridView
    End Class
End Namespace