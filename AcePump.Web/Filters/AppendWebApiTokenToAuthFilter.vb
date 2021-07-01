Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports AcePump.Common
Imports Newtonsoft.Json.Linq

Namespace Filters
    Public Class AppendWebApiTokenToAuthFilter
        Implements IActionFilter

        Public Sub OnActionExecuting(filterContext As ActionExecutingContext) Implements IActionFilter.OnActionExecuting
        End Sub

        Public Sub OnActionExecuted(filterContext As ActionExecutedContext) Implements IActionFilter.OnActionExecuted
            If filterContext.RouteData.Values("controller") = "Account" And filterContext.RouteData.Values("action") = "Login" And filterContext.HttpContext.Request.HttpMethod = "POST" Then
                Dim username As String = filterContext.HttpContext.Request.Form("Username")
                Dim password As String = filterContext.HttpContext.Request.Form("Password")
                Dim token As WebApiAuthToken = Task.Run(Function() GetWebApiAuthToken(username, password)).Result

                If token IsNot Nothing Then
                    Dim cookie As New HttpCookie("webapitoken") With {
                    .Value = token.Token,
                    .Expires = Date.Today.AddSeconds(token.ExpiresInSeconds)
                    }
                    filterContext.HttpContext.Response.SetCookie(cookie)
                End If
            End If
        End Sub

        Private Async Function GetWebApiAuthToken(username As String, password As String) As Task(Of WebApiAuthToken)
            Using client As New HttpClient
                client.BaseAddress = New Uri(AcePumpEnvironment.Environment.Configuration.PtpApi.UriV2)
                Dim request As HttpRequestMessage = New HttpRequestMessage(HttpMethod.Post, "/auth/token")
                Dim keyValues As New List(Of KeyValuePair(Of String, String))
                keyValues.Add(New KeyValuePair(Of String, String)("grant_type", "password"))
                keyValues.Add(New KeyValuePair(Of String, String)("username", username))
                keyValues.Add(New KeyValuePair(Of String, String)("password", password))
                request.Content = New FormUrlEncodedContent(keyValues)

                Dim response = Await client.SendAsync(request)
                If response.IsSuccessStatusCode Then
                    Dim bearerData = Await response.Content.ReadAsStringAsync()

                    Dim bearerToken = JObject.Parse(bearerData)("access_token").ToString()
                    Dim tokenResponse As New WebApiAuthToken With {
                        .Token = JObject.Parse(bearerData)("access_token").ToString(),
                        .ExpiresInSeconds = CInt(JObject.Parse(bearerData)("expires_in"))
                    }
                    Return tokenResponse
                Else
                    Return Nothing
                End If
            End Using
        End Function

        Private Class WebApiAuthToken
            Public Property Token As String
            Public Property ExpiresInSeconds As Integer
        End Class
    End Class
End Namespace
