@(Html.Kendo().ComboBox() _
                            .Name("_SelectPartByNumber_ComboBox") _
                            .DataSource(Sub(config)
                                            config.Read(Sub(read)
                                                            read _
                                                            .Action("List", "PartTemplate") _
                                                            .Type(HttpVerbs.Post)
                                                        End Sub)
                                        End Sub) _
                            .DataTextField("PartTemplateNumber") _
                            .DataValueField("PartTemplateID")
)