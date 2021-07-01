Imports System.Data.Entity.ModelConfiguration
Imports AcePump.Domain.Models

Public Class Log_HttpRequestConfiguration
    Inherits EntityTypeConfiguration(Of Log_HttpRequest)

    Public Sub New()
        ToTable("Log_HttpRequests")
        HasMany(Function(x) x.Parameters).WithRequired(Function(x) x.HttpRequest).HasForeignKey(Function(x) x.Log_HttpRequestID)
    End Sub
End Class
