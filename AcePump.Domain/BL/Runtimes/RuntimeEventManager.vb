Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models

Namespace BL.Runtimes
    Public Class RuntimeEventManager
        Private Property DataSource As AcePumpContext

        Public Sub New(dataSource As AcePumpContext)
            Me.DataSource = dataSource
        End Sub

        Private Sub ProcessRuntimeEvent(Of TEventModel)([event] As TEventModel)
            For Each handler As IRuntimeEventHandler In RuntimeEventHandlerCache.Instance.GetEventHandlers(Of TEventModel)([event])
                handler.UpdateDataSource(DataSource)
            Next
        End Sub

        Public Sub ProcessDeliveryTicketCreated(newTicket As DeliveryTicket)
            Dim model As New DeliveryTicketEventModel With {
                .DeliveryTicketID = newTicket.DeliveryTicketID,
                .NewPumpDispatchedID = newTicket.PumpDispatchedID,
                .NewPumpDispatchedDate = SelectAppropriateDate(newTicket.PumpDispatchedID, newTicket.PumpDispatchedDate, newTicket.TicketDate),
                .NewPumpFailedID = newTicket.PumpFailedID,
                .NewPumpFailedDate = SelectAppropriateDate(newTicket.PumpFailedID, newTicket.PumpFailedDate, newTicket.TicketDate)
            }

            ProcessRuntimeEvent(model)
        End Sub

        Public Sub ProcessDeliveryTicketUpdated(originalTicket As DeliveryTicket, newPumpDispatchedDate As Date?, newPumpDispatchedID As Integer?, newPumpFailedDate As Date?, newPumpFailedID As Integer?)
            Dim model As New DeliveryTicketEventModel With {
                .DeliveryTicketID = originalTicket.DeliveryTicketID,
                .NewPumpDispatchedID = newPumpDispatchedID,
                .NewPumpDispatchedDate = SelectAppropriateDate(newPumpDispatchedID, newPumpDispatchedDate, originalTicket.TicketDate),
                .NewPumpFailedID = newPumpFailedID,
                .NewPumpFailedDate = SelectAppropriateDate(newPumpFailedID, newPumpFailedDate, originalTicket.TicketDate),
                .OldPumpDispatchedID = originalTicket.PumpDispatchedID,
                .OldPumpDispatchedDate = SelectAppropriateDate(originalTicket.PumpDispatchedID, originalTicket.PumpDispatchedDate, originalTicket.TicketDate),
                .OldPumpFailedID = originalTicket.PumpFailedID,
                .OldPumpFailedDate = SelectAppropriateDate(originalTicket.PumpFailedID, originalTicket.PumpFailedDate, originalTicket.TicketDate)
            }

            ProcessRuntimeEvent(model)
        End Sub

        Public Sub ProcessDeliveryTicketDeleted(deletedTicket As DeliveryTicket)
            Dim model As New DeliveryTicketEventModel With {
                .DeliveryTicketID = deletedTicket.DeliveryTicketID,
                .OldPumpDispatchedID = deletedTicket.PumpDispatchedID,
                .OldPumpDispatchedDate = SelectAppropriateDate(deletedTicket.PumpDispatchedID, deletedTicket.PumpDispatchedDate, deletedTicket.TicketDate),
                .OldPumpFailedID = deletedTicket.PumpFailedID,
                .OldPumpFailedDate = SelectAppropriateDate(deletedTicket.PumpFailedID, deletedTicket.PumpFailedDate, deletedTicket.TicketDate)
            }

            ProcessRuntimeEvent(model)
        End Sub

        Public Sub ProcessPartInspectionUpdated(originalInspection As PartInspection, newResult As String, newDate As Date?)
            Dim model As New PartInspectionEventModel With {
                .PartInspectionID = originalInspection.PartInspectionID,
                .OldDate = If(originalInspection.DeliveryTicket.PumpFailedDate.HasValue, originalInspection.DeliveryTicket.PumpFailedDate.Value, originalInspection.DeliveryTicket.TicketDate),
                .OldResult = originalInspection.Result,
                .NewDate = newDate,
                .NewResult = newResult,
                .PumpID = originalInspection.DeliveryTicket.PumpFailedID
            }

            ProcessRuntimeEvent(model)
        End Sub

        Public Sub ProcessPartInspectionCreated(newInspection As PartInspection)
            Dim model As New PartInspectionEventModel With {
                .PartInspectionID = newInspection.PartInspectionID,
                .NewDate = If(newInspection.DeliveryTicket.PumpFailedDate.HasValue, newInspection.DeliveryTicket.PumpFailedDate.Value, newInspection.DeliveryTicket.TicketDate),
                .NewResult = newInspection.Result,
                .PumpID = newInspection.DeliveryTicket.PumpFailedID
            }

            ProcessRuntimeEvent(model)
        End Sub

        Public Sub ProcessPartInspectionDeleted(deletedInspection As PartInspection)
            Dim model As New PartInspectionEventModel With {
                .PartInspectionID = deletedInspection.PartInspectionID,
                .OldDate = If(deletedInspection.DeliveryTicket.PumpFailedDate.HasValue, deletedInspection.DeliveryTicket.PumpFailedDate.Value, deletedInspection.DeliveryTicket.TicketDate),
                .OldResult = deletedInspection.Result,
                .PumpID = deletedInspection.DeliveryTicket.PumpFailedID
            }

            ProcessRuntimeEvent(model)
        End Sub

        Private Function SelectAppropriateDate(forId As Integer?, savedEventDate As Date?, ticketDate As Date?) As Date?
            If forId.HasValue Then
                Return If(savedEventDate.HasValue, savedEventDate.Value, ticketDate)
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace