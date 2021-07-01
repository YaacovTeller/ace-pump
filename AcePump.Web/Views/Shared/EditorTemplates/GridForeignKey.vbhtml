@ModelType Object
           
@(
     Html.Kendo().DropDownListFor(Function(x) x) _
        .BindTo(DirectCast(ViewData(ViewData.TemplateInfo.GetFullHtmlFieldName("") & "_Data"), SelectList))
)
