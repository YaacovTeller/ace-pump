Imports System.Web.Routing
Imports System.Web

Namespace Soris
    Public Class NotAuthenticatedRouteConstraint
        Implements IRouteConstraint

        Public Function Match(httpContext As HttpContextBase, route As Route, parameterName As String, values As RouteValueDictionary, routeDirection As RouteDirection) As Boolean Implements IRouteConstraint.Match
            Select Case routeDirection
                Case Routing.RouteDirection.IncomingRequest
                    Return Not httpContext.Request.IsAuthenticated

                Case Routing.RouteDirection.UrlGeneration
                    Return False

                Case Else
                    Throw New ArgumentException("route")
            End Select
        End Function
    End Class
End Namespace