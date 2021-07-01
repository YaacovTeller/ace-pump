Namespace Areas
    Public Class ChooseAreaByRoleConstraint
        Implements IRouteConstraint

        Private Property AreaName As String
        Private Property RoleName As String

        Friend Sub New(areaName As String, roleName As String)
            Me.AreaName = areaName
            Me.RoleName = roleName
        End Sub

        Public Function Match(httpContext As HttpContextBase, route As Route, parameterName As String, values As RouteValueDictionary, routeDirection As RouteDirection) As Boolean Implements IRouteConstraint.Match
            Return httpContext.AcePumpUser.IsInRole(RoleName) And route.DataTokens("area") = AreaName
        End Function
    End Class
End Namespace