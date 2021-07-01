Imports AcePump.Domain.Models

Namespace Api.Services
    Public Interface ITokenService
        Function GenerateToken(userId As Integer) As ApiToken
        Function ValidateToken(tokenId As String) As Boolean
        Function Kill(tokenId As String) As Boolean
        Function DeleteByUserId(userId As Integer) As Boolean
    End Interface
End Namespace