Imports System.Text.RegularExpressions
Imports Microsoft.WindowsAzure.Storage.Blob

Namespace Storage.Azure
    Public Class AzureStorageContainer
        Implements IStorageContainer

        Private Property AzureStorageContainer As CloudBlobContainer

        Public Sub New(azureStorageContainer As CloudBlobContainer)
            Me.AzureStorageContainer = azureStorageContainer
        End Sub

        Public Function GetFile(fileName As String) As IStorageFile Implements IStorageContainer.GetFile
            Dim rgIsValidFileName As New Regex("[-._~:/?#\[\]@!$&'()*+,;=A-Za-z0-9]", RegexOptions.None)
            If Not rgIsValidFileName.IsMatch(fileName) Then
                Throw New ArgumentException($"`{fileName}` is not a valid Azure blob name.  See naming rules at https://blogs.msdn.microsoft.com/jmstall/2014/06/12/azure-storage-naming-rules/")
            End If

            Dim blob As CloudBlockBlob = AzureStorageContainer.GetBlockBlobReference(fileName)
            Return New AzureStorageFile(blob)
        End Function

        Public Function GetRandomFilename() As String Implements IStorageContainer.GetRandomFilename
            Return Guid.NewGuid().ToString()
        End Function
    End Class
End Namespace