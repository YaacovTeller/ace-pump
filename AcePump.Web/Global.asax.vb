' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Imports AcePump.Common
Imports AcePump.Web.Api.ActionFilters
Imports AcePump.Domain.Models
Imports AcePump.Web.Areas.Employees
Imports AcePump.Web.Areas.Customers
Imports AcePump.Common.Soris
Imports Yesod.ExceptionHandler
Imports Yesod.Tools
Imports Soris.Mvc.Modules.TypeManager.Repositories
Imports AcePump.Web.Models
Imports System.Web.Hosting
Imports Soris.Mvc.Modules.Common.EmbeddedViews
Imports AcePump.Rdlc.Builder
Imports AcePump.Web.Filters
Imports System.Web.Http
Imports AcePump.Web.ActionFilters
Imports System.Net

Public Class MvcApplication
    Inherits HttpApplication

    Private Const UseNamespaceFallback As String = "UseNamespaceFallback"

    Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection, Optional context As HttpContextBase = Nothing)
        If context Is Nothing Then context = New HttpContextWrapper(HttpContext.Current)

        filters.Add(New YesodHandleExceptionAttribute(New ExceptionHandlerSettings() With {
                                                            .AccumulationSettings = New AccumulationSettings() With {
                                                                .SendEmailFrom = "exceptions@netsmithcentral.com",
                                                                .SendEmailTo = {"mkosbie@netsmithcentral.com"},
                                                                .Subject = "Exception Notice: Ace Pump",
                                                                .Threshold = New TimedThreshold(30),
                                                                .MessageText = "Ace Pump's system reported the following errors."
                                                            },
                                                            .LogFilePath = context.Server.MapPath("~/ErrorLogs"),
                                                            .ExceptionLogger = New YesodExceptionLogger(context.Server.MapPath("~/ErrorLogs"))
                                                        }))
        filters.Add(New AppendWebApiTokenToAuthFilter())
        filters.Add(New ForceLogoutOnInvalidAuthCookieFilter())

        filters.Add(New BugzScoutMvc3ExceptionFilter())
        filters.Add(New LogRequestAttribute())
        filters.Add(New HandleErrorAttribute())
    End Sub

    Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        RegisterApiRoutes(routes)

        RegisterNotAuthenticatedRoute(routes)
        RegisterExplicitRoutes(routes)
        RegisterSorisModuleRoutes(routes)
        RegisterAreaRoutes(routes)
        RegisterDefaultRoutes(routes)
    End Sub

    Private Shared Sub RegisterNotAuthenticatedRoute(routes As RouteCollection)
        Dim notAuthenticatedRoute As New Route("{controller}/{action}/{id}", New DynamicLoginPageRouteHandler())
        notAuthenticatedRoute.Constraints = New RouteValueDictionary(New With {.NotAuthenticated = New NotAuthenticatedRouteConstraint()})
        notAuthenticatedRoute.Defaults = New RouteValueDictionary(New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional})

        routes.Add(notAuthenticatedRoute)
    End Sub

    Private Shared Sub RegisterExplicitRoutes(routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.IgnoreRoute("{*favicon}", New With {.favicon = "(.*)?favicon.ico(/.*)?"})

        routes.MapRoute(
            "History",
            "History/{action}/{id}",
            New With {.controller = "History", .action = "CustomerDashboard", .id = UrlParameter.Optional}
        )

        routes.MapPageRoute(
            "Report_Route",
            "Report/{reportName}",
            "~/Views/ReportViewer/ReportViewer.aspx"
        )
    End Sub

    Private Shared Sub RegisterApiRoutes(routes As RouteCollection)
        routes.MapHttpRoute("DefaultApi",
                "api/{controller}/{action}/{id}",
                New With {.id = RouteParameter.Optional})
    End Sub

    Private Shared Sub RegisterSorisModuleRoutes(routes As RouteCollection)
        Dim r As Route = routes.MapRoute(
            "Soris_Account",
            "Account/{action}/{id}",
            New With {.controller = "Account", .action = "Login", .id = UrlParameter.Optional},
            {"Soris.Mvc.Modules.Account.Controllers"}
        )
        r.DataTokens(UseNamespaceFallback) = False

        r = routes.MapRoute(
            "Soris_TypeManager",
            "TypeManager/{type}/{action}/{id}",
            New With {.controller = "TypeManager", .action = "Index", .id = UrlParameter.Optional},
            {"Soris.Mvc.Modules.TypeManager"}
        )
        r.DataTokens(UseNamespaceFallback) = False
    End Sub

    Private Shared Sub RegisterAreaRoutes(routes As RouteCollection)
        EmployeeAreaRegistration.RegisterRoutes(routes)
        CustomersAreaRegistration.RegisterRoutes(routes)
    End Sub

    Private Shared Sub RegisterDefaultRoutes(routes As RouteCollection)
        Dim r As Route = routes.MapRoute(
            "Default_Route",
            "{controller}/{action}/{id}",
            New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional},
            {"AcePump.Web.Controllers"}
        )
        r.DataTokens(UseNamespaceFallback) = False
    End Sub

    Private Sub RegisterSorisModules()
        Dim connectionString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(AcePumpEnvironment.Environment.Configuration.Database.ConnectionStringName)

        Soris.Mvc.Modules.TypeManager.TypeManagerController.Register() _
            .Permissions(Sub(permissions)
                             permissions.Default({AcePumpSecurityRoles.TypeManager})
                             permissions.Action("ListOnly", {AcePumpSecurityRoles.AcePumpEmployee})
                         End Sub) _
            .RepositoryResolver(Function(itemTypeName As String) New DbItemTypeRepository(itemTypeName, connectionString.ConnectionString, connectionString.ProviderName, "Types_{0}", "ItemTypeID"))

        Soris.Mvc.Modules.Account.Controllers.AccountController.Register() _
            .DataSourceFactoryMethod("AcePump.Domain.DataSourceFactory, AcePump.Domain", "GetAcePumpDataSource") _
            .Profile(Of AcePumpProfile)(Sub(profile)
                                            profile.ModelType(Of AcePumpProfileModel)()
                                        End Sub) _
            .Permissions(Sub(permission)
                             permission.Default({AcePumpSecurityRoles.AccountManager})
                             permission.Action("Login", {})
                             permission.Action("Logout", {})
                             permission.Action("Unimpersonate", {})
                         End Sub)

        HostingEnvironment.RegisterVirtualPathProvider(New EmbeddedResourcePathProvider())
    End Sub

    Private Sub ConfigureGlobalDisplaySettings()
        With Yesod.Widgets.DirectionChangeWidget.GlobalDisplaySettings
            .ColorCodePositiveResult = False
            .DisplayAmountChangedText = False
            .DisplayDirectionChangeArrow = False
        End With
    End Sub

    Private Sub ConfigureWebApi()
        GlobalConfiguration.Configuration.Filters.Add(New LogWebApiExceptionAttribute())
        GlobalConfiguration.Configuration.Filters.Add(New BugzScoutWebApiExceptionAttribute())
        ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
    End Sub

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RegisterSorisModules()
        RegisterGlobalFilters(GlobalFilters.Filters, New HttpContextWrapper(Context))
        RegisterRoutes(RouteTable.Routes)

        ConfigureGlobalDisplaySettings()
        ConfigureWebApi()

        System.ComponentModel.TypeDescriptor.AddAttributes(GetType(Double?), New System.ComponentModel.TypeConverterAttribute(GetType(NullableConverterWithDecimalSupport)))

        GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter)

        RdlcBuilder.SetPathMapper(Function(x) HostingEnvironment.MapPath("~/bin/Rdlc/" & x))
    End Sub
End Class
