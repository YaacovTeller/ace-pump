Imports System.Web.Http
Imports AcePump.Domain.DataSource
Imports AcePump.Domain

Namespace Api.Controllers
    Public MustInherit Class AcePumpApiControllerBase
        Inherits ApiController

        Private Property _DataSource As AcePumpContext
        Public Property DataSource As AcePumpContext
            Get
                If _DataSource Is Nothing Then
                    _DataSource = DataSourceFactory.GetAcePumpDataSource()
                End If

                Return _DataSource
            End Get
            Set(value As AcePumpContext)
                _DataSource = value
            End Set
        End Property

        Public Sub New(dataSource As AcePumpContext)
            _DataSource = dataSource
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If _DataSource IsNot Nothing Then
                    _DataSource.Dispose()
                End If
            End If

            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace