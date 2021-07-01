Imports AcePump.SyncForQuickBooks.PtpApi.Models

Namespace UI.Validation
    Public Class QbDeliveryTicketValidator
        Public Shared Function ValidateTicket(ByVal deliveryTicket As DeliveryTicketModel) As Dictionary(Of DeliveryTicketValidationFailureReason, String)
            Dim results As New Dictionary(Of DeliveryTicketValidationFailureReason, String)

            If deliveryTicket.LineItems.Count < 1 Then
                results.Add(DeliveryTicketValidationFailureReason.NoLineItems, "Delivery ticket must have at least one line item.")
            End If

            If deliveryTicket.CustomerID Is Nothing Then
                results.Add(DeliveryTicketValidationFailureReason.MissingPtpCustomerID, "Delivery ticket must have a customer specified.")
            Else
                If String.IsNullOrWhiteSpace(deliveryTicket.CustomerQuickbooksID) Then
                    results.Add(DeliveryTicketValidationFailureReason.MissingQbCustomerID, "Could not find Qb customer information for this ticket.")
                End If

                If String.IsNullOrWhiteSpace(deliveryTicket.CountySalesTaxRateQuickbooksID) Then
                    Dim addedInfo As String
                    If Not String.IsNullOrWhiteSpace(deliveryTicket.CountySalesTaxRateName) Then
                        addedInfo = "Could not find: " & deliveryTicket.CountySalesTaxRateName & " in Quickbooks. Please make sure name online matches the Quickbooks name exactly."
                    End If
                    results.Add(DeliveryTicketValidationFailureReason.MissingCountySalesTaxRateQbID, "Could not find Quickbooks county sales tax rate information for this ticket." & addedInfo)
                End If

                If String.IsNullOrWhiteSpace(deliveryTicket.CountySalesTaxRateName) Then
                    results.Add(DeliveryTicketValidationFailureReason.MissingCountySalesTaxRateName, "No county sales tax rate is set for this ticket. Could not look it up in Quickbooks.")
                End If
            End If

            Return results
        End Function

        Public Shared Function ValidateLineItem(lineItem As LineItemModel) As Dictionary(Of LineItemValidationFailureReason, String)
            Dim results As New Dictionary(Of LineItemValidationFailureReason, String)

            If String.IsNullOrEmpty(lineItem.PartQuickbooksID) Then
                results.Add(LineItemValidationFailureReason.MissingQbPartID, "Could not find part number for this line item.")
            End If

            Return results
        End Function
    End Class
End Namespace