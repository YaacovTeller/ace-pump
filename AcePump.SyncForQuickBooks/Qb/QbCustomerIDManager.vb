'Imports Interop.QBFC13
Imports Interop.QBFC15
Imports System.Threading
Imports AcePump.SyncForQuickBooks.Qb.Models
Imports AcePump.SyncForQuickBooks.PtpApi.Models

Namespace Qb

    ''' <remarks>Consider refactoring to use AcePumpIntegration.QbContext like is used for adding / modifying invoices.</remarks>

    Public Class QbCustomerIDManager
        Public Shared Function GetQbCustomerIDLookup(deliverytickets As List(Of DeliveryTicketModel), token As CancellationToken) As List(Of QbCustomerModel)
            Dim customerQbIDs As List(Of String) = deliverytickets.Where(Function(x) String.IsNullOrEmpty(x.CustomerQuickbooksID)) _
                                                                  .Select(Function(x) x.CustomerName) _
                                                                  .Distinct() _
                                                                  .ToList

            If customerQbIDs.Count > 0 Then
                token.ThrowIfCancellationRequested()

                Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                    Dim customerQueryRequest As ICustomerQuery = session.MessageSetRequest.AppendCustomerQueryRq()

                    token.ThrowIfCancellationRequested()

                    For Each name As String In customerQbIDs
                        customerQueryRequest.ORCustomerListQuery.FullNameList.Add(name)
                    Next

                    customerQueryRequest.IncludeRetElementList.Add("ListID")
                    customerQueryRequest.IncludeRetElementList.Add("Name")
                    Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)

                    token.ThrowIfCancellationRequested()
                    Return ProcessCustomerQueryResponse(responseMsgSet)
                End Using
            Else
                Return Nothing
            End If
        End Function

        Public Shared Function ProcessCustomerQueryResponse(responseMsgSet As IMsgSetResponse) As List(Of QbCustomerModel)
            Dim lookupList As New List(Of QbCustomerModel)

            If responseMsgSet IsNot Nothing Then
                Dim responseList As IResponseList
                responseList = responseMsgSet.ResponseList
                If responseList IsNot Nothing Then
                    For j = 0 To responseList.Count - 1
                        Dim response As IResponse = responseList.GetAt(j)
                        If (response.StatusCode >= 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtCustomerQueryRs) Then
                                    Dim customerRetList As ICustomerRetList = CType(response.Detail, ICustomerRetList)

                                    Dim customerRet As ICustomerRet
                                    Dim listID As String
                                    Dim name As String
                                    For i = 0 To customerRetList.Count - 1
                                        customerRet = customerRetList.GetAt(i)
                                        listID = customerRet.ListID.GetValue()
                                        name = customerRet.Name.GetValue()

                                        lookupList.Add(New QbCustomerModel With {.ListID = listID,
                                                                                 .Name = name})
                                    Next
                                End If
                            End If
                        End If
                    Next j
                End If
            End If

            Return lookupList
        End Function

        Public Shared Function GetAllQbCustomers() As List(Of QbCustomerModel)
            Dim lookupList As List(Of QbCustomerModel)
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim customerQueryRequest As ICustomerQuery = session.MessageSetRequest.AppendCustomerQueryRq()

                customerQueryRequest.IncludeRetElementList.Add("ListID")
                customerQueryRequest.IncludeRetElementList.Add("Name")

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                lookupList = ProcessCustomerQueryResponse(responseMsgSet)
            End Using

            Return lookupList
        End Function

        Public Shared Function CreateNewQbCustomer(customerName As String) As QbCreateResponse
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim customerAdd As ICustomerAdd = session.MessageSetRequest.AppendCustomerAddRq()
                customerAdd.Name.SetValue(customerName)

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)
                Return ProcessCustomerAddResponse(responseMsgSet)
            End Using
        End Function

        Public Shared Function ProcessCustomerAddResponse(responseMsgSet As IMsgSetResponse) As QbCreateResponse
            If responseMsgSet IsNot Nothing Then
                Dim responseList As IResponseList
                responseList = responseMsgSet.ResponseList
                If responseList IsNot Nothing Then
                    For j = 0 To responseList.Count - 1
                        Dim response As IResponse = responseList.GetAt(j)
                        If (response.StatusCode = 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtCustomerAddRs) Then
                                    Dim customerAdd As ICustomerRet = CType(response.Detail, ICustomerRet)
                                    Return New QbCreateResponse With {.StatusMessage = "", .CreatedID = customerAdd.ListID.GetValue()}
                                End If
                            End If
                        Else
                            Return New QbCreateResponse With {.StatusCode = response.StatusCode, .StatusMessage = response.StatusMessage, .CreatedID = ""}
                        End If
                    Next j
                End If
            End If

            Return New QbCreateResponse With {.StatusMessage = "General failure to create customer."}
        End Function
    End Class
End Namespace