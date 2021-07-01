Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports System.Threading
Imports AcePump.SyncForQuickBooks.UI
Imports System.Text
Imports AcePump.Common
Imports AcePump.SyncForQuickBooks.PtpApi.Models

Namespace PtpApi
    Public Class ApiConnector
        Public Shared ApiToken As ApiTokenResponse
        Private Shared _receivedTickets As List(Of DeliveryTicketModel)
        Private Shared _readyTicketsCount As Integer

        Public Shared Function GetToken(username As String, password As String) As ApiTokenResponse
            Dim encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", username, password)))

            Using client As New HttpClient
                client.BaseAddress = New Uri(AcePumpEnvironment.Environment.Configuration.PtpApi.UriV1)
                client.DefaultRequestHeaders.Accept.Clear()
                client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Basic", encoded)

                Dim tokenResponse As New ApiTokenResponse

                Dim task = client.PostAsync("api/account/token", Nothing) _
                                 .ContinueWith(Sub(requestTask As Task(Of HttpResponseMessage))
                                                   Dim response As HttpResponseMessage = requestTask.Result
                                                   If response.IsSuccessStatusCode Then
                                                       tokenResponse.ResponseType = ApiTokenResponseType.SuccessfulLogin
                                                       tokenResponse.Token = New Guid(response.Headers.GetValues("Token").FirstOrDefault())
                                                   ElseIf response.StatusCode = Net.HttpStatusCode.Unauthorized Then
                                                       tokenResponse.ResponseType = ApiTokenResponseType.Unauthorized
                                                       tokenResponse.Token = Nothing
                                                       tokenResponse.Message = response.ToString
                                                   Else
                                                       tokenResponse.ResponseType = ApiTokenResponseType.HttpException
                                                       tokenResponse.Token = Nothing
                                                       tokenResponse.Message = response.ToString
                                                   End If
                                               End Sub)
                task.Wait()
                Return tokenResponse
            End Using
        End Function

        Public Shared Function DownloadDeliverytickets(reporter As UIProgressReporter, token As CancellationToken) As List(Of DeliveryTicketModel)
            Using client As New HttpClient
                SetupConnection(client, reporter, token)
                DownloadTicketsFromApi(client, reporter, token)
            End Using

            Return _receivedTickets
        End Function

        Public Shared Function CountReadyDeliverytickets(reporter As UIProgressReporter, token As CancellationToken) As Integer
            Using client As New HttpClient
                SetupConnection(client, reporter, token)
                CountReadyTicketsFromApi(client, reporter, token)
            End Using

            Return _readyTicketsCount
        End Function

        Private Shared Function SetupConnection(client As HttpClient, reporter As UIProgressReporter, token As CancellationToken) As HttpClient

            reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.ConnectingToApi})
            token.ThrowIfCancellationRequested()
            client.BaseAddress = New Uri(AcePumpEnvironment.Environment.Configuration.PtpApi.UriV1)
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            client.DefaultRequestHeaders.Add("Token", ApiToken.Token.ToString)

            Return client
        End Function

        Private Shared Sub DownloadTicketsFromApi(client As HttpClient, reporter As UIProgressReporter, token As CancellationToken)
            token.ThrowIfCancellationRequested()
            Dim task = client.GetAsync("api/quickbooks/GetLatestDeliveryTickets") _
                             .ContinueWith(Sub(requestTask As Task(Of HttpResponseMessage))
                                               reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.DownloadingTicketsFromApi})
                                               token.ThrowIfCancellationRequested()

                                               Dim response As HttpResponseMessage = requestTask.Result
                                               If response.IsSuccessStatusCode Then
                                                   ProcessDownloadTicketResponse(response, reporter, token)
                                               Else
                                                   Dim failed As String = response.Content.ReadAsStringAsync().Result
                                                   Throw New Exception(failed)
                                               End If
                                           End Sub,
                                           token,
                                           TaskContinuationOptions.NotOnFaulted,
                                           TaskScheduler.Default)
            task.Wait(token)
        End Sub

        Private Shared Sub CountReadyTicketsFromApi(client As HttpClient, reporter As UIProgressReporter, token As CancellationToken)
            token.ThrowIfCancellationRequested()
            Dim task = client.GetAsync("api/quickbooks/GetReadyTicketsCount") _
                             .ContinueWith(Sub(requestTask As Task(Of HttpResponseMessage))
                                               reporter.UpdateProgress(New UIProgressDto With {.Stage = UIProgressStage.ReadingTicketsFromApi})
                                               token.ThrowIfCancellationRequested()

                                               Dim response As HttpResponseMessage = requestTask.Result
                                               If response.IsSuccessStatusCode Then
                                                   ProcessCountReadyTicketResponse(response, reporter, token)
                                               Else
                                                   Dim failed As String = response.Content.ReadAsStringAsync().Result
                                                   Throw New Exception(failed)
                                               End If
                                           End Sub,
                                           token,
                                           TaskContinuationOptions.NotOnFaulted,
                                           TaskScheduler.Default)
            task.Wait(token)
        End Sub

        Private Shared Sub ProcessDownloadTicketResponse(response As HttpResponseMessage, reporter As UIProgressReporter, token As CancellationToken)
            Dim readTask = response.Content.ReadAsAsync(Of DeliveryTicketModel())()
            reporter.ReportPredefinedProgressAsync(New UIProgressDto With {.Stage = UIProgressStage.ReadingTicketsFromApi})
            token.ThrowIfCancellationRequested()
            readTask.Wait()
            _receivedTickets = readTask.Result.ToList
        End Sub

        Private Shared Sub ProcessCountReadyTicketResponse(response As HttpResponseMessage, reporter As UIProgressReporter, token As CancellationToken)
            Dim readTask = response.Content.ReadAsAsync(Of Integer)()
            reporter.ReportPredefinedProgressAsync(New UIProgressDto With {.Stage = UIProgressStage.ReadingTicketsFromApi})
            token.ThrowIfCancellationRequested()
            readTask.Wait()
            _readyTicketsCount = readTask.Result
        End Sub

        Public Shared Function PostToQbApi(controllerMethod As String, objectToPost As Object, reporter As UIProgressReporter, token As CancellationToken) As HttpResponseMessage
            Dim response As HttpResponseMessage = Nothing

            Using client As New HttpClient
                SetupConnection(client, reporter, token)
                Dim task = client.PostAsJsonAsync("api/quickbooks/" & controllerMethod, objectToPost) _
                                 .ContinueWith(Sub(requestTask As Task(Of HttpResponseMessage))
                                                   response = requestTask.Result
                                                   If Not response.IsSuccessStatusCode Then
                                                       Dim failed As String = response.Content.ReadAsStringAsync().Result
                                                       Throw New Exception(failed)
                                                   End If
                                               End Sub,
                                               New CancellationToken,
                                               TaskContinuationOptions.NotOnFaulted,
                                               TaskScheduler.Default)

                task.Wait(New CancellationToken)
            End Using

            Return response
        End Function
    End Class
End Namespace