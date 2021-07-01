Imports System.ComponentModel

Namespace Areas.Employees.Models.DisplayDtos
    Public Class TearDownViewModel
        Public Property DeliveryTicketID As Integer

        <DisplayName("Pump Repaired #")>
        Public Property PumpFailedNumber As String
        Public Property PumpFailedPrefix As String

        Public Property PumpFailedID As Integer?
        Public Property PumpFailedTemplateID As Integer
        Public Property PumpFailedTemplatSpecSummary As String

        <DisplayName("Pump Out")>
        Public Property PumpDispatchedNumber As String
        Public Property PumpDispatchedPrefix As String

        Public Property RepairComplete As Boolean
    End Class
End Namespace