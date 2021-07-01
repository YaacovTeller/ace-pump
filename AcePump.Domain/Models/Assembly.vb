Namespace Models
    Public Class Assembly
        Public Property AssemblyID As Integer

        Public Overridable Property Parts As ICollection(Of AssemblyPartDef)

        Public Property PartCategoryID As Integer?
        Public Overridable Property PartCategory As PartCategory

        Public Property AssemblyNumber As String
        Public Property Description As String
        Public Property Discount As Decimal?
        Public Property Markup As Decimal?
    End Class
End Namespace