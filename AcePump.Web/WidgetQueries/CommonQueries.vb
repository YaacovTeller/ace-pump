Imports System.Data.Entity.SqlServer
Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports AcePump.Domain.Models
Imports System.Linq.Expressions

Namespace WidgetQueries
    Public Class CommonQueries
        Private Property DataSource As AcePumpContext

        Public Property PartRuntimeDaysInGroundExpression As Expression(Of Func(Of PartRuntime, Integer)) =
            Function(x) If(x.TotalDaysInGround.HasValue,
                           x.TotalDaysInGround.Value,
                           x.Segments.Select(Function(y) If(y.LengthInDays.HasValue,
                                                         y.LengthInDays.Value,
                                                         If(y.Finish.HasValue And y.Start.HasValue,
                                                            SqlFunctions.DateDiff("d", y.Start, y.Finish),
                                                            If(y.Start.HasValue,
                                                               SqlFunctions.DateDiff("d", y.Start, Now),
                                                               0)
                                                            )
                                                        )
                                          ) _
                                      .DefaultIfEmpty(0) _
                                      .Sum()
                          )


        Public Sub New(dataSource As AcePumpContext)
            Me.DataSource = dataSource
        End Sub

        Public Function CalculateAverage(partRuntimes As IQueryable(Of PartRuntime)) As Double
            Dim average As Double = partRuntimes.Select(PartRuntimeDaysInGroundExpression).DefaultIfEmpty().Average()

            Return average
        End Function

        Public Function PumpRuntimes(query As AcePumpQueryModel) As IQueryable(Of PumpRuntime)
            Dim result = From runtime In DataSource.PumpRuntimes
                         Where Not runtime.Start.HasValue OrElse runtime.Start >= query.StartDate
                         Where Not runtime.Finish.HasValue OrElse runtime.Finish <= query.EndDate
                         Where runtime.RuntimeStartedByTicket IsNot Nothing OrElse runtime.RuntimeStartedByTicket.Well IsNot Nothing _
                         OrElse runtime.RuntimeStartedByTicket.Well.Lease IsNot Nothing _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.HasValue _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.Value

            If query.WellID.HasValue Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.WellID.HasValue AndAlso x.RuntimeStartedByTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.Well IsNot Nothing AndAlso x.RuntimeStartedByTicket.Well.LeaseID = query.LeaseID.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.RuntimeStartedByTicket.CustomerID.Value))
            End If

            Return result
        End Function

        Public Function PartRuntimesByRuntime(query As AcePumpQueryModel) As IQueryable(Of PartRuntime)
            Dim result = From runtime In DataSource.PartRuntimes
                         Where Not runtime.Start.HasValue OrElse runtime.Start >= query.StartDate
                         Where Not runtime.Finish.HasValue OrElse runtime.Finish <= query.EndDate
                         Where runtime.RuntimeStartedByTicket IsNot Nothing OrElse runtime.RuntimeStartedByTicket.Well IsNot Nothing _
                         OrElse runtime.RuntimeStartedByTicket.Well.Lease IsNot Nothing _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.HasValue _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.Value

            If query.WellID.HasValue Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.WellID.HasValue AndAlso x.RuntimeStartedByTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.Well IsNot Nothing AndAlso x.RuntimeStartedByTicket.Well.LeaseID = query.LeaseID.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                result = result.Where(Function(x) x.RuntimeStartedByTicket IsNot Nothing AndAlso x.RuntimeStartedByTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.RuntimeStartedByTicket.CustomerID.Value))
            End If

            Return result
        End Function

        Public Function PartRuntimesBySegment(query As AcePumpQueryModel) As IQueryable(Of PartRuntimeBySegment)
            Dim result = From runtime In DataSource.PartRuntimes
                         Where Not runtime.Start.HasValue OrElse runtime.Start >= query.StartDate
                         Where Not runtime.Finish.HasValue OrElse runtime.Finish <= query.EndDate
                         Where runtime.RuntimeStartedByTicket IsNot Nothing OrElse runtime.RuntimeStartedByTicket.Well IsNot Nothing _
                         OrElse runtime.RuntimeStartedByTicket.Well.Lease IsNot Nothing _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.HasValue _
                         OrElse Not runtime.RuntimeStartedByTicket.Well.Lease.IgnoreInReporting.Value
                         Select New PartRuntimeBySegment With {
                             .Runtime = runtime,
                             .MostRecentSegment = runtime.Segments.OrderByDescending(Function(x) x.Start).Where(Function(x) Not x.Finish.HasValue).FirstOrDefault()
                         }

            If query.WellID.HasValue Then
                result = result.Where(Function(x) x.MostRecentSegment IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket.WellID.HasValue AndAlso x.MostRecentSegment.SegmentStartedByTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                result = result.Where(Function(x) x.MostRecentSegment IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket.Well IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket.Well.LeaseID = query.LeaseID.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                result = result.Where(Function(x) x.MostRecentSegment IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket IsNot Nothing AndAlso x.MostRecentSegment.SegmentStartedByTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.MostRecentSegment.SegmentStartedByTicket.CustomerID.Value))
            End If

            Return result
        End Function

        Public Function Tickets(query As AcePumpQueryModel) As IQueryable(Of DeliveryTicket)
            Dim result = From ticket In DataSource.DeliveryTickets
                         Where ticket.TicketDate >= query.StartDate And ticket.TicketDate <= query.EndDate
                         Where ticket.Well IsNot Nothing OrElse ticket.Well.Lease IsNot Nothing _
                         OrElse Not ticket.Well.Lease.IgnoreInReporting.HasValue _
                         OrElse Not ticket.Well.Lease.IgnoreInReporting.Value

            If query.WellID.HasValue Then
                result = result.Where(Function(x) x.WellID.HasValue AndAlso x.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                result = result.Where(Function(x) x.Well.LeaseID = query.LeaseID.Value And Not x.Well.Lease.IgnoreInReporting.HasValue OrElse Not x.Well.Lease.IgnoreInReporting.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                result = result.Where(Function(x) x.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.CustomerID.Value))
            End If

            Return result
        End Function

        Public Function TicketLineItemCost(query As AcePumpQueryModel) As IQueryable(Of TicketLineItemCost)
            Dim lines = From line In DataSource.LineItems
                        Where line.DeliveryTicket.TicketDate <= query.EndDate And line.DeliveryTicket.TicketDate >= query.StartDate
                        Where line.DeliveryTicket.Well IsNot Nothing OrElse line.DeliveryTicket.Well.Lease IsNot Nothing _
                        OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue _
                        OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.Value
                        Select New With {
                            .LineItem = line,
                            .SalexTaxRate = If(line.CollectSalesTax.HasValue AndAlso line.CollectSalesTax.Value,
                                               line.DeliveryTicket.SalesTaxRate + 1D,
                                               1D),
                            .Quantity = line.Quantity,
                            .PriceEach = line.UnitPrice * (1 - line.UnitDiscount)
                        }

            If query.WellID.HasValue Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.WellID.HasValue AndAlso x.LineItem.DeliveryTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.Well.LeaseID = query.LeaseID.Value And Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue OrElse Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.LineItem.DeliveryTicket.CustomerID.Value))
            End If

            If Not String.IsNullOrEmpty(query.ReasonRepaired) Then
                lines = lines.Where(Function(x) x.LineItem.PartInspection.ReasonRepaired = query.ReasonRepaired)
            End If

            Dim cost As IQueryable(Of TicketLineItemCost) = From line In lines
                                                            Select New TicketLineItemCost() With {
                                                                  .LineItem = line.LineItem,
                                                                  .Cost = line.PriceEach * line.Quantity * line.SalexTaxRate
                                                            }

            Return cost
        End Function

        Public Function TicketInspectionCost(query As AcePumpQueryModel) As IQueryable(Of TicketLineItemCost)
            Dim lines = From line In DataSource.LineItems
                        Where line.DeliveryTicket.TicketDate <= query.EndDate And line.DeliveryTicket.TicketDate >= query.StartDate
                        Where line.PartInspectionID.HasValue
                        Where line.DeliveryTicket.Well IsNot Nothing OrElse line.DeliveryTicket.Well.Lease IsNot Nothing _
                        OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue _
                        OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.Value
                        Select New With {
                            .LineItem = line,
                            .SalexTaxRate = If(line.CollectSalesTax.HasValue AndAlso line.CollectSalesTax.Value,
                                               line.DeliveryTicket.SalesTaxRate + 1D,
                                               1D),
                            .Quantity = line.Quantity,
                            .PriceEach = line.UnitPrice * (1 - line.UnitDiscount)
                        }

            If query.WellID.HasValue Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.WellID.HasValue AndAlso x.LineItem.DeliveryTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.Well.LeaseID = query.LeaseID.Value And Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue OrElse Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.LineItem.DeliveryTicket.CustomerID.Value))
            End If

            If Not String.IsNullOrEmpty(query.ReasonRepaired) Then
                lines = lines.Where(Function(x) x.LineItem.PartInspection.ReasonRepaired = query.ReasonRepaired)
            End If

            Dim cost As IQueryable(Of TicketLineItemCost) = From line In lines
                                                            Select New TicketLineItemCost() With {
                                                                  .LineItem = line.LineItem,
                                                                  .Cost = line.PriceEach * line.Quantity * line.SalexTaxRate
                                                            }

            Return cost
        End Function

        Public Function FailedInspections(query As AcePumpQueryModel) As IQueryable(Of PartInspection)
            Return AllInspections(query).Where(Function(inspection) inspection.Result = "Replace" Or inspection.Result = "Convert")
        End Function

        Public Function AllInspections(query As AcePumpQueryModel) As IQueryable(Of PartInspection)
            Dim result = From inspection In DataSource.PartInspections
                         Where inspection.DeliveryTicket.TicketDate >= query.StartDate And inspection.DeliveryTicket.TicketDate <= query.EndDate
                         Where inspection.DeliveryTicket.Well IsNot Nothing OrElse inspection.DeliveryTicket.Well.Lease IsNot Nothing _
                         OrElse Not inspection.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue _
                         OrElse Not inspection.DeliveryTicket.Well.Lease.IgnoreInReporting.Value

            If query.WellID.HasValue Then
                result = result.Where(Function(x) x.DeliveryTicket.WellID.HasValue AndAlso x.DeliveryTicket.WellID.Value = query.WellID.Value)
            End If

            If query.LeaseID.HasValue Then
                result = result.Where(Function(x) x.DeliveryTicket.Well.LeaseID = query.LeaseID.Value And Not x.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue OrElse Not x.DeliveryTicket.Well.Lease.IgnoreInReporting.Value)
            End If

            If query.CustomerAccessIDs.Any() Then
                result = result.Where(Function(x) x.DeliveryTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.DeliveryTicket.CustomerID.Value))
            End If

            If Not String.IsNullOrEmpty(query.ReasonRepaired) Then
                result = result.Where(Function(x) x.ReasonRepaired = query.ReasonRepaired)
            End If

            Return result
        End Function
    End Class
End Namespace