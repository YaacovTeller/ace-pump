Imports System.ComponentModel
Imports System.Globalization

Public Class NullableConverterWithDecimalSupport
    Inherits NullableConverter

    Public Sub New(type As Type)
        MyBase.New(type)
    End Sub

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        Dim baseCanConvert As Boolean = MyBase.CanConvertFrom(context, sourceType)

        If Not baseCanConvert And CanConvertFromWithAdditionalSupport(sourceType) Then
            Return True
        End If

        Return baseCanConvert
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
        Dim convertFromType As Type = value.GetType()
        If CanConvertFromWithAdditionalSupport(convertFromType) Then
            Return ConvertFromWithAdditionalSupport(value)

        Else
            Return MyBase.ConvertFrom(context, culture, value)
        End If
    End Function

    Private Function CanConvertFromWithAdditionalSupport(sourceType As Type)
        Return Nullable.GetUnderlyingType(NullableType) = GetType(Double) And (sourceType = GetType(Decimal) Or sourceType = GetType(Integer))
    End Function

    Private Function ConvertFromWithAdditionalSupport(value As Object) As Object
        Return CTypeDynamic(Of Double?)(value)
    End Function
End Class