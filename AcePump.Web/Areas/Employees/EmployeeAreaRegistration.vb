Imports AcePump.Common

Namespace Areas.Employees
    Public Class EmployeeAreaRegistration
        Inherits AreaRegistration

        Private Shared ReadOnly Property SharedAreaName As String
            Get
                Return "Employees"
            End Get
        End Property

        Public Overrides ReadOnly Property AreaName As String
            Get
                Return SharedAreaName
            End Get
        End Property

        Public Overrides Sub RegisterArea(context As AreaRegistrationContext)
        End Sub

        Public Shared Sub RegisterRoutes(routes As RouteCollection)
            MapAreaRoute(
                routes,
                "PumpHistoryList",
                "Pump/History/List/{id}",
                New With {.controller = "Pump", .action = "HistoryList", .id = UrlParameter.Optional}
            )

            MapAreaRoute(
                routes,
                "PumpTemplate",
                "Pump/Template/{action}/{id}",
                New With {.controller = "PumpTemplate", .action = "Index", .id = UrlParameter.Optional}
            )

            MapAreaRoute(
                routes,
                "Employee_default",
                "{controller}/{action}/{id}",
                New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional}
            )
        End Sub

        Private Shared Sub MapAreaRoute(routes As RouteCollection, name As String, urlPattern As String, defaults As Object)
            Dim constraints As Object = New With {.RoleConstraint = New ChooseAreaByRoleConstraint(SharedAreaName, AcePumpSecurityRoles.AcePumpEmployee)}
            Dim namespaces() As String = {"AcePump.Web.Areas.Employees.Controllers"}

            Dim route As Route = routes.MapRoute(name, urlPattern, defaults, constraints, namespaces)

            route.DataTokens("area") = SharedAreaName
            route.DataTokens("UseNamespaceFallback") = namespaces Is Nothing OrElse namespaces.Length = 0
        End Sub
    End Class
End Namespace