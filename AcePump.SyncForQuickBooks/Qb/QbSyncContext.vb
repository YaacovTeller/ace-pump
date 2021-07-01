Imports AcePump.SyncForQuickBooks.PtpApi.Models
Imports AcePump.QbApi.Models

Namespace Qb
    Public Class QbSyncContext
        Public Property DeliveryTicket As DeliveryTicketModel
        Public Property Invoice As QbInvoice

        Public Property AddedToQuickbooks As Boolean
        Public Property Messages As List(Of String)

        Public Sub New(deliveryTicket As DeliveryTicketModel)
            Me.DeliveryTicket = deliveryTicket

            Messages = New List(Of String)
        End Sub

    End Class
End Namespace