Namespace Storage
    Public Interface IStorageProvider
        Function GetContainer(containerPath As String) As IStorageContainer
    End Interface
End Namespace