Imports AcePump.SyncForQuickBooks.UI.Models

Namespace UI.Validation
    Public Class ValidationContext
        Public Property WorkerFormModel As WorkerFormModel

        Public Property DeliveryTicketValidationResult As New DeliveryTicketValidationResult
        Public Property LineItemValidationResults As New List(Of LineItemValidationResult)

        Public Sub New(workerFormModel As WorkerFormModel)
            Me.WorkerFormModel = workerFormModel
        End Sub
    End Class
End Namespace