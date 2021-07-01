Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Public Class Log_HttpRequestParamConfiguration
    Inherits EntityTypeConfiguration(Of Log_HttpRequestParam)

    Public Sub New()
        ToTable("Log_HttpRequestParams")
    End Sub
End Class
