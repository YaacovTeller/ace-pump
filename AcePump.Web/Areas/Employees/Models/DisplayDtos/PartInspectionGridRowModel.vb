Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations
Imports AcePump.Web.Areas.Employees.Models.Validation

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PartInspectionGridRowModel
        Public Property PartInspectionID As Integer

        <NotMapped()>
        Public Property DeliveryTicketID As Integer

        Public Property SortOrder As Integer
        Public Property HasParentAssembly As Boolean
        Public Property ParentAssemblyID As Integer?
        Public Property IsSplitAssembly As Boolean?
        Public Property IsConvertible As Boolean

        <UIHint("InspectionResult")>
        <DisplayName("Status")>
        <PartInspectionResultValidation()>
        Public Property Result As String
        <UIHint("ReasonRepaired")>
        Public Property ReasonRepaired As String

        <DisplayName("Part Number")>
        Public Property OriginalPartTemplateNumber As String
        Public Property OriginalPartTemplateID As Integer
        Public Property TemplatePartDefID As Integer?
        Public Property PartDescription As String
        Public Property Quantity As Integer
        Public Property CanBeRepresentedAsAssembly As Boolean

        <DisplayName("Replacement Part Number")>
        <UIHint("PartTemplateNumber")>
        <AdditionalMetadata("PartTemplateNumberProperty", "ReplacementPartTemplateNumber")>
        Public Property PartReplacedID As Integer?
        Public Property ReplacementPartTemplateNumber As String

        <UIHint("Integer")>
        Public Property ReplacementQuantity As Integer?

        Public Property OriginalCustomerOwnedPartID As Integer?
        Public Property ReplacedWithInventoryPartID As Integer?
    End Class
End Namespace