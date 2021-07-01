Imports AcePump.Domain.DataSource
Imports Yesod.Widgets.Models
Imports Yesod.Widgets.Queries

Namespace WidgetQueries
    Public MustInherit Class AcePumpDirectionChangeQueryBase(Of TValue)
        Inherits DirectionChangeWidgetQueryBase(Of AcePumpContext, TValue)

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
            Me.New(dataSource, TimeSpan.FromDays(365))
        End Sub

        Public Sub New(dataSource As AcePumpContext, compareTo As TimeSpan)
            MyBase.New(dataSource, compareTo)
        End Sub

        Protected NotOverridable Overrides Function GetValue(query As QueryModel) As TValue
            Dim acePumpQuery As New AcePumpQueryModel(query)

            Return GetValue(acePumpQuery)
        End Function

        Protected MustOverride Overloads Function GetValue(query As AcePumpQueryModel) As TValue
    End Class
End Namespace