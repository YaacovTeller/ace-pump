Namespace Soris
    Public Class NaturalStringComparer
        Implements IComparer(Of String)

        Public Function Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
            If x Is Nothing AndAlso y Is Nothing Then
                Return 0
            ElseIf x Is Nothing Then
                Return -1
            ElseIf y Is Nothing Then
                Return 1
            End If
            Return NativeMethods.StrCmpLogicalW(x, y)
        End Function
    End Class
End Namespace