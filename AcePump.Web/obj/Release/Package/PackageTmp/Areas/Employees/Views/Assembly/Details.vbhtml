@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.AssemblyModel
@Imports AcePump.Common
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Details"
End Code

<h2>Details</h2>

<fieldset>
    <legend>Assembly</legend>
	
	<ol>

    <li>
		<div class="display-label">Assembly ID</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.AssemblyID)
            @Html.ActionKendoButton("View Related Part", "Details", "PartTemplate", New With {.id = Model.RelatedPartTemplateID})
		</div>
    </li>

    <li>
		<div class="display-label">Description</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.Description)
		</div>
    </li>

    <li>
		<div class="display-label">Assembly Number</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.AssemblyNumber)
		</div>
    </li>

    <li>
		<div class="display-label">Category</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.Category)
		</div>
    </li>

    @If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
        @<li>
		    <div class="display-label">Total Parts Cost</div>
		    <div class="display-field">
			    @Html.DisplayFor(Function(model) model.TotalPartsCost)
		    </div>
        </li>
        
        @<li>
		    <div class="display-label">Markup</div>
		    <div class="display-field">
			    @Html.DisplayFor(Function(model) model.Markup)
		    </div>
        </li>
    End If

    <li>
		<div class="display-label">Discount</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.Discount)
		</div>
    </li>

    <li>
		<div class="display-label">Resale Price</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.ResalePrice)
		</div>
    </li>

    <li>
		<div class="display-label">Total Parts Resale Price</div>
		<div class="display-field">
			@Html.DisplayFor(Function(model) model.TotalPartsResalePrice)
		</div>
    </li>
    </ol>

    <div style="clear: both; padding-top: 40px">
        <h2>Parts</h2>
        
        @(Html.Kendo().Grid(Of AssemblyPartListRowViewModel) _
            .Name("parts") _
            .DataSource(Sub(config)
                            config _
                            .Ajax() _
                            .Sort(Sub(sort)
                                      sort.Add(Function(x) x.SortOrder)
                                  End Sub) _
                            .Read("PartList", "Assembly", New With {.id = Model.AssemblyID})
                        End Sub) _
            .Columns(Sub(config)
                         config.Bound(Function(x) x.PartsQuantity).Width(20).Title("Quantity")
                         config.Bound(Function(x) x.PartTemplateNumber).Title("Part Number")
                         config.Bound(Function(x) x.Description).Title("Part Description")

                         If User.IsInRole(AcePumpSecurityRoles.SeeCost) Then
                             config.Bound(Function(x) x.Cost).Format("{0:C}")
                         End If

                         config.Bound(Function(x) x.ResaleValue).Title("Resale").Format("{0:C}")
                         config.Bound(Function(x) x.TotalResaleValue).Title("Line Total").Format("{0:C}")
                     End Sub)
        )
    </div>
</fieldset>
<p>
    @Html.ActionKendoButton("Edit", "Edit", New With {.id = Model.AssemblyID}) |
    @Html.ActionKendoButton("Back to List", "Index")
</p>