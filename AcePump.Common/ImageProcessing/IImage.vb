Imports System.IO

Namespace ImageProcessing
    Public Interface IImage
        Inherits IDisposable

        Sub ScaleByHeight(newHeight As Integer)
        Sub ScaleByWidth(newWidth As Integer)

        Sub Write(target As Stream)
        Sub Write(filePath As String)

        Function Clone() As IImage
    End Interface
End Namespace
