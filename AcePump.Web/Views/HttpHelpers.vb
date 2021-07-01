Imports System.Runtime.CompilerServices
Imports System.Security.Principal
Imports Kendo.Mvc.UI
Imports Kendo.Mvc

Namespace Views
    Public Module HttpHelpers
        <Extension()> _
        Public Function ActionKendoButton(Of TModel)(html As HtmlHelper(Of TModel), text As String, actionName As String, routeValues As Object) As MvcHtmlString
            Dim urlHelper As New UrlHelper(html.ViewContext.RequestContext)
            Dim href As String = urlHelper.Action(actionName, routeValues)

            Return html.GetKendoButtonHtml(text, href)
        End Function

        <Extension()> _
        Public Function ActionKendoButton(Of TModel)(html As HtmlHelper(Of TModel), text As String, actionName As String, controllerName As String, routeValues As Object) As MvcHtmlString
            Dim urlHelper As New UrlHelper(html.ViewContext.RequestContext)
            Dim href As String = urlHelper.Action(actionName, controllerName, routeValues)

            Return html.GetKendoButtonHtml(text, href)
        End Function

        <Extension()> _
        Public Function ActionKendoButton(Of TModel)(html As HtmlHelper(Of TModel), text As String, actionName As String, controllerName As String) As MvcHtmlString
            Dim urlHelper As New UrlHelper(html.ViewContext.RequestContext)
            Dim href As String = urlHelper.Action(actionName, controllerName)

            Return html.GetKendoButtonHtml(text, href)
        End Function

        <Extension()> _
        Public Function ActionKendoButton(Of TModel)(html As HtmlHelper(Of TModel), text As String, actionName As String) As MvcHtmlString
            Dim urlHelper As New UrlHelper(html.ViewContext.RequestContext)
            Dim href As String = urlHelper.Action(actionName)

            Return html.GetKendoButtonHtml(text, href)
        End Function

        <Extension()> _
        Private Function GetKendoButtonHtml(Of TModel)(html As HtmlHelper(Of TModel), text As String, href As String) As MvcHtmlString
            Dim buttonId As String = String.Format("kendoButtonFor_{0}_{1}", text.Replace(" ", ""), KendoIdCount)
            KendoIdCount += 1

            Dim buttonHtml As String = html.Kendo().Button() _
                .Name(buttonId) _
                .Tag("a") _
                .Content(text) _
                .Events(Sub(events) events.Click("function(){document.location ='" & href & "';}")) _
                .ToHtmlString()

            Return New MvcHtmlString(buttonHtml)
        End Function

        Private Property KendoIdCount As Integer = 0
    End Module
End Namespace