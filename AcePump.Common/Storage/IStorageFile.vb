Imports System.IO

Namespace Storage
    Public Interface IStorageFile
        Function OpenWrite() As Stream
        Function OpenRead() As Stream
        Sub Delete()
    End Interface
End Namespace