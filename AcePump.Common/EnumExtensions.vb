Imports System.ComponentModel

Public Module EnumExtension
    <System.Runtime.CompilerServices.Extension()> _
    Public Function GetDescription(enumerationValue As [Enum]) As String
        Dim type = enumerationValue.GetType()
        If Not type.IsEnum Then
            Throw New ArgumentException("Must be of Enum type")
        End If
        Dim memberInfo = type.GetMember(enumerationValue.ToString())
        If memberInfo.Length > 0 Then
            Dim attrs = memberInfo(0).GetCustomAttributes(GetType(DescriptionAttribute), False)
            If attrs.Length > 0 Then
                Return DirectCast(attrs(0), DescriptionAttribute).Description
            End If
        End If
        Return enumerationValue.ToString()
    End Function
End Module
