Namespace BL.Runtimes
    Friend Class DeliveryTicketEventModel
        Implements IEventModel

        Public Property DeliveryTicketID As Integer

        Public Property OldPumpDispatchedID As Integer?
        Public Property OldPumpDispatchedDate As Date?

        Public Property NewPumpDispatchedID As Integer?
        Public Property NewPumpDispatchedDate As Date?

        Public Property OldPumpFailedID As Integer?
        Public Property OldPumpFailedDate As Date?

        Public Property NewPumpFailedID As Integer?
        Public Property NewPumpFailedDate As Date?
    End Class
End Namespace