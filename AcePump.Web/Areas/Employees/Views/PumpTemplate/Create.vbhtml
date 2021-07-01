@ModelType AcePump.Web.Areas.Employees.Models.DisplayDtos.PumpTemplateViewModel
@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Create"
End Code

<h2>Create</h2>

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<link rel="Stylesheet" type="text/css" href="@Url.Content("~/Content/PumpTemplate.css")" />

@Using Html.BeginForm()
    @Html.ValidationSummary(True)
    @<fieldset>
        <legend>Pump Template</legend>
        
        <div class="main-details">
            <div class="display-label">
                Pump Template ID and Number
            </div>
            <div class="display-field">
                Automatically Generated When You Click "Create"
            </div>                    
        </div>

        <input type="hidden" id="partsToAdd" name="partsToAdd" />

		<ol>
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.TubingSize, "Tubing Size")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.TubingSize, "TubingSize")
                @Html.ValidationMessageFor(Function(model) model.TubingSize)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.PumpBoreBasic, "Pump Bore Basic")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.PumpBoreBasic, "PumpBoreBasic")
                @Html.ValidationMessageFor(Function(model) model.PumpBoreBasic)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Barrel.Length, "Barrel Length")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Barrel.Length, "BarrelLength")
                @Html.ValidationMessageFor(Function(model) model.Barrel.Length)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.LowerExtension, "Lower Extension (ft)")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.LowerExtension, "LowerExtension")
                @Html.ValidationMessageFor(Function(model) model.LowerExtension)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.UpperExtension, "Upper Extension (ft)")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.UpperExtension, "UpperExtension")
                @Html.ValidationMessageFor(Function(model) model.UpperExtension)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.PumpType, "Pump Type")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.PumpType, "PumpType")
                @Html.ValidationMessageFor(Function(model) model.PumpType)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Barrel.Type, "Barrel Type")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Barrel.Type, "BarrelType")
                @Html.ValidationMessageFor(Function(model) model.Barrel.Type)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Barrel.Material, "Barrel Material")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Barrel.Material, "BarrelMaterial")
                @Html.ValidationMessageFor(Function(model) model.Barrel.Material)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Seating.Location, "Seating Location")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Seating.Location, "SeatingLocation")
                @Html.ValidationMessageFor(Function(model) model.Seating.Location)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Seating.Type, "Seating Type")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Seating.Type, "SeatingType")
                @Html.ValidationMessageFor(Function(model) model.Seating.Type)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Plunger.Material, "Plunger Material")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Plunger.Material, "PlungerMaterial")
                @Html.ValidationMessageFor(Function(model) model.Plunger.Material)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Plunger.Length, "Plunger Length")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Plunger.Length, "PlungerLength")
                @Html.ValidationMessageFor(Function(model) model.Plunger.Length)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Plunger.Fit, "Plunger Fit")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Plunger.Fit, "PlungerFit")
                @Html.ValidationMessageFor(Function(model) model.Plunger.Fit)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.HoldDownType, "Hold Down Type")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.HoldDownType, "HoldDownType")
                @Html.ValidationMessageFor(Function(model) model.HoldDownType)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.TravellingCages, "Travelling Cages")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.TravellingCages, "TravellingCages")
                @Html.ValidationMessageFor(Function(model) model.TravellingCages)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.StandingValveCages, "Standing Valve Cages")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.StandingValveCages, "StandingValveCages")
                @Html.ValidationMessageFor(Function(model) model.StandingValveCages)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.StandingValve, "Standing Valve")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.StandingValve, "StandingValve")
                @Html.ValidationMessageFor(Function(model) model.StandingValve)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.BallsAndSeats, "Balls And Seats")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.BallsAndSeats, "BallsAndSeats")
                @Html.ValidationMessageFor(Function(model) model.BallsAndSeats)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Barrel.Washer, "Barrel Washer")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Barrel.Washer, "BarrelWasher")
                @Html.ValidationMessageFor(Function(model) model.Barrel.Washer)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Collet)
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Collet, "Collet")
                @Html.ValidationMessageFor(Function(model) model.Collet)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.TopSeals, "Top Seals")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.TopSeals, "TopSeals")
                @Html.ValidationMessageFor(Function(model) model.TopSeals)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.OnOffTool, "On/Off Tool")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.OnOffTool, "OnOffTool")
                @Html.ValidationMessageFor(Function(model) model.OnOffTool)
            </div>
		</li>
        
        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.SpecialtyItems, "Specialty Items")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.SpecialtyItems, "SpecialtyItems")
                @Html.ValidationMessageFor(Function(model) model.SpecialtyItems)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.PonyRods, "Pony Rods")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.PonyRods, "PonyRods")
                @Html.ValidationMessageFor(Function(model) model.PonyRods)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.Strainers)
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.Strainers, "Strainers")
                @Html.ValidationMessageFor(Function(model) model.Strainers)
            </div>
		</li>

        <li>
            <div class="editor-label">
                @Html.LabelFor(Function(model) model.KnockOut, "Knock Out")
            </div>
            <div class="editor-field">
                @Html.TypeManagerDropDownFor(Function(model) model.KnockOut, "KnockOut")
                @Html.ValidationMessageFor(Function(model) model.KnockOut)
            </div>
		</li>
        </ol>

        <p>
            <input type="submit" value="Save and Continue to Add Parts" />
        </p>
    </fieldset>
End Using

<div>
    @Html.ActionKendoButton("Back to List", "Index")
</div>
