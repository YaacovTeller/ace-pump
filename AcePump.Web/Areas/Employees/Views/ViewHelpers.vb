Imports System.Runtime.CompilerServices
Imports System.Linq.Expressions
Imports System.Web.Mvc
Imports Kendo.Mvc.UI

Namespace Views
    Public Module ViewHelpers
        <Extension()> _
        Public Function AcePump_TypeManagerDropDownFor(Of TModel, TProperty)(helper As HtmlHelper(Of TModel), [property] As Expression(Of Func(Of TModel, TProperty)), typeName As String, Optional useDisplayTextAsValue As Boolean = True) As MvcHtmlString
            Return TypeManagerDropDownFor(helper, [property], typeName, useDisplayTextAsValue)
        End Function

        <Extension()> _
        Public Function TypeManagerDropDownFor(Of TModel, TProperty)(helper As HtmlHelper(Of TModel), [property] As Expression(Of Func(Of TModel, TProperty)), typeName As String, Optional useDisplayTextAsValue As Boolean = True) As MvcHtmlString
            Dim typeManagerUrl As String = UrlHelper.GenerateUrl(routeName:="Soris_TypeManager",
                                                                 actionName:="ListOnly",
                                                                 controllerName:="TypeManager",
                                                                 routeValues:=New RouteValueDictionary() From {{"type", typeName}},
                                                                 routeCollection:=RouteTable.Routes,
                                                                 requestContext:=helper.ViewContext.RequestContext,
                                                                 includeImplicitMvcValues:=False)

            Dim valueField As String = If(useDisplayTextAsValue, "DisplayText", "ItemTypeID")
            Return New MvcHtmlString(
                helper.Kendo().DropDownListFor([property]) _
                    .DataSource(Sub(dataSource)
                                    dataSource.Read(Sub(conf)
                                                        conf.Url(typeManagerUrl)
                                                        conf.Type(HttpVerbs.Post)
                                                    End Sub)
                                End Sub) _
                    .DataTextField("DisplayText") _
                    .DataValueField(valueField) _
                    .ValuePrimitive(True) _
                    .AutoBind(False) _
                    .ToHtmlString()
                )
        End Function

        <Extension()> _
        Public Function TypeManagerListUrl(helper As UrlHelper, typeName As String) As String
            Dim url As String = UrlHelper.GenerateUrl(routeName:="Soris_TypeManager",
                                                      actionName:="ListOnly",
                                                      controllerName:="TypeManager",
                                                      routeValues:=New RouteValueDictionary() From {{"type", typeName}},
                                                      routeCollection:=RouteTable.Routes,
                                                      requestContext:=helper.RequestContext,
                                                      includeImplicitMvcValues:=False)

            Return url
        End Function

        <Extension()> _
        Public Function TypeManagerComboBoxFor(Of TModel, TProperty)(helper As HtmlHelper(Of TModel), [property] As Expression(Of Func(Of TModel, TProperty)), typeName As String) As MvcHtmlString
            Dim typeManagerUrl As String = UrlHelper.GenerateUrl(routeName:="Soris_TypeManager",
                                                                 actionName:="ListOnly",
                                                                 controllerName:="TypeManager",
                                                                 routeValues:=New RouteValueDictionary() From {{"type", typeName}},
                                                                 routeCollection:=RouteTable.Routes,
                                                                 requestContext:=helper.ViewContext.RequestContext,
                                                                 includeImplicitMvcValues:=False)

            Return New MvcHtmlString(
                helper.Kendo().ComboBoxFor([property]) _
                    .DataSource(Sub(dataSource)
                                    dataSource.Read(Sub(conf)
                                                        conf.Url(typeManagerUrl)
                                                        conf.Type(HttpVerbs.Post)
                                                    End Sub)
                                End Sub) _
                    .DataTextField("DisplayText") _
                    .DataValueField("DisplayText") _
                    .AutoBind(False) _
                    .Events(Function(e) e.Change("TypeManagerComboBoxForChange")) _
                    .ToHtmlString()
                )
        End Function

    End Module
End Namespace