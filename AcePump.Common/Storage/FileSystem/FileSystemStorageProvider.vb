Namespace Storage.FileSystem
    Public Class FileSystemStorageProvider
        Implements IStorageProvider

        Private Property FileSystemRootPath As String

        Public Sub New(fileSystemRootPath As String)
            Me.FileSystemRootPath = fileSystemRootPath
        End Sub

        Public Function GetContainer(containerPath As String) As IStorageContainer Implements IStorageProvider.GetContainer
            Return New FileSystemStorageContainer(FileSystemRootPath, containerPath)
        End Function
    End Class
End Namespace