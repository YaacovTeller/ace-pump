Namespace Models
    Public Class AssemblyPartDef
        Public Property AssemblyPartDefID As Integer

        Public Property AssemblyID As Integer?
        Public Overridable Property Assembly As Assembly

        Public Property PartTemplateID As Integer?
        Public Overridable Property PartTemplate As PartTemplate

        Public Property PartsQuantity As Integer

        Public Property SortOrder As Integer
    End Class
End Namespace