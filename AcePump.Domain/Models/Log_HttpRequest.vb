Namespace Models
    Public Class Log_HttpRequest
        Public Property Log_HttpRequestID As Integer

        Public Overridable Property Parameters As ICollection(Of Log_HttpRequestParam)

        Public Property Environment As String
        Public Property Path As String
        Public Property RequestTime As DateTime
        Public Property HttpMethod As String
        Public Property LoggedInUsername As String
    End Class
End Namespace