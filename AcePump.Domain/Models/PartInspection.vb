Namespace Models
    Public Class PartInspection
        Public Property PartInspectionID As Integer

        Public Property DeliveryTicketID As Integer
        Public Overridable Property DeliveryTicket As DeliveryTicket

        Public Property ParentAssemblyID As Integer?
        Public Property ParentAssembly As PartInspection
        Public Property IsSplitAssembly As Boolean?
        Public Property IsConvertible As Boolean?

        Public Property PartReplacedID As Integer?
        Public Overridable Property PartReplaced As PartTemplate
        Public Property ReplacementQuantity As Integer?

        Public Property PartFailedID As Integer?
        Public Overridable Property PartFailed As PartTemplate
        Public Property Quantity As Integer?

        Public Property Result As String
        Public Property ReasonRepaired As String

        Public Property Sort As Integer?

        Public Property ReplacedWithInventoryPartID As Integer?
        Public Overridable Property ReplacedWithInventoryPart As Part

        Public Property TemplatePartDefID As Integer
        Public Overridable Property TemplatePartDef As TemplatePartDef
    End Class
End Namespace