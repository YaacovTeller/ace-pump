Imports System.Configuration

Namespace BuildModes
    Public NotInheritable Class BuildModesConfigurationSection
        Inherits ConfigurationSection

        <ConfigurationProperty("", IsDefaultCollection:=True)>
        <ConfigurationCollection(GetType(BuildModesCollection), AddItemName:="mode", ClearItemsName:="removeAll", RemoveItemName:="remove")>
        Public ReadOnly Property Modes() As BuildModesCollection
            Get
                Return DirectCast(Me(""), BuildModesCollection)
            End Get
        End Property
    End Class
End Namespace
