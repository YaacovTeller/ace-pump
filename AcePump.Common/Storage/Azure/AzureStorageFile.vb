Imports System.IO
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Storage.Azure
    Public Class AzureStorageFile
        Implements IStorageFile

        Private Property AzureStorageFile As CloudBlockBlob

        Public Sub New(azureStorageFile As CloudBlockBlob)
            Me.AzureStorageFile = azureStorageFile
        End Sub

        Public Function OpenWrite() As Stream Implements IStorageFile.OpenWrite
            Return AzureStorageFile.OpenWrite()
        End Function

        Public Function OpenRead() As Stream Implements IStorageFile.OpenRead
            Return AzureStorageFile.OpenRead()
        End Function

        Public Sub Delete() Implements IStorageFile.Delete
            AzureStorageFile.Delete()
        End Sub
    End Class
End Namespace