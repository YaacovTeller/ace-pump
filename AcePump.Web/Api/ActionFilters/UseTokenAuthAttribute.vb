Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Net
Imports AcePump.Web.Api.Services
Imports AcePump.Domain

Namespace Api.ActionFilters
    Public Class UseTokenAuthAttribute
        Inherits System.Web.Http.Filters.ActionFilterAttribute

        Private Const Token As String = "Token"

        Public Overrides Sub OnActionExecuting(filterContext As HttpActionContext)
            Using db = DataSourceFactory.GetAcePumpDataSource()
                Dim tokenService = New TokenService(db)

                If filterContext.Request.Headers.Contains(Token) Then
                    Dim tokenValue = filterContext.Request.Headers.GetValues(Token).First()

                    If tokenService.ValidateToken(tokenValue) Then
                        ' Authed - do not modfiy response
                        Return
                    End If
                End If

                filterContext.Response = New HttpResponseMessage(HttpStatusCode.Unauthorized)

                MyBase.OnActionExecuting(filterContext)
            End Using
        End Sub
    End Class
End Namespace