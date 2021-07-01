Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource

Namespace BL.Runtimes
    <HandlesEventModel(GetType(DeliveryTicketEventModel))> _
    Friend Class DeliveryTicketEventHandler
        Implements IRuntimeEventHandler

        Private Property Model As DeliveryTicketEventModel
        Private Property DataSource As AcePumpContext
        Private Property QueryHelper As RuntimeQueryHelper

        Public Sub New(model As DeliveryTicketEventModel)
            Me.Model = Model
        End Sub

        Public Sub UpdateDataSource(dataSource As AcePumpContext) Implements IRuntimeEventHandler.UpdateDataSource
            Me.DataSource = dataSource
            Me.QueryHelper = New RuntimeQueryHelper(dataSource)

            Dim pumpDispatchedChanged As Boolean = (Not Model.OldPumpDispatchedID.Equals(Model.NewPumpDispatchedID)) _
                                                   OrElse (Model.OldPumpDispatchedID.HasValue And Not Model.OldPumpDispatchedDate.Equals(Model.NewPumpDispatchedDate))
            If pumpDispatchedChanged Then
                If Model.OldPumpDispatchedID.HasValue Then
                    RemoveOldDispatchedEvent()
                End If

                If Model.NewPumpDispatchedID.HasValue Then
                    StoreNewDispatchedEvent()
                End If
            End If

            Dim pumpFailedChanged As Boolean = (Not Model.OldPumpFailedID.Equals(Model.NewPumpFailedID)) _
                                               OrElse (Model.OldPumpFailedID.HasValue And Not Model.OldPumpFailedDate.Equals(Model.NewPumpFailedDate))
            If pumpFailedChanged Then
                If Model.OldPumpFailedID.HasValue Then
                    RemoveOldFailedEvent()
                End If

                If Model.NewPumpDispatchedID.HasValue Then
                    StoreNewFailedEvent()
                End If
            End If

            Me.DataSource = Nothing
            Me.QueryHelper = Nothing
        End Sub

        Private Sub RemoveOldDispatchedEvent()
            Dim pumpRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.OldPumpDispatchedID.Value, Model.OldPumpDispatchedDate.Value)
            If pumpRuntime.Exists() Then
                pumpRuntime.RemoveStartDate()
            End If

            Dim partRuntimeSegmentQueries As IQueryable(Of ExistingRuntimeQuery) = QueryHelper.GetPartRuntimes(Model.OldPumpDispatchedID.Value, Model.OldPumpDispatchedDate.Value)
            For Each query As ExistingRuntimeQuery In partRuntimeSegmentQueries
                If query.RuntimeIfExists IsNot Nothing Then
                    query.RuntimeIfExists.Manage(DataSource).RemoveStartDate()
                End If

                If query.SegmentIfExists IsNot Nothing OrElse TryFindSegment(query, Model.OldPumpDispatchedDate.Value) Then
                    query.SegmentIfExists.Manage(DataSource).RemoveStartDate()
                End If
            Next
        End Sub

        Private Sub RemoveOldFailedEvent()
            Dim pumpRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.OldPumpFailedID.Value, Model.OldPumpFailedDate.Value)
            If pumpRuntime.Exists() Then
                pumpRuntime.RemoveEndDate()
            End If

            Dim partRuntimeSegmentQueries As IQueryable(Of ExistingRuntimeQuery) = QueryHelper.GetPartRuntimes(Model.OldPumpFailedID.Value, Model.OldPumpFailedDate.Value)
            For Each query As ExistingRuntimeQuery In partRuntimeSegmentQueries
                If query.SegmentIfExists IsNot Nothing OrElse TryFindSegment(query, Model.OldPumpFailedDate.Value) Then
                    query.SegmentIfExists.Manage(DataSource).RemoveEndDate()
                End If
            Next
        End Sub

        Private Sub StoreNewDispatchedEvent()
            Dim pumpRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.NewPumpDispatchedID.Value, Model.NewPumpDispatchedDate.Value)
            pumpRuntime.SetStartDate(Model.NewPumpDispatchedDate.Value, Model.DeliveryTicketID)

            Dim partRuntimeSegmentQueries As IQueryable(Of ExistingRuntimeQuery) = QueryHelper.GetPartRuntimes(Model.NewPumpDispatchedID.Value, Model.NewPumpDispatchedDate.Value)
            For Each query As ExistingRuntimeQuery In partRuntimeSegmentQueries
                If query.RuntimeIfExists Is Nothing Then
                    CreatePartRuntime(query)
                    CreatePartRuntimeSegment(query)

                ElseIf query.SegmentIfExists Is Nothing AndAlso Not TryFindSegment(query, Model.NewPumpDispatchedDate.Value) Then
                    CreatePartRuntimeSegment(query)
                End If

                query.RuntimeIfExists.Manage(DataSource).SetStartDate(Model.NewPumpDispatchedDate.Value, Model.DeliveryTicketID)
                query.SegmentIfExists.Manage(DataSource).SetStartDate(Model.NewPumpDispatchedDate.Value, Model.DeliveryTicketID)
            Next
        End Sub

        Private Sub StoreNewFailedEvent()
            Dim pumpRuntime As IRuntimeManager = QueryHelper.ManageRuntimeEventOccuredIn(Model.NewPumpFailedID.Value, Model.NewPumpFailedDate.Value)
            pumpRuntime.SetEndDate(Model.NewPumpFailedDate.Value, Model.DeliveryTicketID)

            Dim partRuntimeSegmentQueries As IQueryable(Of ExistingRuntimeQuery) = QueryHelper.GetPartRuntimes(Model.NewPumpFailedID.Value, Model.NewPumpFailedDate.Value)
            For Each query As ExistingRuntimeQuery In partRuntimeSegmentQueries
                If query.RuntimeIfExists Is Nothing Then
                    CreatePartRuntime(query)
                    CreatePartRuntimeSegment(query)

                ElseIf query.SegmentIfExists Is Nothing AndAlso Not TryFindSegment(query, Model.NewPumpDispatchedDate.Value) Then
                    CreatePartRuntimeSegment(query)
                End If

                query.SegmentIfExists.Manage(DataSource).SetEndDate(Model.NewPumpFailedDate.Value, Model.DeliveryTicketID)
            Next
        End Sub

        Private Function TryFindSegment(query As ExistingRuntimeQuery, eventDate As Date) As Boolean
            If query.RuntimeSegmentsIfExist Is Nothing Then Return False

            Dim matchingSegment As PartRuntimeSegment = (From segment In query.RuntimeSegmentsIfExist
                                                         Where Not segment.Start.HasValue OrElse segment.Start.Value <= eventDate
                                                         Where Not segment.Finish.HasValue OrElse segment.Finish.Value >= eventDate) _
                                                        .SingleOrDefault()

            If matchingSegment IsNot Nothing Then
                query.SegmentIfExists = matchingSegment
                Return True

            Else
                Return False
            End If
        End Function

        Private Sub CreatePartRuntime(query As ExistingRuntimeQuery)
            query.RuntimeIfExists = New PartRuntime() With {
                .TemplatePartDefID = query.TemplatePartDefID,
                .PumpID = query.PumpID
            }

            DataSource.PartRuntimes.Add(query.RuntimeIfExists)
        End Sub

        Private Sub CreatePartRuntimeSegment(query As ExistingRuntimeQuery)
            query.SegmentIfExists = New PartRuntimeSegment() With {
                .Runtime = query.RuntimeIfExists
            }

            DataSource.PartRuntimeSegments.Add(query.SegmentIfExists)
        End Sub
    End Class
End Namespace