Imports System.Configuration

Namespace BuildModes
    Public Class BuildModesCollection
        Inherits ConfigurationElementCollection

        Public Overrides ReadOnly Property CollectionType As ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.AddRemoveClearMap
            End Get
        End Property

        Protected Overrides Function CreateNewElement() As ConfigurationElement
            Return New BuildModeConfigurationElement()
        End Function

        Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As Object
            Return (CType(element, BuildModeConfigurationElement)).Name
        End Function

        Default Public Shadows Property Item(ByVal index As Integer) As BuildModeConfigurationElement
            Get
                Return CType(BaseGet(index), BuildModeConfigurationElement)
            End Get

            Set(ByVal value As BuildModeConfigurationElement)
                If BaseGet(index) IsNot Nothing Then
                    BaseRemoveAt(index)
                End If

                BaseAdd(index, value)
            End Set
        End Property

        Default Public Shadows ReadOnly Property Item(ByVal Name As String) As BuildModeConfigurationElement
            Get
                Return CType(BaseGet(Name), BuildModeConfigurationElement)
            End Get
        End Property

        Public Function IndexOf(ByVal url As BuildModeConfigurationElement) As Integer
            Return BaseIndexOf(url)
        End Function

        Public Sub Add(ByVal url As BuildModeConfigurationElement)
            BaseAdd(url)
        End Sub

        Protected Overrides Sub BaseAdd(ByVal element As ConfigurationElement)
            BaseAdd(element, False)
        End Sub

        Public Sub Remove(ByVal url As BuildModeConfigurationElement)
            If BaseIndexOf(url) >= 0 Then BaseRemove(url.Name)
        End Sub

        Public Sub RemoveAt(ByVal index As Integer)
            BaseRemoveAt(index)
        End Sub

        Public Sub Remove(ByVal name As String)
            BaseRemove(name)
        End Sub

        Public Sub Clear()
            BaseClear()
        End Sub
    End Class
End Namespace
