
Imports AcePump.Common

Namespace Filters
    Public Class ForceLogoutOnInvalidAuthCookieFilter
        Implements IAuthorizationFilter

        Public Sub OnAuthorization(filterContext As AuthorizationContext) Implements IAuthorizationFilter.OnAuthorization
            If Not IsAccountPageRequest(filterContext) Then
                Dim authCookie As HttpCookie = filterContext.HttpContext.Request.Cookies(FormsAuthentication.FormsCookieName)
                If authCookie IsNot Nothing Then
                    Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)

                    Dim authTicketsValidAfterDate As DateTime = AcePumpEnvironment.Environment.Configuration.AuthTicketsValidAfterDate

                    If ticket.IssueDate < authTicketsValidAfterDate Then
                        FormsAuthentication.SignOut()
                        filterContext.Result = New RedirectResult("~/Account/Login")
                    End If
                End If
            End If
        End Sub

        Public Function IsAccountPageRequest(filterContext As AuthorizationContext)
            If filterContext.RouteData.Values("controller") = "Account" Then
                If filterContext.RouteData.Values("action") = "Login" Or filterContext.RouteData.Values("action") = "Logout" Then
                    Return True
                End If
            End If
            Return False
        End Function
    End Class
End Namespace
