Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class DeliveryTicketViewModel
        Public Property DeliveryTicketID As Integer

        Public Property CustomerID As Integer?
        Public Property LeaseID As Integer?
        Public Property WellID As Integer?

        Public Property IsSignificantDesignChange As Boolean

        Public Property PumpFailedID As Integer?
        Public Property PumpFailedNumber As String
        Public Property PumpFailedPrefix As String

        <DisplayFormat(DataFormatString:="{0:d}")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        Public Property PumpFailedDate As Date?

        Public Property PumpDispatchedID As Integer?
        Public Property PumpDispatchedNumber As String
        Public Property PumpDispatchedPrefix As String

        <DisplayFormat(DataFormatString:="{0:d}")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        Public Property PumpDispatchedDate As Date?
        Public Property PumpDispatchedTemplateID As Integer?
        Public Property PumpDispatchedConciseTemplate As String

        Public Property CustomerName As String
        Public Property LeaseLocation As String
        Public Property WellNumber As String

        Public Property RequiresPaymentUpFront As Boolean

        <UIHint("Date")>
        <DisplayFormat(DataFormatString:="{0:d}")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        Public Property TicketDate As Date? = Today
        Public Property ShipVia As String

        <UIHint("Date")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        <DisplayFormat(DataFormatString:="{0:d}")>
        Public Property OrderDate As Date?

        <UIHint("Time")>
        <DisplayFormat(DataFormatString:="{0:hh:mm tt}")>
        Public Property OrderTime As Date?

        Public Property OrderedBy As String
        Public Property PONumber As String

        <UIHint("Date")>
        <DisplayFormat(DataFormatString:="{0:d}")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        Public Property ShipDate As Date?

        <UIHint("Time")>
        <DisplayFormat(DataFormatString:="{0:hh:mm tt}")>
        Public Property ShipTime As Date?

        <UIHint("Date")>
        <DisplayFormat(DataFormatString:="{0:d}")>
        <Range(GetType(DateTime), "1/1/1900", "1/1/2200", ErrorMessage:="You cannot enter a date before 1900")>
        Public Property LastPull As Date?
        Public Property IsClosed As Boolean
        Public Property HoldDown As String
        Public Property Stroke As String
        Public Property ReasonStillOpen As String

        <Required()>
        Public Property CompletedBy As String
        Public Property RepairedBy As String
        Public Property Notes As String

        Public Property DisplaySignatureName As String
        Public Property DisplaySignatureDate As Date?

        <Required()>
        <DisplayFormat(DataFormatString:="{0:p3}")>
        Public Property SalesTaxRate As Decimal?
        Public Property CountySalesTaxRateID As Integer?
        Public Property CountySalesTaxRateName As String

        Public Property Quote As Boolean
        Public Property QuickbooksInvoiceNumber As String
        Public Property InvoiceStatus As Integer
        Public Property InvoiceStatusText As String

        Public Property InvBarrel As String
        Public Property InvSVCages As String
        Public Property InvDVCages As String
        Public Property InvSVSeats As String
        Public Property InvDVSeats As String
        Public Property InvSVBalls As String
        Public Property InvDVBalls As String
        Public Property InvHoldDown As String
        Public Property InvValveRod As String
        Public Property InvPlunger As String
        Public Property InvPTVCages As String
        Public Property InvPDVCages As String
        Public Property InvPTVSeats As String
        Public Property InvPDVSeats As String
        Public Property InvPTVBalls As String
        Public Property InvPDVBalls As String
        Public Property InvRodGuide As String
        Public Property InvTypeBallandSeat As String
    End Class
End Namespace