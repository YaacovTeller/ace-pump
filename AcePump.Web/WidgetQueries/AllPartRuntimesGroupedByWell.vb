Imports AcePump.Domain.DataSource
Imports AcePump.Web.WidgetQueries.Models
Imports System.Linq.Expressions
Imports AcePump.Domain.Models
Imports System.Reflection
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    <SupportedWidgetId("AllPartRuntimesGroupedByWell")> _
    Public Class AllPartRuntimesGroupedByWell
        Inherits AcePumpChartQueryBase(Of ChartDataPoint(Of String))

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected Overloads Overrides Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of ChartDataPoint(Of String)))
            Dim runtimes As IQueryable(Of PartRuntimeBySegment) = Common.PartRuntimesBySegment(query)

            Dim chartDataPoints As IQueryable(Of ChartDataPoint(Of String))
            If query.AdditionalParameters("runtimeType") = RuntimeType.ByLease Then
                chartDataPoints = From runtime In runtimes
                                  Group By runtime.MostRecentSegment.SegmentStartedByTicket.Well.Lease Into g = Group
                                  Select New ChartDataPoint(Of String) With {
                                      .Category = Lease.LocationName,
                                      .Value = 0D
                                  }

            ElseIf query.AdditionalParameters("runtimeType") = RuntimeType.ByWell Then
                chartDataPoints = From runtime In runtimes
                                  Group By runtime.MostRecentSegment.SegmentStartedByTicket.Well Into g = Group
                                  Select New ChartDataPoint(Of String) With {
                                      .Category = Well.WellNumber,
                                      .Value = 0D
                                  }

            Else
                Throw New InvalidOperationException("unknown runtime type: " & query.AdditionalParameters("runtimeType").ToString())
            End If

            ConvertExpressionToIncludeAverage(chartDataPoints)
            manager.AddDataSet(chartDataPoints)
        End Sub

        Private Sub ConvertExpressionToIncludeAverage(sequence As IQueryable(Of ChartDataPoint(Of String)))
            Dim v As New ValueToAverageVisitor(Common.PartRuntimeDaysInGroundExpression)
            v.Visit(sequence.Expression)
        End Sub

        Private Class ValueToAverageVisitor
            Inherits ExpressionVisitor

            Private Property InValueBinding As Boolean = False
            Private Property AverageLambda As Expression(Of Func(Of PartRuntime, Integer))

            Private Property PartRuntimeParameterExpression As ParameterExpression
            Private Property PartRuntimeSequence As ConstantExpression

            Public Sub New(averageLambda As Expression(Of Func(Of PartRuntime, Integer)))
                Me.AverageLambda = averageLambda
            End Sub

            Protected Overrides Function VisitParameter(node As ParameterExpression) As Expression
                If PartRuntimeParameterExpression Is Nothing AndAlso node.Type = GetType(PartRuntime) Then
                    PartRuntimeParameterExpression = node
                End If

                Return MyBase.VisitParameter(node)
            End Function

            Protected Overrides Function VisitMemberMemberBinding(node As MemberMemberBinding) As MemberMemberBinding
                If node.Member.Name = "Value" Then
                    InValueBinding = True
                End If

                Dim rVal As MemberMemberBinding = MyBase.VisitMemberMemberBinding(node)
                InValueBinding = False

                Return rVal
            End Function

            Protected Overrides Function VisitConstant(node As ConstantExpression) As Expression
                If PartRuntimeSequence Is Nothing AndAlso node.Type = GetType(IQueryable(Of PartRuntime)) Then
                    PartRuntimeSequence = node
                End If

                If InValueBinding Then
                    Return GetAverageExpression()

                Else
                    Return MyBase.VisitConstant(node)
                End If
            End Function

            Private _AverageExpression As MethodCallExpression
            Private Function GetAverageExpression() As MethodCallExpression
                If _AverageExpression Is Nothing Then
                    Dim queryableType As Type = GetType(Queryable)
                    Dim averageMethod As MethodInfo = queryableType.GetMethod("Average", {GetType(IQueryable(Of )), GetType(Expression(Of ))})

                    _AverageExpression = Expression.Call(Nothing, averageMethod, {PartRuntimeSequence, PartRuntimeParameterExpression})
                End If

                Return _AverageExpression
            End Function
        End Class
    End Class
End Namespace