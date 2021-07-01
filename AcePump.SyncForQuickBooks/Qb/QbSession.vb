Imports Interop.QBFC13

Namespace Qb

    ''' <remarks>This is not the same QbSession as the one in AcePump.Qbpi. This class here is used for all code that was not refactored to use QbContext yet.</remarks>
    ''' 
    Public Class QbSession
        Implements IDisposable

        Private _sessionManager As QBSessionManager
        Private _messageSetRequest As IMsgSetRequest
        Private Property QbCompanyFile As String

        Public ReadOnly Property SessionManager As QBSessionManager
            Get
                If _sessionManager Is Nothing Then
                    CreateSession()
                End If
                Return _sessionManager
            End Get
        End Property

        Public ReadOnly Property MessageSetRequest As IMsgSetRequest
            Get
                If _messageSetRequest Is Nothing Then
                    CreateSession()
                End If
                Return _messageSetRequest
            End Get
        End Property

        Private Sub CreateSession()
            Const appID = ""
            _sessionManager = New QBSessionManager()

            Dim assemblyName As String = My.Application.Info.AssemblyName

            _sessionManager.OpenConnection(appID, assemblyName)
            _sessionManager.BeginSession(QbCompanyFile, ENOpenMode.omDontCare)

            _messageSetRequest = GetLatestMessageSetRequest(SessionManager)
            ClearMessagesetRequest()
        End Sub

        Public Sub New(qbCompanyFile As String)
            Me.QbCompanyFile = qbCompanyFile
        End Sub

        Public Sub ClearMessagesetRequest()
            MessageSetRequest.ClearRequests()
            MessageSetRequest.Attributes.OnError = ENRqOnError.roeContinue
        End Sub

        Private Sub ReleaseSession()
            If _sessionManager IsNot Nothing Then
                _sessionManager.EndSession()
                _sessionManager.CloseConnection()
                _messageSetRequest = Nothing
                _sessionManager = Nothing
            End If
        End Sub

        Public Function TestConnection(ByRef responseMessage As String) As Boolean
            ClearMessagesetRequest()

            Dim companyQuery As ICompanyQuery = MessageSetRequest.AppendCompanyQueryRq()
            companyQuery.IncludeRetElementList.Add("CompanyName")

            Dim responseMsgSet As IMsgSetResponse = SessionManager.DoRequests(MessageSetRequest)

            If responseMsgSet IsNot Nothing Then
                Dim responseList As IResponseList
                responseList = responseMsgSet.ResponseList
                If responseList IsNot Nothing Then
                    For j = 0 To responseList.Count - 1
                        Dim response As IResponse = responseList.GetAt(j)
                        If (response.StatusCode = 0) Then
                            If (Not response.Detail Is Nothing) Then
                                Dim responseType As ENResponseType = CType(response.Type.GetValue(), ENResponseType)
                                If (responseType = ENResponseType.rtCompanyQueryRs) Then
                                    Dim companyRet As ICompanyRet = CType(response.Detail, ICompanyRet)
                                    If (Not companyRet.CompanyName Is Nothing) Then
                                        responseMessage = "Connected Successfully to: " & companyRet.CompanyName.GetValue()
                                        Return True
                                    End If
                                End If
                            End If
                        Else
                            responseMessage = response.StatusMessage
                            Return False
                        End If
                    Next j
                End If
            End If
            responseMessage = "Could not connect, response from QuickBooks was NULL, unknown error."
            Return False
        End Function

        Private Function QBFCLatestVersion(SessionManager As QBSessionManager) As String

            Dim msgset As IMsgSetRequest
            'Use oldest version to ensure that we work with any QuickBooks (US)
            msgset = SessionManager.CreateMsgSetRequest("US", 1, 0)
            msgset.AppendHostQueryRq()
            Dim queryResponse As IMsgSetResponse
            queryResponse = SessionManager.DoRequests(msgset)
            Dim response As IResponse

            ' The response list contains only one response,
            ' which corresponds to our single HostQuery request
            response = queryResponse.ResponseList.GetAt(0)
            Dim hostResponse As IHostRet
            hostResponse = response.Detail
            Dim supportedVersions As IBSTRList
            supportedVersions = hostResponse.SupportedQBXMLVersionList

            Dim i As Long
            Dim vers As Double
            Dim lastVers As Double
            Dim latestSupportedVersion As Double
            lastVers = 0
            For i = 0 To supportedVersions.Count - 1
                vers = Val(supportedVersions.GetAt(i))
                If (vers > lastVers) Then
                    lastVers = vers
                    latestSupportedVersion = supportedVersions.GetAt(i)
                End If
            Next i
            Return latestSupportedVersion
        End Function

        Private Function GetLatestMessageSetRequest(sessionManager As QBSessionManager) As IMsgSetRequest
            Dim supportedVersion As String
            supportedVersion = Val(QBFCLatestVersion(sessionManager))

            If (supportedVersion >= 13.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 13, 0)
            ElseIf (supportedVersion >= 12.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 12, 0)
            ElseIf (supportedVersion >= 11.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 11, 0)
            ElseIf (supportedVersion >= 10.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 10, 0)
            ElseIf (supportedVersion >= 9.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 9, 0)
            ElseIf (supportedVersion >= 8.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 8, 0)
            ElseIf (supportedVersion >= 7.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 7, 0)
            ElseIf (supportedVersion >= 6.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 6, 0)
            ElseIf (supportedVersion >= 5.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 5, 0)
            ElseIf (supportedVersion >= 4.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 4, 0)
            ElseIf (supportedVersion >= 3.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 3, 0)
            ElseIf (supportedVersion >= 2.0#) Then
                Return sessionManager.CreateMsgSetRequest("US", 2, 0)
            ElseIf (supportedVersion = 1.1) Then
                Return sessionManager.CreateMsgSetRequest("US", 1, 1)
            Else
                MsgBox("You are apparently running QuickBooks 2002 Release 1, we strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements", vbExclamation)
                Return sessionManager.CreateMsgSetRequest("US", 1, 0)
            End If
        End Function
#Region "IDisposable Support"
        Private _disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me._disposedValue Then
                If disposing Then
                    ReleaseSession()
                End If
            End If
            Me._disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace