Imports AcePump.Common.BugzScout
Imports AcePump.Common
Imports System.Web.Http.Filters
Imports System.Security.Principal

Namespace Api.ActionFilters
    Public Class BugzScoutWebApiExceptionAttribute
        Inherits ExceptionFilterAttribute
        
        Public Overrides Sub OnException(actionExecutedContext As HttpActionExecutedContext)
            If Not AcePumpEnvironment.Environment.Configuration.Logging.LogErrorsToFogBugz Then Return

            Dim bugzScoutClient As New BugzScoutClient("https://sorissoftware.fogbugz.com")
            bugzScoutClient.Submit(
                description:=FormatExceptionDescription(actionExecutedContext),
                additionalInformation:=FormatAdditionalInformation(actionExecutedContext),
                username:="BugzScout",
                project:="Ace Pump",
                area:="Misc"
            )
        End Sub

        Private Function FormatExceptionDescription(context As HttpActionExecutedContext) As String
            Return String.Format(
                "{0}: {1} {2}: {3}",
                AcePumpEnvironment.Environment.Configuration.Logging.LogEntryPrefix,
                context.Request.Method,
                context.Request.RequestUri.AbsolutePath,
                context.Exception.Message
            )
        End Function

        Private Function FormatAdditionalInformation(context As HttpActionExecutedContext) As String
            Dim sb = New StringBuilder()

            If Not String.IsNullOrEmpty(context.Request.RequestUri.Query) Then
                sb.AppendLine("Querystring --")
                sb.AppendLine(context.Request.RequestUri.Query)
                sb.AppendLine()
            End If

            Dim rawRequestBodyTask = context.Request.Content.ReadAsStringAsync()
            rawRequestBodyTask.Wait()
            Dim rawRequestBody = rawRequestBodyTask.Result
            If Not String.IsNullOrEmpty(rawRequestBody) Then
                sb.AppendLine("Raw request body --")
                sb.AppendLine(rawRequestBody)
                sb.AppendLine()
            End If

            If context.ActionContext.ControllerContext.RouteData.Values.Count > 0 Then
                sb.AppendLine("Route data --")
                For Each pair In context.ActionContext.ControllerContext.RouteData.Values
                    sb.AppendLine(pair.Key & " = " & pair.Value)
                Next
                sb.AppendLine()
            End If

            If context.Request.Properties.ContainsKey("MS_UserPrincipal") Then
                Dim user As IPrincipal = DirectCast(context.Request.Properties("MS_UserPrincipal"), IPrincipal)

                sb.AppendLine("Logged in user: " + user.Identity.Name)
                sb.AppendLine()
            End If


            sb.AppendLine("Exception message(s) --")
            Dim current As Exception = context.Exception
            Dim cnt As Integer = 1
            While current IsNot Nothing
                sb.AppendLine($"{cnt}: {current.GetType().Name}: {current.Message}")

                cnt += 1
                current = current.InnerException
            End While
            sb.AppendLine()

            sb.AppendLine("Stack trace --")
            sb.AppendLine(context.Exception.StackTrace)

            Return sb.ToString()
        End Function
    End Class
End Namespace