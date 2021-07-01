Imports System.ComponentModel

Namespace Areas.Employees.Models.DisplayDtos
    Public Class RepairTicketModel
        Public Property DeliveryTicketID As Integer
        Public Property CustomerID As Integer
        Public Property Notes As String
        Public Property CurrentTicketDate As Date?

        <DisplayName("Completed")>
        Public Property IsRepairComplete As Boolean

        <DisplayName("Pump Repaired #")>
        Public Property PumpFailedNumber As String
        Public Property PumpFailedPrefix As String

        Public Property PumpFailedTemplateID As Integer
        Public Property PumpFailedTemplatSpecSummary As String

        <DisplayName("Pump Out")>
        Public Property PumpDispatchedNumber As String
        Public Property PumpDispatchedPrefix As String

        Public Property PlungerBarrelWear As String
        Public Property PumpFailedID As Integer?
        Public Property PlungerOrig As String
        Public Property BarrelOrig As String
        Public Property CustomerUsesInventory As Boolean
        Public ReadOnly Property CustomerUsesInventoryString As String
            Get
                Return If(CustomerUsesInventory, "true", "false")
            End Get
        End Property
    End Class
End Namespace