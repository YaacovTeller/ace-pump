Imports AcePump.Domain.DataSource
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    Public MustInherit Class AcePumpChartQueryBase(Of TDataPoint)
        Inherits ChartWidgetQueryBase(Of AcePumpContext, TDataPoint)

        Private _Common As CommonQueries
        Protected ReadOnly Property Common As CommonQueries
            Get
                If _Common Is Nothing Then
                    _Common = New CommonQueries(DataSource)
                End If

                Return _Common
            End Get
        End Property

        Public Sub New(dataSource As AcePumpContext)
            MyBase.New(dataSource)
        End Sub

        Protected NotOverridable Overrides Sub RunQuery(query As QueryModel, manager As ChartWidgetDataSetManager(Of TDataPoint))
            Dim acePumpQry As New AcePumpQueryModel(Query)

            RunQuery(acePumpQry, manager)
        End Sub

        Protected MustOverride Overloads Sub RunQuery(query As AcePumpQueryModel, manager As ChartWidgetDataSetManager(Of TDataPoint))
    End Class
End Namespace