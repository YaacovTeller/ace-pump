Imports System.Web.Http.Filters
Imports System.Web.Hosting
Imports Yesod.ExceptionHandler

Namespace Api.ActionFilters
    Public Class LogWebApiExceptionAttribute
        Inherits ExceptionFilterAttribute

        Public Overrides Sub OnException(actionExecutedContext As HttpActionExecutedContext)
            Dim ctx As New ExceptionHandlingContext() With {
                .Exception = actionExecutedContext.Exception,
                .HttpContext = Nothing,
                .Handled = False
            }

            Dim logger = New YesodExceptionLogger(HostingEnvironment.MapPath("~/ErrorLogs"))
            logger.Log(ctx)
        End Sub
    End Class
End Namespace