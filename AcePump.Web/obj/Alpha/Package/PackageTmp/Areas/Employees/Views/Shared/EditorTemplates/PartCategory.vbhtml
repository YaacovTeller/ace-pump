@(Html.Kendo().ComboBoxFor(Function(x) x) _
    .DataTextField("CategoryName") _
    .DataValueField("CategoryID") _
    .MinLength(2) _
    .Filter(FilterType.StartsWith) _
    .Placeholder("Start typing...") _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Sub(reader)
                                      reader.Action("StartsWith", "PartCategory")
                                      reader.Type(HttpVerbs.Post)
                              End Sub) _
                        .ServerFiltering(True)
                End Sub)
    )