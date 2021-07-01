Namespace WebBackgroundTask
    Public Class WebBackgroundTask
        Public Property Id As Guid
        Public Property TotalOperations As Integer
        Public Property OperationsComplete As Integer

        Public ReadOnly Property PercentComplete As Double
            Get
                Return If(TotalOperations = 0,
                          0.0,
                          OperationsComplete / TotalOperations)
            End Get
        End Property

        Public Sub New()
            Id = Guid.NewGuid()
        End Sub
    End Class
End Namespace