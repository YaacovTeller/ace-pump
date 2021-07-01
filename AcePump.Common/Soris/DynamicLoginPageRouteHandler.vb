Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.Web

Namespace Soris
    Public Class DynamicLoginPageRouteHandler
        Inherits MvcRouteHandler

        Private Property MobileDeviceUserAgents As New List(Of String) From {
            "Android",
            "iPhone",
            "Blackberry",
            "iPad"
        }

        Protected Overrides Function GetHttpHandler(requestContext As RequestContext) As IHttpHandler
            If UserAgentIsMobileDevice(requestContext.HttpContext.Request.UserAgent) Then
                RouteToMobileLoginPage(requestContext)

            Else
                RouteToStandardLoginPage(requestContext)
            End If

            Return MyBase.GetHttpHandler(requestContext)
        End Function

        Private Sub RouteToMobileLoginPage(requestContext As RequestContext)
            Dim routeValues As RouteValueDictionary = requestContext.RouteData.Values
            routeValues("controller") = "Mobile"
            routeValues("action") = "SignIn"
            routeValues("ReturnUrl") = requestContext.HttpContext.Request.RawUrl

            Dim dataTokens As RouteValueDictionary = requestContext.RouteData.DataTokens
            dataTokens("area") = Nothing
            dataTokens("Namespaces") = {"AcePump.Web.Controllers"}
            dataTokens("UseNamespaceFallback") = False
        End Sub

        Private Sub RouteToStandardLoginPage(requestContext As RequestContext)
            Dim routeValues As RouteValueDictionary = requestContext.RouteData.Values
            routeValues("controller") = "Account"
            routeValues("action") = "Login"
            routeValues("ReturnUrl") = requestContext.HttpContext.Request.RawUrl

            Dim dataTokens As RouteValueDictionary = requestContext.RouteData.DataTokens
            dataTokens("area") = Nothing
            dataTokens("Namespaces") = {"Soris.Mvc.Modules.Account.Controllers"}
            dataTokens("UseNamespaceFallback") = False
        End Sub

        Private Function UserAgentIsMobileDevice(userAgent As String) As Boolean
            Return MobileDeviceUserAgents.Any(Function(mobileUserAgent) userAgent.Contains(mobileUserAgent))
        End Function
    End Class
End Namespace