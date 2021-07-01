Imports System.IO

Namespace Storage.FileSystem
    Public Class FileSystemStorageFile
        Implements IStorageFile

        Private Property FilePath As String

        Public Sub New(filePath As String)
            Me.FilePath = filePath
        End Sub

        Public Function OpenWrite() As Stream Implements IStorageFile.OpenWrite
            Return File.OpenWrite(FilePath)
        End Function

        Public Function OpenRead() As Stream Implements IStorageFile.OpenRead
            Return File.OpenRead(FilePath)
        End Function

        Public Sub Delete() Implements IStorageFile.Delete
            File.Delete(FilePath)
        End Sub
    End Class
End Namespace