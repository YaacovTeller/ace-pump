Imports System.Web.Http.Controllers
Imports System.Threading
Imports System.Security.Principal
Imports System.Web.Http.Filters
Imports System.Net.Http
Imports System.Net
Imports AcePump.Domain
Imports Yesod.LinqProviders

Namespace Api.ActionFilters
    <AttributeUsage(AttributeTargets.[Class] Or AttributeTargets.Method, AllowMultiple:=False)>
    Public Class UseBasicAuthAttribute
        Inherits AuthorizationFilterAttribute

        Private Property AcceptedRoles As String()

        Public Sub New(ParamArray acceptedRoles As String())
            Me.AcceptedRoles = acceptedRoles
        End Sub

        Public Overrides Sub OnAuthorization(filterContext As HttpActionContext)
            Using db = DataSourceFactory.GetAcePumpDataSource()
                Dim accountDataStoreService = New LinqAccountDataStoreService(db) With {.MinRequiredPasswordLength = Membership.MinRequiredPasswordLength, .MaxInvalidPasswordAttempts = Membership.MaxInvalidPasswordAttempts}
                Dim authModel As BasicAuthModel = GetBasicAuthFromContext(filterContext)
                If authModel Is Nothing Then
                    SetAuthChallengeResponse(filterContext)

                ElseIf Not accountDataStoreService.ValidateUser(authModel.Username, authModel.Password) Then
                    SetAuthChallengeResponse(filterContext)

                ElseIf Not Roles.GetRolesForUser(authModel.Username).Any(Function(u) AcceptedRoles.Contains(u)) Then
                    SetAuthChallengeResponse(filterContext)

                Else
                    Thread.CurrentPrincipal = New GenericPrincipal(New GenericIdentity(authModel.Username), Nothing)
                End If
            End Using
        End Sub

        Private Function GetBasicAuthFromContext(filterContext As HttpActionContext) As BasicAuthModel
            If filterContext.Request.Headers.Authorization IsNot Nothing AndAlso filterContext.Request.Headers.Authorization.Scheme = "Basic" Then
                Dim base64EncodedAuthData = filterContext.Request.Headers.Authorization.Parameter
                If String.IsNullOrEmpty(base64EncodedAuthData) Then Return Nothing

                Dim authData = Encoding.[Default].GetString(Convert.FromBase64String(base64EncodedAuthData))
                Dim authDataParts = authData.Split(":"c)

                If authDataParts.Length = 2 Then
                    Return New BasicAuthModel With {
                    .Username = authDataParts(0),
                    .Password = authDataParts(1)
                }
                End If
            End If

            Return Nothing
        End Function

        Private Shared Sub SetAuthChallengeResponse(filterContext As HttpActionContext)
            Dim dnsHost = filterContext.Request.RequestUri.DnsSafeHost
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized)
            filterContext.Response.Headers.Add("WWW-Authenticate", String.Format("Basic realm=""{0}""", dnsHost))
        End Sub

        Private Class BasicAuthModel
            Public Property Username As String
            Public Property Password As String
        End Class
    End Class
End Namespace