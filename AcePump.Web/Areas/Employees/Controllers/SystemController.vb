Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports AcePump.Common.WebBackgroundTask

Namespace Areas.Employees.Controllers
    <Authorize(Roles:=AcePumpSecurityRoles.SysAdmin)> _
    Public Class SystemController
        Inherits AcePumpControllerBase

        '
        ' GET: /System/Index

        <HttpGet()> _
        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' POST: /System/GetTaskProgress

        <HttpPost()> _
        Public Function GetTaskProgress(id As String) As ActionResult
            Dim guid As Guid = guid.ParseExact(id, "N")
            Dim progress As WebBackgroundTaskModel = WebBackgroundTaskManager.GetProgress(guid)

            Return Json(progress)
        End Function
    End Class
End Namespace