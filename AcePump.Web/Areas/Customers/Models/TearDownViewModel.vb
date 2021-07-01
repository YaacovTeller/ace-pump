Namespace Areas.Customers.Models
    Public Class TearDownViewModel
        Public Property DeliveryTicketID As Integer
        Public Property TicketDate As Date?
        Public Property PumpRepairedNumber As String
        Public Property PumpRepairedTemplateVerbose As String
        Public Property Notes As String
        Public Property IsRepairComplete As Boolean
        Public Property RepairMode As AcePump.Common.AcePumpRepairModes
    End Class
End Namespace