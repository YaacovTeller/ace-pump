Namespace Models
    Public Class Log_HttpRequestParam
        Public Property Log_HttpRequestParamID As Integer

        Public Property Log_HttpRequestID As Integer
        Public Overridable Property HttpRequest As Log_HttpRequest

        Public Property ParamType As String
        Public Property ParamName As String
        Public Property ParamValue As String
    End Class
End Namespace