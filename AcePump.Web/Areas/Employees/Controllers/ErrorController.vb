Imports System.Web.Mvc
Imports AcePump.Web.Controllers

Namespace Areas.Employees.Controllers
    Public Class ErrorController
        Inherits AcePumpControllerBase

        Function Http404() As ActionResult
            Return View()
        End Function
    End Class
End Namespace