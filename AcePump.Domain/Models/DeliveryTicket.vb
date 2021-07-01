Imports System.ComponentModel.DataAnnotations
Imports AcePump.Common

Namespace Models
    Public Class DeliveryTicket
        Public Property DeliveryTicketID As Integer

        Public Property WellID As Integer?
        Public Overridable Property Well As Well

        Public Property CustomerID As Integer?
        Public Overridable Property Customer As Customer

        Public Overridable Property Inspections As ICollection(Of PartInspection)
        Public Overridable Property LineItems As ICollection(Of LineItem)

        Public Property PumpFailedDate As Date?
        Public Property PumpFailedID As Integer?
        Public Overridable Property PumpFailed As Pump

        Public Property PumpDispatchedDate As Date?
        Public Property PumpDispatchedID As Integer?
        Public Overridable Property PumpDispatched As Pump

        Public Property SalesTaxRate As Decimal

        Public Property CountySalesTaxRateID As Integer?
        Public Overridable Property CountySalesTaxRate As CountySalesTaxRate

        Public Property IsSignificantDesignChange As Boolean?

        Public Property CompletedBy As String
        Public Property RepairedBy As String

        Public Property ReasonStillOpen As String

        Public Property RepairMode As AcePumpRepairModes

        Public Property SignatureName As String
        Public Property SignatureDate As Date?
        Public Property SignatureCompanyName As String

        <MaxLength()>
        Public Property Signature As Byte()

        Public Property TicketDate As Date?
        Public Property CloseTicket As Boolean?
        Public Property ShipVia As String
        Public Property PONumber As String
        Public Property OrderDate As Date?
        Public Property OrderTime As DateTime?
        Public Property ShipDate As Date?
        Public Property ShipTime As DateTime?
        Public Property OrderedBy As String
        Public Property LastPull As String
        Public Property Stroke As String
        Public Property HoldDown As String
        Public Property Notes As String
        Public Property SortOrder As Integer?
        Public Property FilterAssembly As Integer?

        Public Property Quote As Boolean?

        Public Property QuickbooksID As String
        Public Property QuickbooksInvoiceNumber As String
        Public Property InvoiceStatus As Integer

        Public Property RepairComplete As Boolean

        Public Property PlungerBarrelWear As String
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