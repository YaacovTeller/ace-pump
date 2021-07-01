Imports System.Text.RegularExpressions
Imports Microsoft.WindowsAzure.Storage
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Storage.Azure
    Public Class AzureStorageProvider
        Implements IStorageProvider

        Private Property StorageAccountConnectionString As String

        Public Sub New(storageAccountConnectionString As String)
            Me.StorageAccountConnectionString = storageAccountConnectionString
        End Sub

        Public Function GetContainer(containerPath As String) As IStorageContainer Implements IStorageProvider.GetContainer
            Dim rgValidContainerName As New Regex("^[a-z0-9][-a-z0-9][-a-z0-9][-a-z0-9]{0,60}$", RegexOptions.None)
            If Not rgValidContainerName.IsMatch(containerPath) Then
                Throw New ArgumentException($"`{containerPath}` is not a valid container name.  See container name rules at https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/.")
            End If

            Dim storageAccount As CloudStorageAccount = CloudStorageAccount.Parse(StorageAccountConnectionString)
            Dim blobClient As CloudBlobClient = storageAccount.CreateCloudBlobClient()
            Dim container As CloudBlobContainer = blobClient.GetContainerReference(containerPath)
            container.CreateIfNotExists()

            Return New AzureStorageContainer(container)
        End Function
    End Class
End Namespace