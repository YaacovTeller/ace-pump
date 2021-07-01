Imports AcePump.Web.Controllers
Imports AcePump.Common

Namespace Areas.Employees.Controllers
    <Authorize()> _
    Public Class HomeController
        Inherits AcePumpControllerBase

        '
        ' GET: /[Home]/[Index]

        Public Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /Home/DropDowns

        <Authorize(Roles:=AcePumpSecurityRoles.TypeManager)> _
        Public Function DropDowns() As ActionResult
            Dim availableTypes As New List(Of String) From {
                "BallsAndSeats",
                "BarrelLength",
                "BarrelMaterial",
                "BarrelType",
                "BarrelWasher",
                "Collet",
                "HoldDownType",
                "InvBallsAndSeatsCondition",
                "InvBallsCondition",
                "InvBarrelCondition",
                "InvCagesCondition",
                "InvHoldDownCondition",
                "InvPlungerCondition",
                "InvRodGuideCondition",
                "InvSeatsCondition",
                "InvValveRodCondition",
                "KnockOut",
                "LowerExtension",
                "OnOffTool",
                "PlungerFit",
                "PlungerLength",
                "PlungerMaterial",
                "PonyRods",
                "PumpBoreBasic",
                "PumpType",
                "ReasonRepaired",
                "SeatingLocation",
                "SeatingType",
                "SoldByOption",
                "SpecialtyItems",
                "StandingValve",
                "StandingValveCages",
                "Strainers",
                "TopSeals",
                "TravellingCages",
                "TubingSize",
                "UpperExtension",
                "TicketCompletedBy"
            }

            Return View(availableTypes)
        End Function
    End Class
End Namespace
