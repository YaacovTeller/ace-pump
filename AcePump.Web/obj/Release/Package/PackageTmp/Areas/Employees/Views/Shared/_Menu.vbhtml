@Imports AcePump.Web
@Imports AcePump.Common

@* Employees Menu *@

@(Html.Kendo().Menu() _
   .Name("menu") _
   .Items(Sub(menu)
              menu.Add().Text("Home").Action("Index", "Home")
              menu.Add().Text("Customers").Items(Sub(subMenu)
                                                     subMenu.Add().Text("Customers").Action("Index", "Customer")
                                                     subMenu.Add().Text("County Sales Tax Rates").Action("Index", "CountySalesTaxRate")
                                                 End Sub)
              menu.Add().Text("Pumps").Items(Sub(subMenu)
                                                 subMenu.Add().Text("Pumps").Action("Index", "Pump")
                                                 subMenu.Add().Text("Templates").Action("Index", "PumpTemplate")
                                                 subMenu.Add().Text("Parts").Action("Index", "PartTemplate")
                                                 subMenu.Add().Text("Assemblies").Action("Index", "Assembly")
                                                 subMenu.Add().Text("Inventory").Action("Inventory", "Part")
                                             End Sub)
              menu.Add().Text("Locations").Action("Index", "Well")
              menu.Add().Text("Delivery Tickets").Url(Url.Action("Index", "DeliveryTicket"))
              menu.Add().Text("History").Items(Sub(items)
                                                   items.Add().Text("Lease Dashboard").Action("LeaseDashboard", "History")
                                                   items.Add().Text("Well Dashboard").Action("WellDashboard", "History")
                                                   items.Add().Text("Customer Dashboard").Action("CustomerDashboard", "History")
                                                   items.Add().Text("Part Repaired Dashboard").Action("PartDashboard", "History")
                                                   items.Add().Text("Reason Repaired Dashboard").Action("ReasonRepairedDashboard", "History")
                                               End Sub)
              If Context.AcePumpUser.IsInRole(AcePumpSecurityRoles.AccountManager) Then
                  menu.Add().Text("Accounts").Action("Index", "Account", New With {.Area = ""})
              End If

              If Context.AcePumpUser.IsInRole(AcePumpSecurityRoles.TypeManager) Then
                  menu.Add().Text("Edit Drop Downs").Action("DropDowns", "Home")
              End If
          End Sub) _
      .SecurityTrimming(Sub(sc)
                            sc.Enabled(True)
                            sc.HideParent(True)
                        End Sub))