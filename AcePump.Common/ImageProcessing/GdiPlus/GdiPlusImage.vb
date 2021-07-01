Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO

Namespace ImageProcessing
    Public Class GdiPlusImage
        Implements IImage

        Private Property SourceImage As Bitmap

        Public Sub New(sourceImage As Bitmap)
            Me.SourceImage = sourceImage
        End Sub

        Public Sub ScaleByHeight(newHeight As Integer) Implements IImage.ScaleByHeight
            Dim whRatio = (CType(SourceImage.Width, Double) / SourceImage.Height)
            Dim newWidth = CType(Math.Floor(whRatio * newHeight), Integer)

            Scale(newWidth, newHeight)
        End Sub

        Public Sub ScaleByWidth(newWidth As Integer) Implements IImage.ScaleByWidth
            Dim hwRatio = (CType(SourceImage.Height, Double) / SourceImage.Width)
            Dim newHeight = CType(Math.Floor(hwRatio * newWidth), Integer)

            Scale(newWidth, newHeight)
        End Sub

        Private Sub Scale(newWidth As Integer, newHeight As Integer)
            Dim canvas = New Bitmap(newWidth, newHeight)

            Dim gc As Graphics = Graphics.FromImage(canvas)
            gc.DrawImage(
                SourceImage,
                New Rectangle(0, 0, newWidth, newHeight),
                New Rectangle(0, 0, SourceImage.Width, SourceImage.Height),
                GraphicsUnit.Pixel
            )

            SourceImage.Dispose()
            SourceImage = canvas
        End Sub

        Public Sub Write(target As Stream) Implements IImage.Write
            SourceImage.Save(target, ImageFormat.Jpeg)
        End Sub

        Public Sub Write(filePath As String) Implements IImage.Write
            Using fileStream As FileStream = File.OpenWrite(filePath)
                Write(fileStream)
            End Using
        End Sub

        Public Function Clone() As IImage Implements IImage.Clone
            Return SourceImage.Clone()
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            SourceImage.Dispose()
        End Sub
    End Class
End Namespace