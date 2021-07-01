<Query Kind="VBProgram">
  <Connection>
    <ID>fbf18e39-c3ad-41d7-9ee6-8a8051e00bde</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Development\mkosbie\Ace Pump\Live Trunk\AcePump.Web\bin\AcePump.EfDataImpl.dll</CustomAssemblyPath>
    <CustomTypeName>AcePump.EfDataImpl.AcePumpContext</CustomTypeName>
    <CustomCxString>release</CustomCxString>
    <AppConfigPath>C:\Development\mkosbie\Ace Pump\Live Trunk\AcePump.Web\Web.config</AppConfigPath>
    <DisplayName>Ace Pump Live Trunk</DisplayName>
    <IsProduction>true</IsProduction>
  </Connection>
  <Namespace>System.Globalization</Namespace>
</Query>

Sub Main
	Dim enUs = CultureInfo.GetCultureInfo("en-US")
	Dim models As New List(Of TicketSummaryModel)
	Dim senecaTickets = DeliveryTickets.Where(Function(x) x.Customer.CustomerName.Contains("SENECA")).Take(100)
	For Each ticket In senecaTickets
		If Not ticket.PumpFailedID.HasValue Then Continue For
		Dim model As New TicketSummaryModel With {
			.DeliveryTicketID = ticket.DeliveryTicketID,
			.PumpRepaired = ticket.PumpFailedID.Value,
			.PumpOut = If(ticket.PumpDispatchedID.HasValue, ticket.PumpDispatchedID.Value, "NONE"),
			.RepairDate = ticket.TicketDate.Value,
			.LeaseName = ticket.Well.Lease.LocationName,
			.WellNumber = ticket.Well.WellNumber
		}
		models.Add(model)
	
		For Each inspection In ticket.Inspections
			If enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "barrel", CompareOptions.IgnoreCase) >= 0 Then
				model.Barrel = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "SeatRing", CompareOptions.IgnoreCase) >= 0 Then
				model.SeatRing = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "SvCages", CompareOptions.IgnoreCase) >= 0 Then
				model.SvCages = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "SvSeats", CompareOptions.IgnoreCase) >= 0 Then
				model.SvSeats = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "SvBalls", CompareOptions.IgnoreCase) >= 0 Then
				model.SvBalls = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "HoldDown", CompareOptions.IgnoreCase) >= 0 Then
				model.HoldDown = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "ValveRod", CompareOptions.IgnoreCase) >= 0 Then
				model.ValveRod = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "Plunger", CompareOptions.IgnoreCase) >= 0 Then
				model.Plunger = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "TvCages", CompareOptions.IgnoreCase) >= 0 Then
				model.TvCages = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "TvSeats", CompareOptions.IgnoreCase) >= 0 Then
				model.TvSeats = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "TvBalls", CompareOptions.IgnoreCase) >= 0 Then
				model.TvBalls = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "RodGuide", CompareOptions.IgnoreCase) >= 0 Then
				model.RodGuide = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "TypeBallAndSeat", CompareOptions.IgnoreCase) >= 0 Then
				model.TypeBallAndSeat = inspection.Result
	
			ElseIf enUs.CompareInfo.IndexOf(inspection.PartFailed.Description, "TypePump", CompareOptions.IgnoreCase) >= 0 Then
				model.TypePump = inspection.Result
	
			Else
				model.Unknowns.Add(inspection.PartFailed.Description)
			End If
		Next
	Next
	
	models.Dump()
End Sub

Private Class TicketSummaryModel
	Public Property DeliveryTicketID As Integer
	Public Property PumpOut As String
	Public Property PumpRepaired As String
	Public Property RepairDate As Date
	Public Property LeaseName As String
	Public Property WellNumber As String
	
	Public Property Barrel As String
	Public Property SeatRing As String
	Public Property SvCages As String
	Public Property SvSeats As String
	Public Property SvBalls As String
	Public Property HoldDown As String
	Public Property ValveRod As String
	Public Property Plunger As String
	Public Property TvCages As String
	Public Property TvSeats As String
	Public Property TvBalls As String
	Public Property RodGuide As String
	Public Property TypeBallAndSeat As String
	Public Property TypePump As String
	Public Property Unknowns As New List(Of String)
End Class