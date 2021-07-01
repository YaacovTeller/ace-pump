Imports AcePump.Domain.Models
Imports AcePump.Web.Api.ActionFilters
Imports AcePump.Common
Imports AcePump.Web.Api.Models
Imports Yesod.Mvc

Namespace Api.Controllers
    <Authorize()>
    Public Class PartController
        Inherits AcePumpApiControllerBase

        <HttpGet()>
        Public Function [Get](customerID As Integer)
            Return DataSource.Parts
        End Function
    End Class
End Namespace
