Imports System.Drawing
Imports System.IO

Namespace ImageProcessing
    Public Class GdiPlusImageProcessingLibrary
        Implements IImageProcessingLibrary

        Public Function LoadImage(path As String) As IImage Implements IImageProcessingLibrary.LoadImage
            Return New GdiPlusImage(New Bitmap(path))
        End Function

        Public Function LoadImage(bytes() As Byte) As IImage Implements IImageProcessingLibrary.LoadImage
            Dim stream As New MemoryStream(bytes)

            Return New GdiPlusImage(Bitmap.FromStream(stream))
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
        End Sub
    End Class
End Namespace