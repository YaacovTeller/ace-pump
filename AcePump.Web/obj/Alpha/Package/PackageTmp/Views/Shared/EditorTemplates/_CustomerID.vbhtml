@(Html.Kendo().ComboBoxFor(Function(x) x) _
    .Name("CustomerID") _
    .DataTextField("Name") _
    .DataValueField("Id") _
    .Filter(FilterType.StartsWith) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Sub(read)
                                      read.Action("StartsWith", "Customer")
                                      'read.Data("CustomerID_Data")
                              End Sub) _
                        .ServerFiltering(True)
                End Sub))