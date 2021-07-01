Imports System.Security
Imports System.Runtime.InteropServices

Namespace Soris
    <SuppressUnmanagedCodeSecurity()> _
    Module NativeMethods
        <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode)> _
        Public Function StrCmpLogicalW(psz1 As String, psz2 As String) As Integer
        End Function
    End Module
End Namespace