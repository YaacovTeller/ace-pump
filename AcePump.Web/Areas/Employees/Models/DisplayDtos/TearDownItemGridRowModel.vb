Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports AcePump.Web.Areas.Employees.Models.Validation

Namespace Areas.Employees.Models.DisplayDtos
    Public Class TearDownItemGridRowModel
        Public Property PartInspectionID As Integer

        <NotMapped()>
        Public Property DeliveryTicketID As Integer

        Public Property SortOrder As Integer

        <UIHint("InspectionResult")>
        <DisplayName("Result")>
        Public Property Result As String
        <UIHint("ReasonRepaired")>
        Public Property ReasonRepaired As String

        <DisplayName("Part Number")>
        Public Property OriginalPartTemplateNumber As String
        Public Property OriginalPartTemplateID As Integer
        Public Property PartDescription As String
        Public Property Quantity As Integer
        Public Property CanBeRepresentedAsAssembly As Boolean
        Public Property HasParentAssembly As Boolean
    End Class
End Namespace