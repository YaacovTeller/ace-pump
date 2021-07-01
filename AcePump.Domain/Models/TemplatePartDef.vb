Namespace Models
    Public Class TemplatePartDef
        Public Property TemplatePartDefID As Integer

        Public Property PartTemplateID As Integer
        Public Overridable Property PartTemplate As PartTemplate

        Public Property PumpTemplateID As Integer
        Public Overridable Property PumpTemplate As PumpTemplate

        Public Property Quantity As Integer
        Public Property SortOrder As Integer?
    End Class
End Namespace