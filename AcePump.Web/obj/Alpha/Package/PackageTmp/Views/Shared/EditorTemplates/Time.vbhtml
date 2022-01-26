@ModelType DateTime?
           
@(Html.Kendo().TimePickerFor(Function(x) x) _
    .Format("hh:mm tt") _
    .Interval(1) _
    .HtmlAttributes(New With {.style = "width:110px", .type = ""})
 )