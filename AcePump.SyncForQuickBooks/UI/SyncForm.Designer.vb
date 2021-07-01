Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SyncForm
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
            Me.components = New System.ComponentModel.Container()
            Me.btnDownloadTickets = New System.Windows.Forms.Button()
            Me.ChooseQBFileButton = New System.Windows.Forms.Button()
            Me.QBFileExistsLabel = New System.Windows.Forms.Label()
            Me.VersonLabel = New System.Windows.Forms.Label()
            Me.ClearFileButton = New System.Windows.Forms.Button()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnCheckReadyTickets = New System.Windows.Forms.Button()
            Me.btnTestQb = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'btnDownloadTickets
            '
            Me.btnDownloadTickets.Enabled = False
            Me.btnDownloadTickets.Location = New System.Drawing.Point(142, 119)
            Me.btnDownloadTickets.Name = "btnDownloadTickets"
            Me.btnDownloadTickets.Size = New System.Drawing.Size(215, 46)
            Me.btnDownloadTickets.TabIndex = 1
            Me.btnDownloadTickets.Text = "Download Tickets"
            Me.btnDownloadTickets.UseVisualStyleBackColor = True
            '
            'ChooseQBFileButton
            '
            Me.ChooseQBFileButton.Location = New System.Drawing.Point(274, 25)
            Me.ChooseQBFileButton.Name = "ChooseQBFileButton"
            Me.ChooseQBFileButton.Size = New System.Drawing.Size(218, 23)
            Me.ChooseQBFileButton.TabIndex = 0
            Me.ChooseQBFileButton.Text = "Choose Company File"
            Me.ChooseQBFileButton.UseVisualStyleBackColor = True
            '
            'QBFileExistsLabel
            '
            Me.QBFileExistsLabel.AutoSize = True
            Me.QBFileExistsLabel.Location = New System.Drawing.Point(12, 9)
            Me.QBFileExistsLabel.Name = "QBFileExistsLabel"
            Me.QBFileExistsLabel.Size = New System.Drawing.Size(345, 13)
            Me.QBFileExistsLabel.TabIndex = 3
            Me.QBFileExistsLabel.Text = "No Quickbooks Company File chosen. Please choose a file to continue."
            '
            'VersonLabel
            '
            Me.VersonLabel.AutoSize = True
            Me.VersonLabel.Location = New System.Drawing.Point(450, 258)
            Me.VersonLabel.Name = "VersonLabel"
            Me.VersonLabel.Size = New System.Drawing.Size(42, 13)
            Me.VersonLabel.TabIndex = 4
            Me.VersonLabel.Text = "Version"
            '
            'ClearFileButton
            '
            Me.ClearFileButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
            Me.ClearFileButton.Location = New System.Drawing.Point(474, 8)
            Me.ClearFileButton.Name = "ClearFileButton"
            Me.ClearFileButton.Size = New System.Drawing.Size(18, 18)
            Me.ClearFileButton.TabIndex = 5
            Me.ClearFileButton.Text = "X"
            Me.ClearFileButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            Me.ClearFileButton.UseVisualStyleBackColor = True
            Me.ClearFileButton.Visible = False
            '
            'btnCheckReadyTickets
            '
            Me.btnCheckReadyTickets.Location = New System.Drawing.Point(17, 72)
            Me.btnCheckReadyTickets.Name = "btnCheckReadyTickets"
            Me.btnCheckReadyTickets.Size = New System.Drawing.Size(167, 29)
            Me.btnCheckReadyTickets.TabIndex = 6
            Me.btnCheckReadyTickets.Text = "How many tickets are ready?"
            Me.btnCheckReadyTickets.UseVisualStyleBackColor = True
            Me.btnCheckReadyTickets.Visible = False
            '
            'btnTestQb
            '
            Me.btnTestQb.Location = New System.Drawing.Point(17, 38)
            Me.btnTestQb.Name = "btnTestQb"
            Me.btnTestQb.Size = New System.Drawing.Size(167, 28)
            Me.btnTestQb.TabIndex = 7
            Me.btnTestQb.Text = "Test Qb"
            Me.btnTestQb.UseVisualStyleBackColor = True
            Me.btnTestQb.Visible = False
            '
            'SyncForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(504, 280)
            Me.Controls.Add(Me.btnTestQb)
            Me.Controls.Add(Me.btnCheckReadyTickets)
            Me.Controls.Add(Me.ClearFileButton)
            Me.Controls.Add(Me.VersonLabel)
            Me.Controls.Add(Me.QBFileExistsLabel)
            Me.Controls.Add(Me.ChooseQBFileButton)
            Me.Controls.Add(Me.btnDownloadTickets)
            Me.Name = "SyncForm"
            Me.Text = "Sync online data with Quickbooks"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnDownloadTickets As System.Windows.Forms.Button
        Friend WithEvents ChooseQBFileButton As System.Windows.Forms.Button
        Friend WithEvents QBFileExistsLabel As System.Windows.Forms.Label
        Friend WithEvents VersonLabel As System.Windows.Forms.Label
        Friend WithEvents ClearFileButton As System.Windows.Forms.Button
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Friend WithEvents btnCheckReadyTickets As Button
        Friend WithEvents btnTestQb As Button
    End Class
End Namespace