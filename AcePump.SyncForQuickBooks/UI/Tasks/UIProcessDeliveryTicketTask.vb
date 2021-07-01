Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports AcePump.SyncForQuickBooks.PtpApi
Imports AcePump.SyncForQuickBooks.PtpApi.Models
Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.Qb.Models
Imports AcePump.SyncForQuickBooks.UI.Models
Imports AcePump.SyncForQuickBooks.UI.Validation

Namespace UI.Tasks
    Public Class UIProcessDeliveryTicketTask
        Implements IUITask

        Private Property WorkerFormModel As WorkerFormModel
        Private _changeOnProgressList As List(Of UIChangeOnProgress) = Nothing
        Private _taskAction As Action = Nothing
        Private Shared _runningInvoices As New List(Of InvoiceUIModel)
        Private _reporter As UIProgressReporter
        Private _token As CancellationToken

        Public ReadOnly Property ChangeOnProgressList As List(Of UIChangeOnProgress) Implements IUITask.ChangeOnProgressList
            Get
                If _changeOnProgressList Is Nothing Then
                    _changeOnProgressList = New List(Of UIChangeOnProgress) From {
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Start,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Operation Begun",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Zero},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Canceled,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Cancelled",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Canceling,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Canceling....",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 0},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Failure,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Failed!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Zero},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.NoTicketsToProcess,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "No tickets to process!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ConnectingToApi,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Connecting to Pump Tracking Programm online.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 1},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ReadingTicketsFromApi,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Reading tickets from Pump Tracking Programm.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 2},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.DownloadingTicketsFromApi,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Downloading tickets from Pump Tracking Programm.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 2},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ValidatingTickets,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Validating ticket ",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 10},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ConnectingToQb,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Connecting to Quickbooks.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 1},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ProcessingNewInvoices,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Processing and creating new invoice ",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ProcessingModifiedInvoices,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Processing and modifying invoice ",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.SavingTickets,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Saving to Quickbooks ",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.ProcessingTicketAdditionalData,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Processing additional data for invoice ",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.LoadingCustomerList,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Loading customers from Quickbooks. This might take a while.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.LoadingPartList,
                                             .OkButtonVisible = False,
                                             .ProgressCancelButtonEnabled = True,
                                             .StatusLabelText = "Loading parts from Quickbooks. This might take a while.",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.AddPercentage,
                                             .ProgressWeightOutOf100 = 5},
                New UIChangeOnProgress With {.ProgressStage = UIProgressStage.Complete,
                                             .OkButtonVisible = True,
                                             .ProgressCancelButtonEnabled = False,
                                             .StatusLabelText = "Operation Complete!",
                                             .WorkerProgressBarValue = UIChangeProgressBarState.Maximum,
                                             .ProgressWeightOutOf100 = 5}
                }
                    Return _changeOnProgressList
                Else
                    Return _changeOnProgressList
                End If
            End Get
        End Property

        Public Sub New(reporter As UIProgressReporter, token As CancellationToken)
            _reporter = reporter
            _token = token
            WorkerFormModel = New WorkerFormModel()
        End Sub

        Public ReadOnly Property TaskAction As Action Implements IUITask.TaskAction
            Get
                If _taskAction Is Nothing Then
                    _taskAction = Sub() ProcessDeliverytickets(WorkerFormModel, _reporter, _token)
                    Return _taskAction
                Else
                    Return _taskAction
                End If
            End Get
        End Property

        Public Shared Sub ProcessDeliverytickets(uiModel As WorkerFormModel, reporter As UIProgressReporter, token As CancellationToken)
            reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Start})

            Dim receivedTickets As List(Of DeliveryTicketModel)
            _runningInvoices.Clear()

            Dim downloadTask As Task = Task.Factory.StartNew(Sub() receivedTickets = ApiConnector.DownloadDeliverytickets(reporter, token), token)
            downloadTask.Wait()

            If receivedTickets Is Nothing OrElse receivedTickets.Count = 0 Then
                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.NoTicketsToProcess})

            Else
                Dim processingContexts As List(Of DeliveryTicketProcessingContext) = (From ticket In receivedTickets
                                                                                      Select New DeliveryTicketProcessingContext(uiModel, ticket)
                                                                                      ).ToList()

                ValidateTickets(processingContexts, receivedTickets, reporter, token)

                Dim validContexts = From context In processingContexts
                                    Where context.ValidationContext.DeliveryTicketValidationResult.IsValid
                                    Select context.QbSyncContext

                QbDeliveryTicketSyncer.SyncDeliveryTicketsWithQb(validContexts, _runningInvoices, reporter, token)

                Dim summaryBuilder As New StringBuilder
                For Each processingContext As DeliveryTicketProcessingContext In processingContexts
                    If processingContext.QbSyncContext.AddedToQuickbooks Then
                        Dim response = ApiConnector.PostToQbApi("PostSingleDeliveryTicketQuickbooksID",
                                                 New With {.QbID = processingContext.DeliveryTicket.QuickbooksID,
                                                           .DeliveryTicketID = processingContext.DeliveryTicket.DeliveryTicketID,
                                                           .QbInvoiceNumber = processingContext.DeliveryTicket.QuickbooksInvoiceNumber},
                                                  reporter,
                                                  token)

                        processingContext.SendingQbSyncInfoToPtpFailed = Not response.IsSuccessStatusCode
                    End If

                    AddToSummary(summaryBuilder, processingContext)
                Next

                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.Complete})

                DisplaySummary(summaryBuilder)
            End If
        End Sub

        Private Shared Sub ValidateTickets(ByVal contexts As IEnumerable(Of DeliveryTicketProcessingContext), ByVal receivedTickets As List(Of DeliveryTicketModel), ByVal reporter As UIProgressReporter, ByVal token As CancellationToken)
            token.ThrowIfCancellationRequested()
            LookupMissingQbCustomerIDs(receivedTickets, reporter, token)

            token.ThrowIfCancellationRequested()
            LookupMissingQbPartIDs(receivedTickets, reporter, token)
            Dim upto As Integer = 0
            For Each deliveryTicket In receivedTickets
                token.ThrowIfCancellationRequested()
                upto += 1
                reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.ValidatingTickets,
                                                                .Data = New Dictionary(Of String, Object) From {{"Upto", upto}, {"TotalTickets", receivedTickets.Count}}})

                Dim currentContext As DeliveryTicketProcessingContext = contexts.First(Function(x) x.DeliveryTicket.Equals(deliveryTicket))
                Dim results As Dictionary(Of DeliveryTicketValidationFailureReason, String) = ValidateSingleTicket(currentContext.ValidationContext, deliveryTicket, reporter, token)
                If results.Count() = 0 Then
                    GatherRunningInvoiceInformation(deliveryTicket, reporter, token)

                    For Each lineItem As LineItemModel In deliveryTicket.LineItems
                        token.ThrowIfCancellationRequested()
                        ValidateSingleLineItem(currentContext.ValidationContext, lineItem, deliveryTicket.DeliveryTicketID, reporter, token)
                    Next
                End If
            Next
        End Sub

        Private Shared Sub LookupMissingQbCustomerIDs(ByVal receivedTickets As List(Of DeliveryTicketModel), reporter As UIProgressReporter, token As CancellationToken)
            Dim qbCustomerLookupList As List(Of QbCustomerModel) = QbCustomerIDManager.GetQbCustomerIDLookup(receivedTickets, token)

            If qbCustomerLookupList IsNot Nothing AndAlso qbCustomerLookupList.Count > 0 Then
                Dim ticket As DeliveryTicketModel
                Dim name As String
                For Each customerModel As QbCustomerModel In qbCustomerLookupList
                    name = customerModel.Name
                    Dim tickets = receivedTickets.Where(Function(x) x.CustomerName = name)
                    For Each ticket In tickets
                        ticket.CustomerQuickbooksID = customerModel.ListID
                    Next
                Next

                token.ThrowIfCancellationRequested()
                Dim updatedCustomers = From t In receivedTickets
                                       Where qbCustomerLookupList.Any(Function(x) x.ListID = t.CustomerQuickbooksID)
                                       Select New With {
                                           .CustomerID = t.CustomerID,
                                           .QbID = t.CustomerQuickbooksID
                                       }
                ApiConnector.PostToQbApi("PostCustomerQuickbooksIDs", updatedCustomers, reporter, token)
            End If
        End Sub

        Private Shared Sub LookupMissingQbPartIDs(ByVal receivedTickets As List(Of DeliveryTicketModel), reporter As UIProgressReporter, token As CancellationToken)
            Dim qbPartLookupList As List(Of QbItemModel) = QbPartIDManager.GetQbPartIDLookup(receivedTickets, token)

            If qbPartLookupList IsNot Nothing AndAlso qbPartLookupList.Count > 0 Then
                Dim lineItems = receivedTickets.SelectMany(Function(x) x.LineItems).ToList()

                Dim lineItem As LineItemModel
                For Each partModel As QbItemModel In qbPartLookupList
                    Dim matchingLineItems = lineItems.Where(Function(x) x.PartTemplateNumber = partModel.Name)
                    For Each lineItem In matchingLineItems
                        lineItem.PartQuickbooksID = partModel.ListID
                    Next
                Next

                token.ThrowIfCancellationRequested()
                Dim updatedLineItems = From li In lineItems
                                       Where qbPartLookupList.Any(Function(x) x.ListID = li.PartQuickbooksID)
                                       Select New With {
                                           .PartTemplateID = li.PartTemplateID,
                                           .QbID = li.PartQuickbooksID
                                       }
                ApiConnector.PostToQbApi("PostPartQuickbooksIDs", updatedLineItems, reporter, token)
            End If
        End Sub

        Public Shared Function GetUserChosenCustomerIDFromDialog(deliveryTicket As DeliveryTicketModel) As String
            Using chooseCustomerIDDialog As New ChooseCustomerForm
                chooseCustomerIDDialog.SetValues(deliveryTicket.CustomerName, deliveryTicket.DeliveryTicketID)
                Dim dialogResult = chooseCustomerIDDialog.ShowDialog()
                If dialogResult = DialogResult.OK Then
                    Return chooseCustomerIDDialog.ChosenCustomerQbListID

                Else
                    Return Nothing
                End If
            End Using
        End Function

        Private Shared Function ValidateSingleTicket(ByVal context As ValidationContext, ByVal deliveryTicket As DeliveryTicketModel, ByVal reporter As UIProgressReporter, ByVal token As CancellationToken) As Dictionary(Of DeliveryTicketValidationFailureReason, String)
            Dim results As Dictionary(Of DeliveryTicketValidationFailureReason, String) = QbDeliveryTicketValidator.ValidateTicket(deliveryTicket)
            If results.Count > 0 Then
                If results.ContainsKey(DeliveryTicketValidationFailureReason.MissingQbCustomerID) Then
                    If context.WorkerFormModel.QbCustomerCache.ContainsKey(deliveryTicket.CustomerID.Value) Then
                        deliveryTicket.CustomerQuickbooksID = context.WorkerFormModel.QbCustomerCache(deliveryTicket.CustomerID.Value)

                    Else
                        token.ThrowIfCancellationRequested()
                        reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.LoadingCustomerList})
                        Dim customerQbId As String = GetUserChosenCustomerIDFromDialog(deliveryTicket)
                        If customerQbId IsNot Nothing Then
                            deliveryTicket.CustomerQuickbooksID = customerQbId
                            context.WorkerFormModel.QbCustomerCache.Add(deliveryTicket.CustomerID.Value, customerQbId)

                            token.ThrowIfCancellationRequested()
                            ApiConnector.PostToQbApi("PostSingleCustomerQuickbooksID", New With {.CustomerID = deliveryTicket.CustomerID.Value, .QbID = customerQbId}, reporter, token)
                        End If
                    End If
                End If

                If results.ContainsKey(DeliveryTicketValidationFailureReason.MissingCountySalesTaxRateQbID) Then
                    If Not results.ContainsKey(DeliveryTicketValidationFailureReason.MissingCountySalesTaxRateName) Then
                        If context.WorkerFormModel.QbCountyCache.ContainsKey(deliveryTicket.CountySalesTaxRateName) Then
                            deliveryTicket.CountySalesTaxRateQuickbooksID = context.WorkerFormModel.QbCountyCache(deliveryTicket.CountySalesTaxRateName)

                        Else
                            token.ThrowIfCancellationRequested()
                            Dim countyQbId As String = QbCountySalesTaxRateManager.FindCountySalesTaxRateInQb(deliveryTicket.CountySalesTaxRateName)
                            If countyQbId IsNot Nothing Then
                                deliveryTicket.CountySalesTaxRateQuickbooksID = countyQbId
                                context.WorkerFormModel.QbCountyCache.Add(deliveryTicket.CountySalesTaxRateName, countyQbId)

                                token.ThrowIfCancellationRequested()
                                ApiConnector.PostToQbApi("PostSingleCountySalesTaxRateQuickbooksID", New With {.QbID = countyQbId, .CountyName = deliveryTicket.CountySalesTaxRateName}, reporter, token)
                            End If
                        End If
                    End If
                End If
            End If

            results = QbDeliveryTicketValidator.ValidateTicket(deliveryTicket)
            context.DeliveryTicketValidationResult.DeliveryTicketID = deliveryTicket.DeliveryTicketID
            context.DeliveryTicketValidationResult.Results = results

            Return results
        End Function

        Private Shared Sub GatherRunningInvoiceInformation(ByVal deliveryTicket As DeliveryTicketModel, ByVal reporter As UIProgressReporter, ByVal token As CancellationToken)
            token.ThrowIfCancellationRequested()

            If deliveryTicket.CustomerUsesRunningInvoices Then
                Dim runningInvoiceModel As InvoiceUIModel = _runningInvoices.SingleOrDefault(Function(x) x.CustomerQuickbooksID = deliveryTicket.CustomerQuickbooksID)
                If runningInvoiceModel IsNot Nothing Then
                    deliveryTicket.RunningInvoiceNumber = runningInvoiceModel.QbInvoiceNumber
                Else
                    Using useRunningInvoiceDialog As New RunningInvoicePromptDialog
                        useRunningInvoiceDialog.PromptUserFor(deliveryTicket, reporter, token)
                        useRunningInvoiceDialog.ShowDialog()
                        Dim runningInvoiceSearchResult = useRunningInvoiceDialog.RunningInvoiceSearchResult

                        If runningInvoiceSearchResult.Status = RunningInvoiceSearchStatus.Found Then
                            If Not _runningInvoices.Any(Function(x) x.QbInvoiceNumber = runningInvoiceSearchResult.FoundInvoiceUIModel.QbInvoiceNumber) Then
                                _runningInvoices.Add(runningInvoiceSearchResult.FoundInvoiceUIModel)
                            End If
                            deliveryTicket.RunningInvoiceNumber = runningInvoiceSearchResult.FoundInvoiceUIModel.QbInvoiceNumber
                        Else
                            deliveryTicket.RunningInvoiceNumber = ""
                        End If
                    End Using
                End If
            End If
        End Sub

        Private Shared Sub ValidateSingleLineItem(ByVal context As ValidationContext, ByVal lineItem As LineItemModel, ByVal deliveryTicketID As Integer, ByVal reporter As UIProgressReporter, ByVal token As CancellationToken)
            Dim lineItemValidationResults As Dictionary(Of LineItemValidationFailureReason, String) = QbDeliveryTicketValidator.ValidateLineItem(lineItem)
            If lineItemValidationResults.Count > 0 Then
                If lineItemValidationResults.ContainsKey(LineItemValidationFailureReason.MissingQbPartID) Then
                    If context.WorkerFormModel.QbPartCache.ContainsKey(lineItem.PartTemplateID.Value) Then
                        lineItem.PartQuickbooksID = context.WorkerFormModel.QbPartCache(lineItem.PartTemplateID.Value)

                    Else
                        reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.LoadingPartList})
                        token.ThrowIfCancellationRequested()
                        Dim partQbId As String = GetUserChosenPartIDFromDialog(lineItem, deliveryTicketID)
                        If partQbId IsNot Nothing Then
                            lineItem.PartQuickbooksID = partQbId
                            context.WorkerFormModel.QbPartCache.Add(lineItem.PartTemplateID.Value, partQbId)

                            token.ThrowIfCancellationRequested()
                            ApiConnector.PostToQbApi("PostSinglePartQuickbooksID", New With {.QbID = partQbId, .PartTemplateID = lineItem.PartTemplateID}, reporter, token)
                        End If
                    End If
                End If
            End If

            lineItemValidationResults = QbDeliveryTicketValidator.ValidateLineItem(lineItem)
            If lineItemValidationResults.Count > 0 Then
                context.LineItemValidationResults.Add(
                    New LineItemValidationResult With {
                        .LineItemID = lineItem.LineItemID,
                        .DeliveryTicketID = deliveryTicketID,
                        .Results = lineItemValidationResults
                    }
                )
            End If
        End Sub

        Public Shared Function GetUserChosenPartIDFromDialog(lineItem As LineItemModel, deliveryTicketID As Integer) As String
            Using choosePartIDDialog As New ChoosePartForm
                choosePartIDDialog.SetValues(lineItem.PartTemplateNumber, deliveryTicketID)
                Dim dialogResult = choosePartIDDialog.ShowDialog()
                If dialogResult = DialogResult.OK Then
                    Return choosePartIDDialog.ChosenQbPartID

                Else
                    Return Nothing
                End If
            End Using
        End Function

        Private Shared Sub AddToSummary(summaryBuilder As StringBuilder, processingContext As DeliveryTicketProcessingContext)
            summaryBuilder.Append("Delivery Ticket ").AppendLine(processingContext.DeliveryTicket.DeliveryTicketID)

            If processingContext.ValidationContext.DeliveryTicketValidationResult.IsValid Then
                If processingContext.QbSyncContext.AddedToQuickbooks Then
                    If String.IsNullOrEmpty(processingContext.DeliveryTicket.RunningInvoiceNumber) Then
                        summaryBuilder.Append("Ticket was successfully added to Quickbooks.") _
                                      .Append("Invoice Number: ") _
                                      .AppendLine(processingContext.DeliveryTicket.QuickbooksInvoiceNumber)
                    Else
                        summaryBuilder.Append("Ticket was successfully added to Running Invoice Number: ") _
                                      .AppendLine(processingContext.DeliveryTicket.QuickbooksInvoiceNumber)
                    End If

                    If processingContext.SendingQbSyncInfoToPtpFailed Then
                        summaryBuilder.AppendLine("IMPORTANT -- Could not automatically save the quickbooks invoice number to the online delivery ticket.")
                        summaryBuilder.AppendLine("You MUST manually add the invoice number (" & processingContext.DeliveryTicket.QuickbooksInvoiceNumber & ") to delivery ticket #" & processingContext.DeliveryTicket.DeliveryTicketID)
                    End If
                Else
                    summaryBuilder.AppendLine("Ticket was not added to Quickbooks.")
                End If

            Else
                summaryBuilder.Append("Ticket was not valid. Reason: ")

                For Each result As KeyValuePair(Of DeliveryTicketValidationFailureReason, String) In processingContext.ValidationContext.DeliveryTicketValidationResult.Results
                    summaryBuilder.AppendLine(result.Value)
                Next
            End If

            For Each result As LineItemValidationResult In processingContext.ValidationContext.LineItemValidationResults
                Dim partTemplateNumber = processingContext.DeliveryTicket.LineItems.First(Function(x) x.LineItemID = result.LineItemID).PartTemplateNumber
                summaryBuilder.AppendFormat("{0}LineItem with part number {1} was not valid. Reason: ", vbTab, partTemplateNumber)

                For Each innerResult As KeyValuePair(Of LineItemValidationFailureReason, String) In result.Results
                    summaryBuilder.AppendLine(innerResult.Value)
                Next
            Next

            If processingContext.QbSyncContext.Messages.Any() Then
                summaryBuilder.AppendLine()
                summaryBuilder.AppendLine("There were additional messages or errors for this ticket: ")
                summaryBuilder.AppendLine()
                For Each syncMessage As String In processingContext.QbSyncContext.Messages
                    summaryBuilder.AppendLine(syncMessage)
                Next
            End If

            summaryBuilder.AppendLine()
        End Sub

        Private Shared Sub DisplaySummary(ByVal summaryBuilder As StringBuilder)
            Dim summary As String = summaryBuilder.ToString

            Using summaryForm As New SyncSummaryForm
                summaryForm.SetText(summary)
                summaryForm.ShowDialog()
            End Using
        End Sub
    End Class
End Namespace
