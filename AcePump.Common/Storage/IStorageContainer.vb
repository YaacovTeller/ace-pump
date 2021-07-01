Namespace Storage
    Public Interface IStorageContainer
        Function GetFile(fileName As String) As IStorageFile
        Function GetRandomFilename() As String
    End Interface
End Namespace