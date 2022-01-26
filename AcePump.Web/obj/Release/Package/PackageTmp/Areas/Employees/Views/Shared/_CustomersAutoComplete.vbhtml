 @(Html.Kendo().AutoComplete() _
        .Name("CustomerAutoComplete") _
        .DataTextField("Name") _
        .MinLength(2) _
        .Placeholder("Start typing a customer name...") _
        .Events(Sub(e)
                        e.Select("jumpToCustomer_SelectCustomer")
                End Sub) _
        .DataSource(Sub(dataSource)
                            dataSource _
                            .Read(Function(reader) reader.Action("StartsWith", "Customer") _
                                                            .Data("onAdditionalData")) _
                            .ServerFiltering(True)
                    End Sub)
    )