Namespace Areas.Customers.Models
    Public Class RepairTicketViewModel
        Public Property DeliveryTicketID As Integer
        Public Property TicketDate As Date?
        Public Property LeaseAndWell As String
        Public Property PumpFailedID As Integer?
        Public Property PumpRepairedNumber As String
        Public Property PumpOutNumber As String
        Public Property HoldDown As String
        Public Property PumpRepairedTemplateVerbose As String
        Public Property Notes As String
        Public Property PlungerBarrelWear As String
        Public Property IsRepairComplete As Boolean
        Public Property RepairMode As AcePump.Common.AcePumpRepairModes
        Public Property CustomerUsesInventory As Boolean
        Public ReadOnly Property CustomerUsesInventoryString As String
            Get
                Return If(CustomerUsesInventory, "true", "false")
            End Get
        End Property
    End Class
End Namespace