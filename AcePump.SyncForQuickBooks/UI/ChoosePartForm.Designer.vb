Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ChoosePartForm
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
            Me.ChooseGroupBox = New System.Windows.Forms.GroupBox()
            Me.SelectedLabel = New System.Windows.Forms.Label()
            Me.WarningLoadListLabel = New System.Windows.Forms.Label()
            Me.PopulateButton = New System.Windows.Forms.Button()
            Me.ChooseGridView = New System.Windows.Forms.DataGridView()
            Me.SelectedTitleLabel = New System.Windows.Forms.Label()
            Me.ChooseOkButton = New System.Windows.Forms.Button()
            Me.CreateNewGroupBox = New System.Windows.Forms.GroupBox()
            Me.LookupSingleInstructionLabel = New System.Windows.Forms.Label()
            Me.CheckSpecificPartButton = New System.Windows.Forms.Button()
            Me.TitleGroupBox = New System.Windows.Forms.GroupBox()
            Me.ChooseInstructionLabel = New System.Windows.Forms.Label()
            Me.ChooseGroupBox.SuspendLayout()
            CType(Me.ChooseGridView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CreateNewGroupBox.SuspendLayout()
            Me.TitleGroupBox.SuspendLayout()
            Me.SuspendLayout()
            '
            'ChooseGroupBox
            '
            Me.ChooseGroupBox.Controls.Add(Me.SelectedLabel)
            Me.ChooseGroupBox.Controls.Add(Me.WarningLoadListLabel)
            Me.ChooseGroupBox.Controls.Add(Me.PopulateButton)
            Me.ChooseGroupBox.Controls.Add(Me.ChooseGridView)
            Me.ChooseGroupBox.Controls.Add(Me.SelectedTitleLabel)
            Me.ChooseGroupBox.Controls.Add(Me.ChooseOkButton)
            Me.ChooseGroupBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChooseGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.ChooseGroupBox.Location = New System.Drawing.Point(0, 152)
            Me.ChooseGroupBox.Name = "ChooseGroupBox"
            Me.ChooseGroupBox.Size = New System.Drawing.Size(581, 334)
            Me.ChooseGroupBox.TabIndex = 2
            Me.ChooseGroupBox.TabStop = False
            Me.ChooseGroupBox.Text = "2) Load entire part list"
            '
            'SelectedLabel
            '
            Me.SelectedLabel.Location = New System.Drawing.Point(401, 80)
            Me.SelectedLabel.Name = "SelectedLabel"
            Me.SelectedLabel.Size = New System.Drawing.Size(147, 144)
            Me.SelectedLabel.TabIndex = 7
            '
            'WarningLoadListLabel
            '
            Me.WarningLoadListLabel.AutoSize = True
            Me.WarningLoadListLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.WarningLoadListLabel.Location = New System.Drawing.Point(8, 24)
            Me.WarningLoadListLabel.Name = "WarningLoadListLabel"
            Me.WarningLoadListLabel.Size = New System.Drawing.Size(295, 13)
            Me.WarningLoadListLabel.TabIndex = 9
            Me.WarningLoadListLabel.Text = "WARNING! It can take up to 5 minutes to load entire part list!"
            '
            'PopulateButton
            '
            Me.PopulateButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.PopulateButton.Location = New System.Drawing.Point(429, 19)
            Me.PopulateButton.Name = "PopulateButton"
            Me.PopulateButton.Size = New System.Drawing.Size(119, 23)
            Me.PopulateButton.TabIndex = 8
            Me.PopulateButton.Text = "Load parts list"
            Me.PopulateButton.UseVisualStyleBackColor = True
            '
            'ChooseGridView
            '
            Me.ChooseGridView.AllowUserToAddRows = False
            Me.ChooseGridView.AllowUserToDeleteRows = False
            Me.ChooseGridView.AllowUserToResizeRows = False
            Me.ChooseGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ChooseGridView.ColumnHeadersVisible = False
            Me.ChooseGridView.Location = New System.Drawing.Point(11, 58)
            Me.ChooseGridView.MultiSelect = False
            Me.ChooseGridView.Name = "ChooseGridView"
            Me.ChooseGridView.ReadOnly = True
            Me.ChooseGridView.RowHeadersVisible = False
            Me.ChooseGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChooseGridView.Size = New System.Drawing.Size(381, 226)
            Me.ChooseGridView.TabIndex = 5
            '
            'SelectedTitleLabel
            '
            Me.SelectedTitleLabel.AutoSize = True
            Me.SelectedTitleLabel.Location = New System.Drawing.Point(401, 58)
            Me.SelectedTitleLabel.Name = "SelectedTitleLabel"
            Me.SelectedTitleLabel.Size = New System.Drawing.Size(0, 15)
            Me.SelectedTitleLabel.TabIndex = 6
            '
            'ChooseOkButton
            '
            Me.ChooseOkButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.ChooseOkButton.Location = New System.Drawing.Point(429, 261)
            Me.ChooseOkButton.Name = "ChooseOkButton"
            Me.ChooseOkButton.Size = New System.Drawing.Size(119, 23)
            Me.ChooseOkButton.TabIndex = 5
            Me.ChooseOkButton.Text = "OK"
            Me.ChooseOkButton.UseVisualStyleBackColor = True
            Me.ChooseOkButton.Visible = False
            '
            'CreateNewGroupBox
            '
            Me.CreateNewGroupBox.Controls.Add(Me.LookupSingleInstructionLabel)
            Me.CreateNewGroupBox.Controls.Add(Me.CheckSpecificPartButton)
            Me.CreateNewGroupBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.CreateNewGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.CreateNewGroupBox.Location = New System.Drawing.Point(0, 94)
            Me.CreateNewGroupBox.Name = "CreateNewGroupBox"
            Me.CreateNewGroupBox.Size = New System.Drawing.Size(581, 58)
            Me.CreateNewGroupBox.TabIndex = 4
            Me.CreateNewGroupBox.TabStop = False
            Me.CreateNewGroupBox.Text = "1) Lookup single part number"
            '
            'LookupSingleInstructionLabel
            '
            Me.LookupSingleInstructionLabel.AutoSize = True
            Me.LookupSingleInstructionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.LookupSingleInstructionLabel.Location = New System.Drawing.Point(8, 20)
            Me.LookupSingleInstructionLabel.Name = "LookupSingleInstructionLabel"
            Me.LookupSingleInstructionLabel.Size = New System.Drawing.Size(206, 13)
            Me.LookupSingleInstructionLabel.TabIndex = 5
            Me.LookupSingleInstructionLabel.Text = "Lookup single one you know of or created"
            '
            'CheckSpecificPartButton
            '
            Me.CheckSpecificPartButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.CheckSpecificPartButton.Location = New System.Drawing.Point(429, 19)
            Me.CheckSpecificPartButton.Name = "CheckSpecificPartButton"
            Me.CheckSpecificPartButton.Size = New System.Drawing.Size(119, 23)
            Me.CheckSpecificPartButton.TabIndex = 4
            Me.CheckSpecificPartButton.Text = "Check Quickbooks"
            Me.CheckSpecificPartButton.UseVisualStyleBackColor = True
            '
            'TitleGroupBox
            '
            Me.TitleGroupBox.Controls.Add(Me.ChooseInstructionLabel)
            Me.TitleGroupBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.TitleGroupBox.Location = New System.Drawing.Point(0, 0)
            Me.TitleGroupBox.Name = "TitleGroupBox"
            Me.TitleGroupBox.Size = New System.Drawing.Size(581, 94)
            Me.TitleGroupBox.TabIndex = 6
            Me.TitleGroupBox.TabStop = False
            '
            'ChooseInstructionLabel
            '
            Me.ChooseInstructionLabel.Dock = System.Windows.Forms.DockStyle.Top
            Me.ChooseInstructionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.ChooseInstructionLabel.Location = New System.Drawing.Point(3, 16)
            Me.ChooseInstructionLabel.Name = "ChooseInstructionLabel"
            Me.ChooseInstructionLabel.Size = New System.Drawing.Size(575, 51)
            Me.ChooseInstructionLabel.TabIndex = 1
            Me.ChooseInstructionLabel.Text = "We could not find part XYZ"
            '
            'ChoosePartForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(581, 486)
            Me.Controls.Add(Me.ChooseGroupBox)
            Me.Controls.Add(Me.CreateNewGroupBox)
            Me.Controls.Add(Me.TitleGroupBox)
            Me.Name = "ChoosePartForm"
            Me.Text = "Choose  Part"
            Me.ChooseGroupBox.ResumeLayout(False)
            Me.ChooseGroupBox.PerformLayout()
            CType(Me.ChooseGridView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CreateNewGroupBox.ResumeLayout(False)
            Me.CreateNewGroupBox.PerformLayout()
            Me.TitleGroupBox.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ChooseGroupBox As System.Windows.Forms.GroupBox
        Friend WithEvents SelectedTitleLabel As System.Windows.Forms.Label
        Friend WithEvents ChooseOkButton As System.Windows.Forms.Button
        Friend WithEvents CreateNewGroupBox As System.Windows.Forms.GroupBox
        Friend WithEvents LookupSingleInstructionLabel As System.Windows.Forms.Label
        Friend WithEvents CheckSpecificPartButton As System.Windows.Forms.Button
        Friend WithEvents SelectedLabel As System.Windows.Forms.Label
        Friend WithEvents ChooseGridView As System.Windows.Forms.DataGridView
        Friend WithEvents WarningLoadListLabel As System.Windows.Forms.Label
        Friend WithEvents PopulateButton As System.Windows.Forms.Button
        Friend WithEvents TitleGroupBox As System.Windows.Forms.GroupBox
        Friend WithEvents ChooseInstructionLabel As System.Windows.Forms.Label
    End Class
End Namespace