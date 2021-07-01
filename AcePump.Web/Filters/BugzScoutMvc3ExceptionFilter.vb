Imports System.Web.Mvc
Imports AcePump.Common.BugzScout
Imports AcePump.Common
Imports System.Runtime.CompilerServices

Namespace Filters
    Public Class BugzScoutMvc3ExceptionFilter
        Implements IExceptionFilter

        Public Sub OnException(filterContext As ExceptionContext) Implements IExceptionFilter.OnException
            If AcePumpEnvironment.Environment.Configuration.Logging.LogErrorsToFogBugz Then
                Dim client As New BugzScoutClient("https://sorissoftware.fogbugz.com")
                client.Submit(
                    description:=FormatExceptionDescription(filterContext),
                    additionalInformation:=FormatAdditionalInformation(filterContext),
                    username:="BugzScout",
                    project:="Ace Pump",
                    area:="Misc"
                )
            End If
        End Sub

        Private Function FormatExceptionDescription(filterContext As ExceptionContext) As String
            Return String.Format(
                "Ace Pump-{0}: MVC:  {1} {2}: {3}",
                AcePumpEnvironment.Environment.Configuration.Name,
                filterContext.HttpContext.Request.HttpMethod,
                filterContext.HttpContext.Request.Path,
                filterContext.Exception.Message
            )
        End Function

        Private Function FormatAdditionalInformation(filterContext As ExceptionContext) As String
            Dim sb As New StringBuilder()

            If (filterContext.HttpContext.Request.QueryString.Count > 0) Then
                sb.AppendLine("Querystring --")
                sb.AppendNvCollection(filterContext.HttpContext.Request.QueryString)
                sb.AppendLine()
            End If

            If (filterContext.HttpContext.Request.Form.Count > 0) Then
                sb.AppendLine("Form data --")
                sb.AppendNvCollection(filterContext.HttpContext.Request.Form)
                sb.AppendLine()
            End If

            If (filterContext.RequestContext.RouteData.Values.Count > 0) Then
                sb.AppendLine("Route data --")
                For Each pair In filterContext.RequestContext.RouteData.Values
                    sb.AppendLine(pair.Key & " = " & pair.Value)
                Next
                sb.AppendLine()
            End If

            If (filterContext.HttpContext.User.Identity.IsAuthenticated) Then
                sb.AppendLine("Logged in user: " + filterContext.HttpContext.User.Identity.Name)
                sb.AppendLine()
            End If

            sb.AppendLine("Exception message(s) --")
            Dim current As Exception = filterContext.Exception
            Dim cnt As Integer = 1
            While current IsNot Nothing
                sb.AppendLine($"{cnt}: {current.GetType().Name}: {current.Message}")

                cnt += 1
                current = current.InnerException
            End While
            sb.AppendLine()

            sb.AppendLine("Stack trace --")
            sb.AppendLine(filterContext.Exception.StackTrace)

            Return sb.ToString()
        End Function
    End Class

    Friend Module StringBuildExtensions
        <Extension()>
        Public Sub AppendNvCollection(sb As StringBuilder, col As NameValueCollection)
            For ixCollectionPos = 0 To col.Count - 1
                Dim key As String = col.GetKey(ixCollectionPos)
                Dim value As String = col.Get(key)

                sb.AppendLine(key & " = " & value)
            Next
        End Sub
    End Module
End Namespace
