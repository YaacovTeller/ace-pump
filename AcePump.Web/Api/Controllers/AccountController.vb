Imports System.Web.Http
Imports AcePump.Web.Api.Services
Imports System.Net.Http
Imports System.Net
Imports System.Threading
Imports AcePump.Web.Api.ActionFilters
Imports AcePump.Common

Namespace Api.Controllers

    <UseBasicAuth(AcePumpSecurityRoles.ApiQuickbooksUser)>
    Public Class AccountController
        Inherits AcePumpApiControllerBase

        Private _tokenService As ITokenService
        Public ReadOnly Property TokenService As TokenService
            Get
                If _tokenService Is Nothing Then
                    _tokenService = New TokenService(DataSource)
                End If
                Return _tokenService
            End Get
        End Property

        <HttpPost()> _
        Function Token() As HttpResponseMessage
            If Thread.CurrentPrincipal IsNot Nothing AndAlso Thread.CurrentPrincipal.Identity.IsAuthenticated Then
                Dim currentUser As MembershipUser = Membership.GetUser(Thread.CurrentPrincipal.Identity.Name)

                Return GetAuthToken(currentUser.ProviderUserKey)
            End If
            Return Nothing

            'do we need a role to use the quickbooks api - download quickbooks data
        End Function

        Private Function GetAuthToken(userId As Integer) As HttpResponseMessage
            Dim token = TokenService.GenerateToken(userId)
            Dim response = Request.CreateResponse(HttpStatusCode.OK, "Authorized")
            response.Headers.Add("Token", token.AuthToken)
            response.Headers.Add("TokenExpiry", My.Settings("AuthTokenExpiry"))
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry")
            Return response
        End Function
    End Class
End Namespace
