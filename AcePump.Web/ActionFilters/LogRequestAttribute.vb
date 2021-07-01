Imports AcePump.Common
Imports AcePump.Domain
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.Models

Namespace ActionFilters
    Public Class LogRequestAttribute
        Inherits ActionFilterAttribute

        Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
            If AcePumpEnvironment.Environment.Configuration.Logging.LogAllRequests Then
                Using db As AcePumpContext = DataSourceFactory.GetAcePumpDataSource()
                    Dim request = New Log_HttpRequest With {
                        .Environment = "mvc",
                        .HttpMethod = filterContext.HttpContext.Request.HttpMethod,
                        .Path = filterContext.HttpContext.Request.Path,
                        .RequestTime = DateTime.Now,
                        .LoggedInUsername = If(filterContext.HttpContext.User.Identity.IsAuthenticated, filterContext.HttpContext.User.Identity.Name, ""),
                        .Parameters = New List(Of Log_HttpRequestParam)
                    }

                    For Each qstringKey As String In filterContext.HttpContext.Request.QueryString
                        If qstringKey.Equals("password", StringComparison.InvariantCultureIgnoreCase) Then Continue For

                        For Each qstringValue As String In filterContext.HttpContext.Request.QueryString.GetValues(qstringKey)
                            request.Parameters.Add(New Log_HttpRequestParam With {
                                .ParamType = "qstring",
                                .ParamName = qstringKey,
                                .ParamValue = qstringValue
                            })
                        Next
                    Next

                    For Each formKey As String In filterContext.HttpContext.Request.Form
                        If formKey.Equals("password", StringComparison.InvariantCultureIgnoreCase) Then Continue For

                        For Each formValue As String In filterContext.HttpContext.Request.Form.GetValues(formKey)
                            request.Parameters.Add(New Log_HttpRequestParam With {
                                .ParamType = "form",
                                .ParamName = formKey,
                                .ParamValue = formValue
                            })
                        Next
                    Next

                    For Each routeDataEntry As KeyValuePair(Of String, Object) In filterContext.RequestContext.RouteData.Values
                        request.Parameters.Add(New Log_HttpRequestParam With {
                            .ParamType = "routedata",
                            .ParamName = routeDataEntry.Key,
                            .ParamValue = routeDataEntry.Value
                        })
                    Next

                    db.Log_HttpRequests.Add(request)
                    db.SaveChanges()
                End Using
            End If
        End Sub
    End Class
End Namespace