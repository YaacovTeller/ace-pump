Imports Yesod.LinqProviders

Namespace Models
    Public Class ApiToken
        Public Property ApiTokenID As Integer
        Public Property UserID As Integer
        Public Overridable Property User As AccountDataStoreUser
        Public Property AuthToken As String
        Public Property IssuedOn As Date
        Public Property ExpiresOn As Date
    End Class
End Namespace