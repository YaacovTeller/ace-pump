@Imports AcePump.Web
@Imports AcePump.Common

@* Customers Menu *@

@(Html.Kendo().Menu() _
   .Name("menu") _
   .Items(Sub(menu)
              menu.Add().Text("Home").Action("Index", "Home")
              menu.Add().Text("Pumps").Items(Sub(items)
                                                 items.Add().Text("Pumps").Action("Index", "Pump")
                                                 If Context.AcePumpUser.Profile.UsesInventory Then
                                                     items.Add().Text("Inventory").Action("Inventory", "Part")
                                                 End If
                                             End Sub)
              menu.Add().Text("Delivery Tickets").Action("Index", "DeliveryTicket")
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
          End Sub) _
      .SecurityTrimming(False))