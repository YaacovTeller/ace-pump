Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal

Namespace Storage.FileSystem
    Public Class FileSystemStorageContainer
        Implements IStorageContainer

        Private Property FileSystemRootPath As String
        Private Property ContainerName As String

        Public Sub New(fileSystemRootPath As String, containerName As String)
            Me.FileSystemRootPath = fileSystemRootPath
            Me.ContainerName = containerName
        End Sub

        Public Function GetFile(fileName As String) As IStorageFile Implements IStorageContainer.GetFile
            Dim filePath = CreatePathWithPermission(FileSystemRootPath, ContainerName, fileName)

            Return New FileSystemStorageFile(filePath)
        End Function

        Private Function CreatePathWithPermission(fileSystemRootPath As String, containerName As String, fileName As String) As String
            Dim filePath As String = Path.Combine(fileSystemRootPath, containerName)
            If Not Directory.Exists(filePath) Then
                Directory.CreateDirectory(filePath)

                Dim DirectoryInfo As DirectoryInfo = New DirectoryInfo(filePath)
                Dim Security As DirectorySecurity = DirectoryInfo.GetAccessControl()
                Dim allUsers = New SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, Nothing)

                Security.AddAccessRule(New FileSystemAccessRule(allUsers,
                            FileSystemRights.Modify,
                            AccessControlType.Allow))

                DirectoryInfo.SetAccessControl(Security)
            End If

            Return Path.Combine(filePath, fileName)
        End Function

        Public Function GetRandomFilename() As String Implements IStorageContainer.GetRandomFilename
            Return Guid.NewGuid().ToString()
        End Function
    End Class
End Namespace