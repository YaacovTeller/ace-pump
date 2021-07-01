Imports Soris.Mvc.Modules.TypeManager.Models

Namespace Models
    Public Class Material
        Implements IItemType

        Public Property MaterialID As Integer Implements IItemType.ItemTypeID
        Public Property MaterialName As String Implements IItemType.DisplayText
    End Class
End Namespace