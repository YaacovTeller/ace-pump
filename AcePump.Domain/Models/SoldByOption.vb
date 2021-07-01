Imports Soris.Mvc.Modules.TypeManager.Models

Namespace Models
    Public Class SoldByOption
        Implements IItemType

        Public Property SoldByOptionID As Integer Implements IItemType.ItemTypeID

        Public Property Description As String Implements IItemType.DisplayText
    End Class
End Namespace