<Query Kind="VBProgram">
  <Connection>
    <ID>694357da-ced8-4b17-9058-1a2764041250</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Development\mkosbie\AcePump\AcePump.EfDataImpl\bin\Release\AcePump.EfDataImpl.dll</CustomAssemblyPath>
    <CustomTypeName>AcePump.EfDataImpl.AcePumpContext</CustomTypeName>
    <AppConfigPath>C:\Development\mkosbie\AcePump\AcePump.Web\Web.config</AppConfigPath>
    <DisplayName>Ace Pump Release</DisplayName>
    <CustomCxString>release</CustomCxString>
  </Connection>
</Query>

Sub Main
	Dim h = PumpHistories.Include(Function(x) x.DeliveryTicket).ToList()
	
	For Each history In h
		If history.DeliveryTicket Is Nothing Then Continue For
		
		If history.HistoryType.Trim() = "Failed" Then
			history.DeliveryTicket.PumpFailedID = history.PumpID
			history.DeliveryTicket.PumpFailedDate = history.HistoryDate
			
		ElseIf history.HistoryType.Trim() = "Dispatched" Then
			history.DeliveryTicket.PumpDispatchedID = history.PumpID
			history.DeliveryTicket.PumpDispatchedDate = history.HistoryDate
		Else
			Throw New InvalidOperationException("Unknown history type: " & history.HistoryType)
		End If
	Next
	
	SaveChanges()
End Sub