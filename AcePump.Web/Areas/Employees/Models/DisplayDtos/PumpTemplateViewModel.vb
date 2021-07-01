Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PumpTemplateViewModel
        Public Property PumpTemplateID As Integer

        Public Property ConciseSpecificationSummary As String
        Public Property VerboseSpecificationSummary As String

        Public Property Barrel As PumpBarrelViewModel
        Public Property Plunger As PumpPlungerViewModel
        Public Property Seating As PumpSeatingViewModel

        <Required()> _
        Public Property TubingSize As String
        <Required()> _
        Public Property PumpBoreBasic As String
        <Required()> _
        Public Property LowerExtension As String
        <Required()> _
        Public Property UpperExtension As String
        <Required()> _
        Public Property PumpType As String
        <Required()> _
        Public Property HoldDownType As String
        <Required()> _
        Public Property StandingValveCages As String
        <Required()> _
        Public Property StandingValve As String
        <Required()> _
        Public Property BallsAndSeats As String
        <Required()> _
        Public Property TravellingCages As String
        <Required()> _
        Public Property Collet As String
        <Required()> _
        Public Property TopSeals As String
        <Required()> _
        Public Property OnOffTool As String
        <Required()> _
        Public Property SpecialtyItems As String
        <Required()> _
        Public Property PonyRods As String
        <Required()> _
        Public Property Strainers As String
        <Required()> _
        Public Property KnockOut As String

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property TotalPartCost As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property TotalPartResale As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <Range(0.0, 0.999)> _
        Public Property DiscountRate As Decimal

        <DisplayFormat(DataFormatString:="{0:p}")> _
        <Range(0.0, 0.999)> _
        Public Property MarkupRate As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property ResalePrice As Decimal

        <DisplayFormat(DataFormatString:="{0:c}")> _
        Public Property ListPrice As Decimal
    End Class
End Namespace