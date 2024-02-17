@(Html.Kendo().ComboBoxFor(Function(x) x) _
                        .Name("CustomerAccessList") _
                        .DataTextField("Name") _
                        .DataValueField("Id") _
                        .Filter(FilterType.StartsWith) _
                        .DataSource(Sub(dataSource)
                                        dataSource _
                            .Read(Sub(read)
                                      read.Action("StartsWith", "Customer")
                                  End Sub) _
                            .ServerFiltering(True)
                                    End Sub))