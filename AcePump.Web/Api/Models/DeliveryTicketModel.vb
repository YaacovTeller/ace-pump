Namespace Api.Models
    Public Class DeliveryTicketModel
        Public Property DeliveryTicketID As Integer

        Public Property CustomerUsesRunningInvoices As Boolean
        Public Property CustomerID As Integer?
        Public Property CustomerQuickbooksID As String
        Public Property CustomerName As String

        Public Property CountySalesTaxRateName As String
        Public Property CountySalesTaxRateQuickbooksID As String
        Public Property SalesTaxRate As Decimal

        Public Property InvoiceClassFullName As String

        Public Overridable Property LineItems As IEnumerable(Of LineItemModel)

        Public Property PumpFailedDate As Date?
        Public Property PumpFailedNumber As String
        Public Property PumpDispatchedNumber As String

        Public Property TicketDate As Date?
        Public Property PONumber As String
        Public Property ShipDate As Date?

        Public Property LeaseAndWell As String
        Public Property HoldDown As String
        Public Property TypeAndSizeOfPump As String
        Public Property InvBarrel As String
        Public Property InvPlunger As String
        Public Property InvSVSeats As String
        Public Property InvSVBalls As String
        Public Property InvHoldDown As String
        Public Property InvValveRod As String
        Public Property InvTVCages As String
        Public Property InvTVSeats As String
        Public Property InvTVBalls As String

        Public Property InvTypeBallAndSeat As String
        Public Property InvSVCages As String
        Public Property InvRodGuide As String
        Public Property OrderedBy As String
        Public Property Notes As String
        Public Property LastPull() As String
        Public ReadOnly Property LastPullAsFormattedDate() As String
            Get
                Dim buffer As Date
                If (Not String.IsNullOrWhiteSpace(LastPull)) And (Date.TryParse(LastPull, buffer)) Then
                    Return buffer
                Else
                    Return ""
                End If
            End Get
        End Property
    End Class
End Namespace