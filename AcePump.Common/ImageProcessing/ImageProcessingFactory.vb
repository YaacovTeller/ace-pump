Namespace ImageProcessing
    Public Class ImageProcessingFactory
        Public Shared Function GetImageProcessingLibrary() As IImageProcessingLibrary
            Return New FallbackChainProcessingLibrary(New GdiPlusImageProcessingLibrary(), New MagickNetImageProcessingLibrary())
        End Function
    End Class
End Namespace