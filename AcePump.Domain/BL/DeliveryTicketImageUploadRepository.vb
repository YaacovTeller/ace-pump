Imports System.IO
Imports AcePump.Common
Imports AcePump.Common.ImageProcessing
Imports AcePump.Common.Storage
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models

Namespace BL
    Public Class DeliveryTicketImageUploadRepository
        Implements IDisposable

        Private Property AcePumpContext As AcePumpContext
        Private Property ImageProcessingLibrary As IImageProcessingLibrary
        Private Property StorageProvider As IStorageProvider

        Private ReadOnly Property DtImageStorageContainer As IStorageContainer
            Get
                Return StorageProvider.GetContainer(AcePumpEnvironment.Environment.Configuration.Storage.DtImageContainerName)
            End Get
        End Property


        Public Sub New(acePumpContext As AcePumpContext, imageProcessingLibrary As IImageProcessingLibrary, storageProvider As IStorageProvider)
            Me.AcePumpContext = acePumpContext
            Me.ImageProcessingLibrary = imageProcessingLibrary
            Me.StorageProvider = storageProvider
        End Sub

        Public Function AddUpload(imageData As Byte(), contentType As String, uploadedBy As String, deliveryTicket As DeliveryTicket) As DeliveryTicketImageUpload
            Using image As IImage = ImageProcessingLibrary.LoadImage(imageData)
                Dim largeImageDiskName As String = DtImageStorageContainer.GetRandomFilename() & ".jpg"
                Dim largeImageStorageFile = dtImageStorageContainer.GetFile(largeImageDiskName)
                Using lgImgWriteStream = largeImageStorageFile.OpenWrite()
                    Dim lgImage = image.Clone()
                    lgImage.ScaleByWidth(800)
                    lgImage.Write(lgImgWriteStream)
                    lgImage.Dispose()
                End Using

                Dim smallImageDiskName As String = dtImageStorageContainer.GetRandomFilename() & ".jpg"
                Dim smallImageStorageFile = dtImageStorageContainer.GetFile(smallImageDiskName)
                Using smImgWriteStream = smallImageStorageFile.OpenWrite()
                    Dim smImage = image.Clone()
                    smImage.ScaleByWidth(150)
                    smImage.Write(smImgWriteStream)
                    smImage.Dispose()
                End Using

                Dim dbImage As New DeliveryTicketImageUpload() With {
                    .LargeImageName = largeImageDiskName,
                    .SmallImageName = smallImageDiskName,
                    .MimeType = contentType,
                    .DeliveryTicket = deliveryTicket,
                    .UploadedBy = uploadedBy,
                    .UploadedOn = Today
                }
                AcePumpContext.DeliveryTicketImageUploads.Add(dbImage)

                Return dbImage
            End Using
        End Function

        Public Sub DeleteUpload(uploadId As Integer)
            Dim upload As DeliveryTicketImageUpload = AcePumpContext.DeliveryTicketImageUploads.Find(uploadId)
            Dim smImageStorageFile = DtImageStorageContainer.GetFile(upload.SmallImageName)
            smImageStorageFile.Delete()

            Dim lgImageStorageFile = DtImageStorageContainer.GetFile(upload.LargeImageName)
            lgImageStorageFile.Delete()

            AcePumpContext.DeliveryTicketImageUploads.Remove(upload)
            AcePumpContext.SaveChanges()
        End Sub

        Public Function GetSmallImage(uploadId As Integer) As StreamableDownload
            Dim upload = AcePumpContext.DeliveryTicketImageUploads.Find(uploadId)
            If upload IsNot Nothing Then
                Dim smallImageStorageFile = DtImageStorageContainer.GetFile(upload.SmallImageName)

                If smallImageStorageFile IsNot Nothing Then
                    Return New StreamableDownload With {
                        .ReadStream = smallImageStorageFile.OpenRead(),
                        .MimeType = upload.MimeType
                    }
                End If
            End If

            Return Nothing
        End Function

        Public Function GetLargeImage(uploadId As Integer) As StreamableDownload
            Dim upload = AcePumpContext.DeliveryTicketImageUploads.Find(uploadId)
            If upload IsNot Nothing Then
                Dim largeImageStorageFile = DtImageStorageContainer.GetFile(upload.LargeImageName)

                If largeImageStorageFile IsNot Nothing Then
                    Return New StreamableDownload With {
                        .ReadStream = largeImageStorageFile.OpenRead(),
                        .MimeType = upload.MimeType
                    }
                End If
            End If

            Return Nothing
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            ImageProcessingLibrary.Dispose()
        End Sub
    End Class
End Namespace