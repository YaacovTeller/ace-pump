Imports ImageMagick

Namespace ImageProcessing
    Public Class MagickNetImageProcessingLibrary
        Implements IImageProcessingLibrary

        Public Function LoadImage(path As String) As IImage Implements IImageProcessingLibrary.LoadImage
            Return New MagickNetImageWrapper(New MagickImage(path))
        End Function

        Public Function LoadImage(bytes() As Byte) As IImage Implements IImageProcessingLibrary.LoadImage
            Return New MagickNetImageWrapper(New MagickImage(bytes))
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class
End Namespace