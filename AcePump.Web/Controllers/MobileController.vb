Imports AcePump.Web.Models

Namespace Controllers
    Public Class MobileController
        Inherits AcePumpControllerBase

        '
        ' GET: /Mobile/SignIn

        <HttpGet()> _
        Public Function SignIn() As ActionResult
            Return View()
        End Function

        '
        ' POST: /Mobile/SignIn

        <HttpPost()> _
        Public Function SignIn(model As MobileLoginViewModel) As ActionResult
            If ModelState.IsValid() Then
                If Membership.ValidateUser(model.UserName, model.Password) Then
                    AccountService.SetUserLoggedInToThisSession(model.UserName, model.RememberMe)

                    Return RedirectToAction("Choose", "DeliveryTicket")
                Else
                    ModelState.AddModelError("", "The user name or password provided is incorrect.")
                End If
            End If

            Return View(model)
        End Function
    End Class
End Namespace