Imports System.IO
Imports ImageMagick

Namespace ImageProcessing
    Public Class MagickNetImageWrapper
        Implements IImage

        Private Property SourceImage As IMagickImage

        Public Sub New(sourceImage As IMagickImage)
            Me.SourceImage = sourceImage
        End Sub

        Public Function Clone() As IImage Implements IImage.Clone
            Return New MagickNetImageWrapper(SourceImage.Clone())
        End Function

        Public Sub ScaleByHeight(newHeight As Integer) Implements IImage.ScaleByHeight
            SourceImage.Resize(0, newHeight)
        End Sub

        Public Sub ScaleByWidth(newWidth As Integer) Implements IImage.ScaleByWidth
            SourceImage.Resize(0, newWidth)
        End Sub

        Public Sub Write(target As Stream) Implements IImage.Write
            SourceImage.Write(target)
        End Sub

        Public Sub Write(filePath As String) Implements IImage.Write
            Using targetStream As FileStream = File.OpenWrite(filePath)
                Write(targetStream)
            End Using
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            SourceImage.Dispose()
        End Sub
    End Class
End Namespace