Imports AcePump.SyncForQuickBooks.Qb.Models

Namespace Qb

    ''' <remarks>Consider refactoring to use AcePumpIntegration.QbContext like is used for adding / modifying invoices.</remarks>

    Public Class QbCacheService
        Public Shared Property Instance As New Lazy(Of QbCacheService)(Function() New QbCacheService)

        Private Sub New()
        End Sub

        Private Shared Property PartsCache As IEnumerable(Of QbItemModel)
        Private Shared Property CustomersCache As IEnumerable(Of QbCustomerModel)

        Public Shared Function GetAllQbParts() As IEnumerable(Of QbItemModel)
            If (PartsCache Is Nothing) Then
                PartsCache = ReadPartsFromQbfc()
            End If

            Return PartsCache
        End Function

        Private Shared Function ReadPartsFromQbfc() As IEnumerable(Of QbItemModel)
            Return QbPartIDManager.GetAllQbParts().AsEnumerable()
        End Function

        Public Shared Function GetAllQbCustomers() As IEnumerable(Of QbCustomerModel)
            If (CustomersCache Is Nothing) Then
                CustomersCache = ReadCustomersFromQbfc()
            End If

            Return CustomersCache
        End Function

        Private Shared Function ReadCustomersFromQbfc() As IEnumerable(Of QbCustomerModel)
            Return QbCustomerIDManager.GetAllQbCustomers().AsEnumerable()
        End Function
    End Class
End Namespace