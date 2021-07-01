Namespace Models
    Public Class Part
        Public Property PartID As Integer
        Public Property PartTemplateID As Integer
        Public Overridable Property PartTemplate As PartTemplate
        Public Property CustomerID As Integer
        Public Overridable Property Customer As Customer
    End Class
End Namespace