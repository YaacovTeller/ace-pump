Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource
Imports System.Web.Configuration
Imports System.Data.Entity

Namespace Api.Services

    Public Class TokenService
        Implements ITokenService

        Private ReadOnly _dataSource As AcePumpContext
        Public Sub New(dataSource As AcePumpContext)
            _dataSource = dataSource
        End Sub

        Public Function GenerateToken(userID As Integer) As ApiToken Implements ITokenService.GenerateToken
            Dim token As String = Guid.NewGuid().ToString()
            Dim issuedOn As DateTime = DateTime.Now
            Dim expiredOn As DateTime = DateTime.Now.AddSeconds(Convert.ToDouble(My.Settings("AuthTokenExpiry")))
            Dim tokendomain = New ApiToken() With { _
                    .UserID = userID, _
                    .AuthToken = token, _
                    .IssuedOn = issuedOn, _
                    .ExpiresOn = expiredOn _
                    }

            _dataSource.ApiTokens.Add(tokendomain)
            _dataSource.SaveChanges()

            Dim tokenModel = New ApiToken() With { _
                    .UserID = userID, _
                     .IssuedOn = issuedOn, _
                     .ExpiresOn = expiredOn, _
                     .AuthToken = token _
                    }

            Return tokenModel
        End Function

        Public Function ValidateToken(tokenID As String) As Boolean Implements ITokenService.ValidateToken
            Dim token As ApiToken = GetNonExpiredToken(tokenID)
            If (token IsNot Nothing) AndAlso Not (DateTime.Now > token.ExpiresOn) Then
                token.ExpiresOn = token.ExpiresOn.AddSeconds(Convert.ToDouble(Convert.ToDouble(My.Settings("AuthTokenExpiry"))))
                _dataSource.SaveChanges()
                Return True
            End If
            Return False
        End Function

        Public Function GetNonExpiredToken(tokenID As String) As ApiToken
            Dim token As ApiToken = _dataSource.ApiTokens.Include(Function(x) x.User).FirstOrDefault(Function(x) x.AuthToken = tokenID And x.ExpiresOn > Now())
            If (token IsNot Nothing) AndAlso Not (DateTime.Now > token.ExpiresOn) Then
                Return token
            End If
            Return Nothing
        End Function

        Public Function GetUserFromToken(tokenID As String) As String
            Dim token As ApiToken = GetNonExpiredToken(tokenID)
            If token IsNot Nothing Then
                Return token.User.Username
            End If
            Return ""
        End Function

        Public Function Kill(tokenID As String) As Boolean Implements ITokenService.Kill
            Dim token As ApiToken = _dataSource.ApiTokens.FirstOrDefault(Function(x) x.AuthToken = tokenID)
            If token IsNot Nothing Then
                _dataSource.ApiTokens.Remove(token)
            End If
            Dim isNotDeleted As Boolean = _dataSource.ApiTokens.Any(Function(x) x.AuthToken = tokenID)
            If isNotDeleted Then
                Return False
            End If
            Return True
        End Function

        Public Function DeleteByUserId(userId As Integer) As Boolean Implements ITokenService.DeleteByUserId
            Dim token As ApiToken = _dataSource.ApiTokens.FirstOrDefault(Function(x) x.UserID = userId)
            If token IsNot Nothing Then
                _dataSource.ApiTokens.Remove(token)
            End If
            Dim isNotDeleted As Boolean = _dataSource.ApiTokens.Any(Function(x) x.UserID = userId)
            If isNotDeleted Then
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace