Namespace Models
    Public Class PartInspectionDashboardGridRowModel
        Public Property PartInspectionID As Integer
        Public Property DeliveryTicketID As Integer

        Public Property CustomerName As String
        Public Property ApiNumber As String
        Public Property LeaseName As String
        Public Property WellNumber As String
        Public Property PartDescription As String
        Public Property ReasonRepaired As String
        Public Property TicketDate As Date
        Public Property Cost As Decimal
        Public Property IsSignificantDesignChange As Boolean
    End Class
End Namespace