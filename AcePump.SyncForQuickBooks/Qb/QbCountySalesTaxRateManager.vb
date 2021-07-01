Imports Interop.QBFC13

Namespace Qb

    ''' <remarks>Consider refactoring to use AcePumpIntegration.QbContext like is used for adding / modifying invoices.</remarks>

    Public Class QbCountySalesTaxRateManager
        Public Shared Function FindCountySalesTaxRateInQb(nameToFind As String) As String
            Using session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Dim itemQueryRequest As IItemQuery = session.MessageSetRequest.AppendItemQueryRq()

                itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(nameToFind)
                itemQueryRequest.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains)
                itemQueryRequest.IncludeRetElementList.Add("ListID")
                itemQueryRequest.IncludeRetElementList.Add("Name")

                Dim responseMsgSet As IMsgSetResponse = session.SessionManager.DoRequests(session.MessageSetRequest)

                Dim countyQbId As String = Nothing

                If responseMsgSet IsNot Nothing Then
                    Dim responseList As IResponseList
                    responseList = responseMsgSet.ResponseList
                    If responseList IsNot Nothing Then
                        For j = 0 To responseList.Count - 1
                            Dim response As IResponse = responseList.GetAt(j)
                            If (response.StatusCode >= 0) Then
                                If (Not response.Detail Is Nothing) Then
                                    Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                    If (responseType = ENResponseType.rtItemQueryRs) Then
                                        Dim itemRetList As IORItemRetList = CType(response.Detail, IORItemRetList)

                                        Dim itemRet As IORItemRet
                                        Dim listID As String = ""
                                        Dim name As String = ""

                                        For k = 0 To itemRetList.Count - 1
                                            itemRet = itemRetList.GetAt(k)
                                            If (itemRet.ItemSalesTaxRet IsNot Nothing) Then
                                                listID = itemRet.ItemSalesTaxRet.ListID.GetValue()
                                                name = itemRet.ItemSalesTaxRet.Name.GetValue()
                                            End If
                                            If Not String.IsNullOrEmpty(name) Then
                                                If name = nameToFind Then
                                                    If Not String.IsNullOrEmpty(listID) Then
                                                        countyQbId = listID
                                                        k = itemRetList.Count
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                End If

                            End If
                        Next j
                    End If
                End If
                Return countyQbId
            End Using
        End Function
    End Class
End Namespace