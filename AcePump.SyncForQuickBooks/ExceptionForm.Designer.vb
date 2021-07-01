<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExceptionForm
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
        Me.ExceptionLabel = New System.Windows.Forms.Label()
        Me.PathToLogFileLink = New System.Windows.Forms.LinkLabel()
        Me.CloseFormButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ExceptionLabel
        '
        Me.ExceptionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.ExceptionLabel.Location = New System.Drawing.Point(13, 13)
        Me.ExceptionLabel.Name = "ExceptionLabel"
        Me.ExceptionLabel.Size = New System.Drawing.Size(405, 80)
        Me.ExceptionLabel.TabIndex = 0
        Me.ExceptionLabel.Text = "Label1"
        Me.ExceptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PathToLogFileLink
        '
        Me.PathToLogFileLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.PathToLogFileLink.Location = New System.Drawing.Point(13, 93)
        Me.PathToLogFileLink.Name = "PathToLogFileLink"
        Me.PathToLogFileLink.Size = New System.Drawing.Size(402, 58)
        Me.PathToLogFileLink.TabIndex = 1
        Me.PathToLogFileLink.TabStop = True
        Me.PathToLogFileLink.Text = "LinkLabel1"
        Me.PathToLogFileLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CloseFormButton
        '
        Me.CloseFormButton.Location = New System.Drawing.Point(343, 154)
        Me.CloseFormButton.Name = "CloseFormButton"
        Me.CloseFormButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseFormButton.TabIndex = 2
        Me.CloseFormButton.Text = "OK"
        Me.CloseFormButton.UseVisualStyleBackColor = True
        '
        'ExceptionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(430, 193)
        Me.Controls.Add(Me.CloseFormButton)
        Me.Controls.Add(Me.PathToLogFileLink)
        Me.Controls.Add(Me.ExceptionLabel)
        Me.Name = "ExceptionForm"
        Me.Text = "ExceptionForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExceptionLabel As System.Windows.Forms.Label
    Friend WithEvents PathToLogFileLink As System.Windows.Forms.LinkLabel
    Friend WithEvents CloseFormButton As System.Windows.Forms.Button
End Class
