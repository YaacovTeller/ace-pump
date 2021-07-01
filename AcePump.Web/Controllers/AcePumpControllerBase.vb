Imports Yesod.Mvc
Imports AcePump.Domain.DataSource
Imports Newtonsoft.Json
Imports AcePump.Domain
Imports AcePump.Rdlc.Builder
Imports AcePump.Rdlc.Pdf


Namespace Controllers
    Public MustInherit Class AcePumpControllerBase
        Inherits ControllerBase(Of AcePumpContext)

        Public Overrides Property DataSource As AcePumpContext
            Get
                If _DataSource Is Nothing Then
                    _DataSource = DataSourceFactory.GetAcePumpDataSource()
                End If

                Return _DataSource
            End Get
            Set(value As AcePumpContext)
                _DataSource = value
            End Set
        End Property

        Public Sub New(dataSource As AcePumpContext)
            _DataSource = dataSource
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Protected Overridable Overloads Function Json(ByVal model As Object, ByVal modelState As ModelStateDictionary) As JsonResult
            Dim errors = modelState _
                            .Where(Function(x) x.Value.Errors.Any()) _
                            .ToDictionary(
                                Function(x) x.Key,
                                Function(x) x.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()
                            )

            Return Json(New With {
                            .Model = model,
                            .Errors = errors,
                            .Success = Not errors.Any()
                        })
        End Function

        Protected Function GetPdfReportAsActionResult(entityId As Integer, reportDefinitionType As Type, exceptionActionMethod As String)
            Try
                Dim reportDefinition = Activator.CreateInstance(reportDefinitionType, DataSource, entityId)

                Dim builder As New RdlcBuilder
                Dim strmPdf = builder.LoadReportDefinition(reportDefinition).CreatePdfStream()
                Return File(strmPdf, "application/pdf", builder.SaveAsName)
            Catch e As ArgumentException
                Return RedirectToAction(exceptionActionMethod)
            End Try
        End Function

        Protected Overrides Function Json(data As Object, contentType As String, contentEncoding As Encoding, behavior As JsonRequestBehavior) As JsonResult
            Return New JsonNetResult() With {
                .Data = data,
                .ContentType = contentType,
                .ContentEncoding = contentEncoding,
                .JsonRequestBehavior = behavior
            }
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If _DataSource IsNot Nothing Then
                    _DataSource.Dispose()
                End If
            End If

            MyBase.Dispose(disposing)
        End Sub

        Private Class JsonNetResult
            Inherits JsonResult

            Public Overrides Sub ExecuteResult(context As ControllerContext)
                If context Is Nothing Then
                    Throw New ArgumentNullException("context")
                End If

                Dim response As HttpResponseBase = context.HttpContext.Response
                response.ContentType = If(String.IsNullOrEmpty(ContentType),
                                          "application/json",
                                          ContentType)

                If ContentEncoding IsNot Nothing Then
                    response.ContentEncoding = ContentEncoding
                End If

                Dim serializedData As String = JsonConvert.SerializeObject(Data)
                response.Write(serializedData)
            End Sub
        End Class
    End Class
End Namespace