Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class RunningInvoicePromptDialog
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
            Me.StartNewInvoiceButton = New System.Windows.Forms.Button()
            Me.UseThisInvoice = New System.Windows.Forms.Button()
            Me.HeaderLabel = New System.Windows.Forms.Label()
            Me.InvoiceNumberTextBox = New System.Windows.Forms.TextBox()
            Me.validationLabel = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'StartNewInvoiceButton
            '
            Me.StartNewInvoiceButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.StartNewInvoiceButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.StartNewInvoiceButton.Location = New System.Drawing.Point(424, 151)
            Me.StartNewInvoiceButton.Name = "StartNewInvoiceButton"
            Me.StartNewInvoiceButton.Size = New System.Drawing.Size(109, 23)
            Me.StartNewInvoiceButton.TabIndex = 0
            Me.StartNewInvoiceButton.Text = "Start New Invoice"
            Me.StartNewInvoiceButton.UseVisualStyleBackColor = True
            '
            'UseThisInvoice
            '
            Me.UseThisInvoice.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.UseThisInvoice.Location = New System.Drawing.Point(269, 151)
            Me.UseThisInvoice.Name = "UseThisInvoice"
            Me.UseThisInvoice.Size = New System.Drawing.Size(140, 23)
            Me.UseThisInvoice.TabIndex = 1
            Me.UseThisInvoice.Text = "Use This Invoice"
            Me.UseThisInvoice.UseVisualStyleBackColor = True
            '
            'HeaderLabel
            '
            Me.HeaderLabel.Dock = System.Windows.Forms.DockStyle.Top
            Me.HeaderLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, CType(177, Byte))
            Me.HeaderLabel.Location = New System.Drawing.Point(0, 0)
            Me.HeaderLabel.Name = "HeaderLabel"
            Me.HeaderLabel.Padding = New System.Windows.Forms.Padding(10, 20, 0, 0)
            Me.HeaderLabel.Size = New System.Drawing.Size(545, 74)
            Me.HeaderLabel.TabIndex = 2
            Me.HeaderLabel.Text = "Label1"
            '
            'InvoiceNumberTextBox
            '
            Me.InvoiceNumberTextBox.Location = New System.Drawing.Point(53, 86)
            Me.InvoiceNumberTextBox.Name = "InvoiceNumberTextBox"
            Me.InvoiceNumberTextBox.Size = New System.Drawing.Size(219, 20)
            Me.InvoiceNumberTextBox.TabIndex = 3
            '
            'validationLabel
            '
            Me.validationLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.validationLabel.AutoSize = True
            Me.validationLabel.ForeColor = System.Drawing.Color.Red
            Me.validationLabel.Location = New System.Drawing.Point(279, 86)
            Me.validationLabel.MaximumSize = New System.Drawing.Size(250, 50)
            Me.validationLabel.Name = "validationLabel"
            Me.validationLabel.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
            Me.validationLabel.Size = New System.Drawing.Size(225, 39)
            Me.validationLabel.TabIndex = 4
            Me.validationLabel.Text = " Label1 Label1 Label1 Label1 Label1 Label1 Label1 Label1 Label1 Label1 Label1 Lab" & _
        "el1 Label1 Label1 Label1 Label1 Label1 Label1"
            Me.validationLabel.Visible = False
            '
            'RunningInvoicePromptDialog
            '
            Me.AcceptButton = Me.UseThisInvoice
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.StartNewInvoiceButton
            Me.ClientSize = New System.Drawing.Size(545, 186)
            Me.Controls.Add(Me.validationLabel)
            Me.Controls.Add(Me.InvoiceNumberTextBox)
            Me.Controls.Add(Me.HeaderLabel)
            Me.Controls.Add(Me.UseThisInvoice)
            Me.Controls.Add(Me.StartNewInvoiceButton)
            Me.Name = "RunningInvoicePromptDialog"
            Me.Text = "RunningInvoiceForm"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents StartNewInvoiceButton As System.Windows.Forms.Button
        Friend WithEvents UseThisInvoice As System.Windows.Forms.Button
        Friend WithEvents HeaderLabel As System.Windows.Forms.Label
        Friend WithEvents InvoiceNumberTextBox As System.Windows.Forms.TextBox
        Friend WithEvents validationLabel As System.Windows.Forms.Label
    End Class
End Namespace