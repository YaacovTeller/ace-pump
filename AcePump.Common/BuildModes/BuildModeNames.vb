Namespace BuildModes
    Public Class BuildModeNames
        Public Shared Function GetCurrentBuildModeName() As String
#If DEBUG Then
            Return "Debug"
#ElseIf BETA Then
        Return "Beta"
#ElseIf RELEASE Then
        Return "Release"
#ElseIf ALPHA Then
        Return "Alpha"
#Else
        Throw New ArgumentException("unknown build mode")
#End If
        End Function
    End Class
End Namespace
