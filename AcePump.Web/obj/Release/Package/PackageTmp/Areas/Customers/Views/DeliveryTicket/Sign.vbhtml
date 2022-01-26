@ModelType AcePump.Web.Areas.Customers.Models.DeliveryTicketSignatureViewModel

@Code
    ViewData("Title") = "Sign"
    Layout = "~/Views/Shared/_Layout.mobile.vbhtml"
End Code
<div class="logout">
    Logged in as @Html.ViewContext.HttpContext.User.Identity.Name @Html.ActionLink("Logout", "Logout", "Account")
</div>

<h2 id="title">Sign Ticket # @Model.DeliveryTicketID</h2>

<div class="deliveryticket-summary">
    <ol>
    <div class="location-summary">
        <li>
            <div class="label">Customer:</div>
            <div class="field">@Html.DisplayFor(Function(model) model.CustomerName)</div>
        </li>
        <li>
            <div class="label">Lease / Well: </div>
            <div class="field">@Html.DisplayFor(Function(model) model.Lease) / @Html.DisplayFor(Function(model) model.Well)</div>
        </li>
    </div>
    <div class="ticket-details-summary">
        <li>
            <div class="label">Ticket Date</div>
            <div class="field">@String.Format("{0:d}", Model.TicketDate)</div>
        </li>
        <li>
            <div class="label">Closed Ticket?</div>
            <div class="field">@Html.DisplayFor(Function(model) model.CloseTicket)</div>   
        </li>
    </div>
    </ol>

    @If Model.LineItems.Any() Then
        @<table id="line-items-table">
            <thead>
                <tr>
                    <th>Quantity</th>
                    <th>Item</th>
                    <th>Unit Price</th>
                    <th>Line Total</th>
                </tr>
            </thead>
            <tbody>
                @For Each lineItem In Model.LineItems
                    @<tr>
                        <td>@lineItem.Quantity</td>
                        <td>@lineItem.Item</td>
                        <td>@lineItem.UnitPrice.ToString("c")</td>
                        <td>@lineItem.LineTotal.ToString("c")</td>
                    </tr>
                Next

                @Code
                Dim salesTotal As Decimal = Model.LineItems.Sum(Function(x) x.LineTotal)
                Dim taxOwed As Decimal = Model.LineItems.Sum(Function(x) x.LineTotal * If(x.LineIsTaxable, Model.SalesTaxRate, 0))
                Dim grandTotal As Decimal = salesTotal + taxOwed
                End Code
                
                <tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td><td>&nbsp;</td><td>Sales Total</td><td><strong>@salesTotal.ToString("c")</strong></td></tr>
                <tr><td>&nbsp;</td><td>&nbsp;</td><td>Sales Tax @@ <strong>@Model.SalesTaxRate.ToString("p")</strong></td><td><strong>@taxOwed.ToString("c")</strong></td></tr>
                <tr><td>&nbsp;</td><td>&nbsp;</td><td>Invoice Total</td><td><strong>@grandTotal.ToString("c")</strong></td></tr>
            </tbody>
        </table>
    End If
</div>

<div style="clear:both"/>

<script src="@Url.Content("~/Scripts/jquery-1.5.1.min.js")" type="text/javascript"></script>

<script type="text/javascript" src="@Url.Content("~/Scripts/jSignature.min.js")"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var sigdiv = $("#newSignatureObj").jSignature({ 'UndoButton': false });

        $("#reset").bind("click", function (e) {
                sigdiv.jSignature("reset")
        });

        $("#btnSign").bind("click", function (e) {
                sign()
        });

        $("#btnResign").bind("click", function (e) {
                resign()
        });

        $("#btnDeleteSignature").bind("click", function (e) {
            deleteSignature()
        });

        $("#btnCancel").bind("click", function (e) {
            location.reload();
        });

        $("#btnChoose").bind("click", function (e) {
            document.location = "@Url.Action("Choose")";
        });

        if ("@model.SignatureDate.HasValue" === "True") {
            showExistingSignature();
        } else {
            hideAndClearExistingSignature();
        }
    });

    function resign() {
        hideAndClearExistingSignature();
    }

    function deleteSignature() {
            $.ajax({
            dataType: "json",
            url: "@Url.Action("DeleteSignature", "DeliveryTicket")",
            data: {id: @Model.DeliveryTicketID},
            type: "POST",
            success: function(result) {           
                hideAndClearExistingSignature();
            },
            error: function(data) {
            }           
        });
    };

    function sign() {
        var signatureString = getSignatureAsSvg();

        if ($("#SignatureName").val() ==="") {
            alert("You must enter a name.");            
        } else if( $("#newSignatureObj").jSignature('getData', 'native').length == 0) {
            alert("You must enter a signature.");            
        } else {
            $("#SignatureBase64").val(signatureString);
            $("form").submit();
        }
    }

    function getSignatureAsSvg() {
        return $("#newSignatureObj").jSignature("getData");
    }

    function hideAndClearExistingSignature() {
        $("#SignatureName").val("");
        $("#SignatureCompanyName").val("");
        $("#newSignatureObj").jSignature("reset")

        $(".newSignature").show();
        $(".existingSignature").hide();

        $("#title").html("Please sign Delivery Ticket " + @Model.DeliveryTicketID)
    }

    function showExistingSignature() {
        $(".newSignature").hide();
        $(".existingSignature").show();

        $("#SignatureDate").html("@Model.SignatureDate");
        $("#title").html("Signature Details of Delivery Ticket " + @Model.DeliveryTicketID)
    }
</script>

<style type="text/css">

	div {
		margin-top: 0.5em;
		margin-bottom: 0.5em;
	}

	#signatureparent {
		color:darkblue;
		background-color:darkgrey;
		/*max-width:600px;*/
		padding:1%;
	}
	
	/*This is the div within which the signature canvas is fitted*/
	#newSignatureObj {
		border: 2px dotted black;
		background-color:lightgrey;
	}

	/* Drawing the 'gripper' for touch-enabled devices */ 
	html.touch #newSignatureContent {
		float:left;
		width:92%;
	}
	html.touch #scrollgrabber {
		float:right;
		width:4%;
		margin-right:2%;
		background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAAFCAAAAACh79lDAAAAAXNSR0IArs4c6QAAABJJREFUCB1jmMmQxjCT4T/DfwAPLgOXlrt3IwAAAABJRU5ErkJggg==)
	}
	html.borderradius #scrollgrabber {
		border-radius: 1em;
	}
	
	#line-items-table {
	    clear: both;
	    
	    border-spacing: 0px;
	    border-collapse: collapse;
	    
	    margin-left: 40px;
	}
	
	#line-items-table td, #line-items-table th {
	    border: 1px solid black;
	    padding: 4px;
	}
	 
</style>
@Using Html.BeginForm()
@Html.ValidationSummary(False)

@<fieldset>
    @Html.HiddenFor(Function(model) model.DeliveryTicketID)
    @Html.HiddenFor(Function(model) model.SignatureBase64)

    <div class="existingSignature">
        @Code             
            If Model.Signature IsNot Nothing Then
                @<img src="data:image;base64,@System.Convert.ToBase64String(Model.Signature)"  width="100%" alt="Signature"/>
            End If            
        End Code
    </div>

    <div id="newSignatureContent" class="newSignature">
	    <div id="signatureparent">
		    <div id="newSignatureObj"></div>
        </div>
	    <div id="tools"><input type="button" value="Reset" id="reset" class="mobile-button"/></div>
    </div>

    
    <ol>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.SignatureName, "Name:")
            </div>

            <div class="existingSignature">
                <div class="display-field">
                    @model.SignatureName
                </div>
            </div>

            <div class="newSignature">
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.SignatureName)
                    @Html.ValidationMessageFor(Function(model) model.SignatureName)
                </div>
            </div>
        </li>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.SignatureCompanyName, "Company Name:")
            </div>

            <div class="existingSignature">
                <div class="display-field">
                    @Model.SignatureCompanyName
                </div>
            </div>

            <div class="newSignature">
                <div class="editor-field">
                    @Html.EditorFor(Function(model) model.SignatureCompanyName)
                    @Html.ValidationMessageFor(Function(model) model.SignatureCompanyName)
                </div>
            </div>
        </li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.SignatureDate, "Date:")
            </div>

            <div class="existingSignature">
                <div class="display-field">
                    @String.Format("{0:d}", Model.SignatureDate)
                </div>
            </div>
            
            <div class="newSignature">
                <div class="editor-field">
                    @Date.Today.ToShortDateString()
                </div>
            </div>
        </li>
   </ol>

   <div class="newSignature">
        <input type="button" value="Sign" class="submit-area mobile-button" id="btnSign"/>
        <input type="button" value="Cancel" class="submit-area mobile-button" id="btnCancel"/>
   </div>

   <div class="existingSignature">
        <input type="button" value="Resign" class="submit-area mobile-button" id="btnResign"/>
   </div>

   <input type="button" value="Choose a different ticket" class="submit-area mobile-button" id="btnChoose"/>
</fieldset>       
End Using    
