Namespace UI.Validation
    Public Class LineItemValidationResult
        Public Property LineItemID As Integer
        Public Property DeliveryTicketID As Integer

        Public Property Results As New Dictionary(Of LineItemValidationFailureReason, String)

        Public ReadOnly Property IsValid As Boolean
            Get
                Return Results.Count() = 0
            End Get
        End Property
    End Class
End Namespace