@Code
    ViewData("Title") = "Choose"
    Layout = "~/Views/Shared/_Layout.mobile.vbhtml"
End Code

<script type="text/javascript">
    function deliveryTicketID_AdditionalData() {
        return {
            term: $("#deliveryTicketID").data("kendoComboBox").input.val()
        };
    }
</script>

<div class="logout">
    Logged in as @Html.ViewContext.HttpContext.User.Identity.Name @Html.ActionLink("Logout", "Logout", "Account")
</div>

<h2>Choose a Ticket to Sign</h2>

@Using Html.BeginForm("Choose", "DeliveryTicket")
@Html.ValidationSummary(False)

    @<div>
        Ticket # <input name="id" type="text" />
        <input type="submit" value="Sign Now" class="mobile-button"/>
    </div>
End Using