Namespace ImageProcessing
    Public Interface IImageProcessingLibrary
        Inherits IDisposable

        Function LoadImage(path As String) As IImage
        Function LoadImage(bytes As Byte()) As IImage
    End Interface
End Namespace