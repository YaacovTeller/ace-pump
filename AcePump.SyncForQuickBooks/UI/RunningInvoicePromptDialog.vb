Imports AcePump.SyncForQuickBooks.UI.Models
Imports System.Threading
Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.PtpApi.Models

Namespace UI
    Public Class RunningInvoicePromptDialog
        Private _currentTicket As DeliveryTicketModel
        Private _reporter As UIProgressReporter
        Private _token As CancellationToken
        Public RunningInvoiceSearchResult As New RunningInvoiceSearchResult With {.Status = RunningInvoiceSearchStatus.None}
        Public _invalidInvoiceNumbers As New List(Of String)

        Public Sub PromptUserFor(deliveryTicket As DeliveryTicketModel, reporter As UIProgressReporter, token As CancellationToken)
            _currentTicket = deliveryTicket
            _reporter = reporter
            _token = token
            HeaderLabel.Text = String.Format("Deliveryticket {0} with customer {1} uses running invoices. {2}" & _
                                             "Please enter the number for an existing QuickBooks invoice to use or start a new one.", _
                                             deliveryTicket.DeliveryTicketID, deliveryTicket.CustomerName, vbNewLine)

        End Sub

        Private Sub ProcessUserInputInvoiceNumber()
            Cursor.Current = Cursors.WaitCursor
            Dim invoiceNumberToProcess As String = InvoiceNumberTextBox.Text
            If ValidateInvoiceNumber(invoiceNumberToProcess) Then
                Dim invoiceSearchResult As New RunningInvoiceSearchResult
                invoiceSearchResult = QbDeliveryTicketSyncer.GetInvoiceFor(invoiceNumberToProcess, _currentTicket.CustomerQuickbooksID, _reporter, _token)
                If invoiceSearchResult.Status = RunningInvoiceSearchStatus.Failed Then
                    _invalidInvoiceNumbers.Add(invoiceNumberToProcess)
                    ValidateInvoiceNumber(invoiceNumberToProcess)
                ElseIf invoiceSearchResult.Status = RunningInvoiceSearchStatus.Found Then
                    RunningInvoiceSearchResult = invoiceSearchResult
                    Close()
                End If
            End If
            Cursor.Current = Cursors.Default
        End Sub

        Private Function ValidateInvoiceNumber(invoiceNumbertoProcess) As Boolean
            If String.IsNullOrWhiteSpace(invoiceNumbertoProcess) Then
                validationLabel.Text = "Please enter an invoice number."
                validationLabel.Show()
                Return False
            ElseIf _invalidInvoiceNumbers.Contains(invoiceNumbertoProcess) Then
                validationLabel.Text = "This invoice number is not valid. Either it does not exist or it does not belong to the current customer." & vbNewLine & _
                                       "Please enter a different invoice number."
                validationLabel.Show()
                Return False
            Else
                validationLabel.Hide()
                Return True
            End If
        End Function

        Private Sub UseNewInvoice()
            RunningInvoiceSearchResult.Status = RunningInvoiceSearchStatus.CreateNew
            Me.Close()
        End Sub

        Private Sub ChooseCustomerForm_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If RunningInvoiceSearchResult.Status = RunningInvoiceSearchStatus.None Then
                Dim result As DialogResult = MessageBox.Show("You have not entered an invoice Number. If you close now, this ticket will create a new invoice in QuickBooks." & vbNewLine & _
                                                             "Are you sure you want to close?", "Running Invoice", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    e.Cancel = True
                Else
                    RunningInvoiceSearchResult.Status = RunningInvoiceSearchStatus.CreateNew
                End If
            End If
        End Sub

        Private Sub StartNewInvoiceButton_Click(sender As Object, e As System.EventArgs) Handles StartNewInvoiceButton.Click
            UseNewInvoice()
        End Sub

        Private Sub UseThisInvoice_Click(sender As Object, e As System.EventArgs) Handles UseThisInvoice.Click
            ProcessUserInputInvoiceNumber()
        End Sub
    End Class
End Namespace