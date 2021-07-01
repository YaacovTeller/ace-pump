@Imports AcePump.Web
@Imports AcePump.Common

@* Anonymous and Common Controller Menu *@

@If Context.AcePumpUser.IsInRole(AcePumpSecurityRoles.AcePumpEmployee) Then
    @Html.Partial("~/Areas/Employees/Views/Shared/_Menu.vbhtml")
    
ElseIf Context.AcePumpUser.IsInRole(AcePumpSecurityRoles.Customer) Then
    @Html.Partial("~/Areas/Customers/Views/Shared/_Menu.vbhtml")

Else
    @Html.Kendo().Menu().Name("menu").Items(Sub(menu)menu.Add().Text("Please Log In"))
End If