Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class AssemblyPartListRowViewModel

        Public Property AssemblyPartDefID As Integer
        Public Property AssemblyID As Integer

        <UIHint("Integer")> _
        Public Property PartsQuantity As Integer

        <UIHint("PartTemplateNumber")>
        <AdditionalMetadata("PartTemplateNumberProperty", "PartTemplateNumber")>
        Public Property PartTemplateID As Integer
        Public Property PartTemplateNumber As String
        Public Property Description As String

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property Cost As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property ResaleValue As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property TotalResaleValue As Decimal

        Public Property SortOrder As Integer
    End Class
End Namespace