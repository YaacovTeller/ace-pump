Namespace ImageProcessing
    Public Class FallbackChainProcessingLibrary
        Implements IImageProcessingLibrary

        Public Property ProcessingChain As New List(Of IImageProcessingLibrary)

        Public Sub New(ParamArray processingChain As IImageProcessingLibrary())
            Me.ProcessingChain = processingChain.ToList()
        End Sub

        Public Function LoadImage(path As String) As IImage Implements IImageProcessingLibrary.LoadImage
            Return New FallbackChainImage(ProcessingChain, path)
        End Function

        Public Function LoadImage(bytes() As Byte) As IImage Implements IImageProcessingLibrary.LoadImage
            Return New FallbackChainImage(ProcessingChain, bytes)
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            For Each library In ProcessingChain
                library.Dispose()
            Next
        End Sub
    End Class
End Namespace