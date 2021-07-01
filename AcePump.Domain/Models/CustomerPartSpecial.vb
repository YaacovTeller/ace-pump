Namespace Models
    Public Class CustomerPartSpecial
        Public Property CustomerPartSpecialID As Integer

        Public Property CustomerID As Integer
        Public Overridable Property Customer As Customer

        Public Property PartTemplateID As Integer
        Public Overridable Property PartTemplate As PartTemplate

        Public Property Discount As Decimal
    End Class
End Namespace