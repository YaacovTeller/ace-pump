Imports System.IO

Namespace ImageProcessing
    Public Class FallbackChainImage
        Implements IImage

        Private Property ProcessingChain As List(Of IImageProcessingLibrary)
        Private Property IxProcessingChain As Integer
        Private Property CurrentImage As IImage

        Private Property OriginalImagePath As String
        Private Property OriginalImageData As Byte()

        Public Sub New(processingChain As List(Of IImageProcessingLibrary), originalImagePath As String)
            Me.ProcessingChain = processingChain
            Me.OriginalImagePath = originalImagePath
            Me.IxProcessingChain = -1

            FallbackToNextProcessingLibrary()
        End Sub

        Public Sub New(processingChain As List(Of IImageProcessingLibrary), originalImageData As Byte())
            Me.ProcessingChain = processingChain
            Me.OriginalImageData = originalImageData
            Me.IxProcessingChain = -1

            FallbackToNextProcessingLibrary()
        End Sub

        Public Sub ScaleByHeight(newHeight As Integer) Implements IImage.ScaleByHeight
            Try
                CurrentImage.ScaleByHeight(newHeight)
            Catch ex As Exception
                FallbackToNextProcessingLibrary()
                ScaleByHeight(newHeight)
            End Try
        End Sub

        Public Sub ScaleByWidth(newWidth As Integer) Implements IImage.ScaleByWidth
            Try
                CurrentImage.ScaleByWidth(newWidth)
            Catch ex As Exception
                FallbackToNextProcessingLibrary()
                ScaleByWidth(newWidth)
            End Try
        End Sub

        Public Sub Write(target As Stream) Implements IImage.Write
            CurrentImage.Write(target)
        End Sub

        Public Sub Write(filePath As String) Implements IImage.Write
            CurrentImage.Write(filePath)
        End Sub

        Public Function Clone() As IImage Implements IImage.Clone
            If Not String.IsNullOrEmpty(OriginalImagePath) Then
                Return New FallbackChainImage(ProcessingChain, OriginalImagePath)
            Else
                Return New FallbackChainImage(ProcessingChain, OriginalImageData)
            End If
        End Function

        Private Sub FallbackToNextProcessingLibrary()
            If CurrentImage IsNot Nothing Then
                CurrentImage.Dispose()
            End If

            IxProcessingChain += 1
            If ProcessingChain.Count > IxProcessingChain Then
                Dim nextLibrary As IImageProcessingLibrary = ProcessingChain(IxProcessingChain)
                If OriginalImageData IsNot Nothing Then
                    CurrentImage = nextLibrary.LoadImage(OriginalImageData)
                Else
                    CurrentImage = nextLibrary.LoadImage(OriginalImagePath)
                End If

            Else
                Throw New NoFallbackLibraryFoundException()
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            If CurrentImage IsNot Nothing Then
                CurrentImage.Dispose()
            End If
        End Sub
    End Class
End Namespace