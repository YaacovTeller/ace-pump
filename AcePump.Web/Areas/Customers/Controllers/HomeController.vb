Imports AcePump.Web.Controllers

Namespace Areas.Customers.Controllers
    <Authorize()> _
    Public Class HomeController
        Inherits AcePumpControllerBase

        '
        ' GET: /[Home]/[Index]

        Public Function Index() As ActionResult
            Return View()
        End Function
    End Class
End Namespace
