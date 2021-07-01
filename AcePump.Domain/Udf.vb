Imports System.Data.Entity

Public Class Udf
    <DbFunction("CodeFirstDatabaseSchema", "ClrRound_10_4")> _
    Public Shared Function ClrRound_10_4(value As Decimal) As Decimal
        Debug.WriteLine("Udf.ClrRound_10_4 has no effect when called directly")

        Return value
    End Function
End Class
