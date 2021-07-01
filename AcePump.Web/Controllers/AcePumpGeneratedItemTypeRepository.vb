Imports System.Data.Entity
Imports AcePump.Web.Areas.Employees.Controllers
Imports Soris.Mvc.Modules.TypeManager.Models
Imports System.Reflection
Imports Soris.Mvc.Modules.TypeManager.Repositories

Namespace Controllers
    Public Class AcePumpGeneratedItemTypeRepository
        Inherits ItemTypeRepositoryBase

        Private Property ItemContext As DbContext
        Private Property ItemSet As Object 'As DbSet(Of IItemType)
        Private Property ItemTypeConstructor As ConstructorInfo

        Public Sub New(itemTypeName As String)
            Dim generator As New ItemTypeDbContextGenerator(itemTypeName)

            ItemContext = generator.GetDbContext()
            ItemSet = generator.GetDbSet(ItemContext)
            ItemTypeConstructor = generator.GetItemTypeConstructor()
        End Sub

        Protected Overrides Function AddItem(displayText As String) As IItemType
            Dim c As IItemType = ItemTypeConstructor.Invoke({})
            c.DisplayText = displayText

            ItemSet.Add(c)
            ItemContext.SaveChanges()

            Return c
        End Function

        Protected Overloads Overrides Sub DeleteItem(itemId As Integer)
            Dim m = GetItem(itemId)
            If m Is Nothing Then Return

            Delete(m)
        End Sub

        Protected Overloads Overrides Sub DeleteItem(item As IItemType)
            ItemSet.Remove(item)
            ItemContext.SaveChanges()
        End Sub

        Protected Overrides Sub EditItem(itemId As Integer, newDisplayText As String)
            Dim item As IItemType = GetItem(itemId)

            item.DisplayText = newDisplayText
            ItemContext.SaveChanges()
        End Sub

        Protected Overrides Function QueryAll() As IEnumerable(Of IItemType)
            Return ItemSet
        End Function
    End Class
End Namespace