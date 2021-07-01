Imports AcePump.SyncForQuickBooks.UI.Models
Imports AcePump.SyncForQuickBooks.UI.Validation
Imports AcePump.SyncForQuickBooks.PtpApi.Models
Imports AcePump.SyncForQuickBooks.Qb

Namespace UI
    Public Class DeliveryTicketProcessingContext
        Public Property WorkerFormModel As WorkerFormModel

        Public Property DeliveryTicket As DeliveryTicketModel
        Public Property ValidationContext As ValidationContext
        Public Property QbSyncContext As QbSyncContext

        Public Property SendingQbSyncInfoToPtpFailed As Boolean

        Public Sub New(uiModel As WorkerFormModel, deliveryTicket As DeliveryTicketModel)
            Me.WorkerFormModel = uiModel
            Me.DeliveryTicket = deliveryTicket

            Me.ValidationContext = New ValidationContext(uiModel)
            Me.QbSyncContext = New QbSyncContext(deliveryTicket)
        End Sub
    End Class
End Namespace