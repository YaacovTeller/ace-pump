Imports Soris.Mvc.Modules.TypeManager.Models

Namespace Models
    Public Class PartCategory
        Implements IItemType

        Public Property PartCategoryID As Integer Implements IItemType.ItemTypeID

        Public Property CategoryName As String Implements IItemType.DisplayText
        Public Property CategoryDescription As String
    End Class
End Namespace