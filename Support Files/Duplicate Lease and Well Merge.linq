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
	Dim duplicateLeaseNames = (From lease In LeaseLocations
							   Where lease.LocationName IsNot Nothing
						       Group By lease.LocationName Into g = Group
						       Where g.Count() > 1
						       Select LocationName) _
							   .ToList()
						 
	For Each leaseName As String In duplicateLeaseNames
		Dim leases = LeaseLocations.Where(Function(x) x.LocationName = leaseName).ToList()
		Dim mergeToLeaseId As Integer = leases.First().LeaseID
		Dim otherLeases = leases.Where(Function(x) x.LeaseID <> mergeToLeaseId).ToList()
		
		UpdateLeaseDependencies(mergeToLeaseId, otherLeases.Select(Function(x) x.LeaseID).ToList())
		RemoveLeases(otherLeases)
	Next
	
	Dim duplicateWells = (From well In WellLocations
						  Where well.WellNumber IsNot Nothing
					      Group By well.WellNumber, well.LeaseID Into g = Group
					      Where g.Count() > 1
					      Select New With {.WellNumber = WellNumber, .LeaseID = LeaseID }) _
						  .ToList()
						 
	For Each match In duplicateWells
		Dim wells = WellLocations.Where(Function(x) x.WellNumber = match.WellNumber And x.LeaseID = match.LeaseID).ToList()
		Dim mergeToWellId As Integer = wells.First().WellID
		Dim otherWells = wells.Where(Function(x) x.WellID <> mergeToWellId).ToList()
		
		UpdateWellDependencies(mergeToWellId, otherWells.Select(Function(x) x.WellId).ToList())
		RemoveWells(otherWells)
	Next
End Sub

Private Sub UpdateLeaseDependencies(mergeToLeaseId As Integer, otherLeaseIds As List(Of Integer))
	Dim wellsToUpdate = WellLocations.Where(Function(x) otherLeaseIds.Contains(x.LeaseID))
	
	For Each well In wellsToUpdate
		well.LeaseID = mergeToLeaseId
	Next
	
	SaveChanges()
End Sub

Private Sub RemoveLeases(leases)
	For Each lease In leases
		LeaseLocations.Remove(lease)
	Next
	
	SaveChanges()
End Sub

Private Sub UpdateWellDependencies(mergeToWellId As Integer, otherWellIds As List(Of Integer))
	Dim ticketsToUpdate = DeliveryTickets.Where(Function(x) otherWellIds.Contains(x.WellID))
	
	For Each ticket In ticketsToUpdate
		ticket.WellID = mergeToWellId
	Next
	
	SaveChanges()
	
	Dim pumpsToUpdate = Pumps.Where(Function(x) otherWellIds.Contains(x.InstalledInWellID))
	
	For Each pump In pumpsToUpdate
		pump.InstalledInWellID = mergeToWellId
	Next
	
	SaveChanges()
End Sub

Private Sub RemoveWells(wells)
	For Each well In wells
		WellLocations.Remove(well)
	Next
	
	'SaveChanges()
End Sub