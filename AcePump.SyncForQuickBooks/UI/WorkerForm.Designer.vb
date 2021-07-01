Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class WorkerForm
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
            Me.StatusLabel = New System.Windows.Forms.Label()
            Me.WorkerProgressBar = New System.Windows.Forms.ProgressBar()
            Me.ProgressCancelButton = New System.Windows.Forms.Button()
            Me.DeliveryTicketsBackgroundWorker = New System.ComponentModel.BackgroundWorker()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'StatusLabel
            '
            Me.StatusLabel.AutoSize = True
            Me.StatusLabel.Location = New System.Drawing.Point(36, 33)
            Me.StatusLabel.Name = "StatusLabel"
            Me.StatusLabel.Size = New System.Drawing.Size(43, 13)
            Me.StatusLabel.TabIndex = 7
            Me.StatusLabel.Text = "Status: "
            '
            'WorkerProgressBar
            '
            Me.WorkerProgressBar.Location = New System.Drawing.Point(36, 52)
            Me.WorkerProgressBar.Name = "WorkerProgressBar"
            Me.WorkerProgressBar.Size = New System.Drawing.Size(360, 23)
            Me.WorkerProgressBar.Step = 1
            Me.WorkerProgressBar.TabIndex = 6
            '
            'ProgressCancelButton
            '
            Me.ProgressCancelButton.Location = New System.Drawing.Point(321, 81)
            Me.ProgressCancelButton.Name = "ProgressCancelButton"
            Me.ProgressCancelButton.Size = New System.Drawing.Size(75, 23)
            Me.ProgressCancelButton.TabIndex = 5
            Me.ProgressCancelButton.Text = "Cancel"
            Me.ProgressCancelButton.UseVisualStyleBackColor = True
            '
            'DeliveryTicketsBackgroundWorker
            '
            Me.DeliveryTicketsBackgroundWorker.WorkerReportsProgress = True
            Me.DeliveryTicketsBackgroundWorker.WorkerSupportsCancellation = True
            '
            'OkButton
            '
            Me.OkButton.Location = New System.Drawing.Point(321, 81)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 8
            Me.OkButton.Text = "OK"
            Me.OkButton.UseVisualStyleBackColor = True
            Me.OkButton.Visible = False
            '
            'WorkerForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(432, 136)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.StatusLabel)
            Me.Controls.Add(Me.WorkerProgressBar)
            Me.Controls.Add(Me.ProgressCancelButton)
            Me.Name = "WorkerForm"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents StatusLabel As System.Windows.Forms.Label
        Friend WithEvents WorkerProgressBar As System.Windows.Forms.ProgressBar
        Friend WithEvents ProgressCancelButton As System.Windows.Forms.Button
        Friend WithEvents DeliveryTicketsBackgroundWorker As System.ComponentModel.BackgroundWorker
        Friend WithEvents OkButton As System.Windows.Forms.Button
    End Class
End Namespace