BEGIN TRANSACTION
exec sp_rename 'tblApiTokens', 'ApiTokens';  
exec sp_rename 'tblAcePumpProfiles', 'AcePumpProfiles';
exec sp_rename 'tblAssemblies', 'Assemblies';  
exec sp_rename 'tblCountySalesTaxRates', 'CountySalesTaxRates';  
exec sp_rename 'tblCustomerPartSpecials', 'CustomerPartSpecials';  
exec sp_rename 'tblCustomers', 'Customers';  
exec sp_rename 'tblDeliveryTicketImageUploads', 'DeliveryTicketImageUploads';  
exec sp_rename 'tblLeaseLocations', 'Leases';  
exec sp_rename 'tblLineItems', 'LineItems';  
exec sp_rename 'tblMaterials', 'Materials';  
exec sp_rename 'tblPartInstances', 'Parts';  
exec sp_rename 'tblPartRuntimes', 'PartRuntimes';  
exec sp_rename 'tblPartRuntimeSegments', 'PartRuntimeSegments';  
exec sp_rename 'tblParts', 'PartTemplates';  
exec sp_rename 'tblPartsAssemblies', 'AssemblyPartDefs';  
exec sp_rename 'tblPartsCategory', 'PartCategories';  
exec sp_rename 'tblPumpRuntimes', 'PumpRuntimes';  
exec sp_rename 'tblPumps', 'Pumps';
exec sp_rename 'tblPumpTemplates', 'PumpTemplates';
exec sp_rename 'tblQbInvoiceClasses', 'QbInvoiceClasses';
exec sp_rename 'tblRepairTickets', 'PartInspections';
exec sp_rename 'tblRoles', 'Roles';
exec sp_rename 'tblShopTickets', 'DeliveryTickets';
exec sp_rename 'tblTemplatePartsJoin', 'TemplatePartDefs';
exec sp_rename 'tblTypes_BallsAndSeats', 'Types_BallsAndSeats';
exec sp_rename 'tblTypes_BarrelLength', 'Types_BarrelLength';
exec sp_rename 'tblTypes_BarrelMaterial', 'Types_BarrelMaterial';
exec sp_rename 'tblTypes_BarrelType', 'Types_BarrelType';
exec sp_rename 'tblTypes_BarrelWasher', 'Types_BarrelWasher';
exec sp_rename 'tblTypes_Collet', 'Types_Collet';
exec sp_rename 'tblTypes_HoldDownType', 'Types_HoldDownType';
exec sp_rename 'tblTypes_InvBallsAndSeatsCondition', 'Types_InvBallsAndSeatsCondition';
exec sp_rename 'tblTypes_InvBallsCondition', 'Types_InvBallsCondition';
exec sp_rename 'tblTypes_InvBarrelCondition', 'Types_InvBarrelCondition';
exec sp_rename 'tblTypes_InvCagesCondition', 'Types_InvCagesCondition';
exec sp_rename 'tblTypes_InvHoldDownCondition', 'Types_InvHoldDownCondition';
exec sp_rename 'tblTypes_InvPlungerCondition', 'Types_InvPlungerCondition';
exec sp_rename 'tblTypes_InvRodGuideCondition', 'Types_InvRodGuideCondition';
exec sp_rename 'tblTypes_InvSeatsCondition', 'Types_InvSeatsCondition';
exec sp_rename 'tblTypes_InvValveRodCondition', 'Types_InvValveRodCondition';
exec sp_rename 'tblTypes_KnockOut', 'Types_KnockOut';
exec sp_rename 'tblTypes_LowerExtension', 'Types_LowerExtension';
exec sp_rename 'tblTypes_OnOffTool', 'Types_OnOffTool';
exec sp_rename 'tblTypes_PlungerFit', 'Types_PlungerFit';
exec sp_rename 'tblTypes_PlungerLength', 'Types_PlungerLength';
exec sp_rename 'tblTypes_PlungerMaterial', 'Types_PlungerMaterial';
exec sp_rename 'tblTypes_PonyRods', 'Types_PonyRods';
exec sp_rename 'tblTypes_PumpBoreBasic', 'Types_PumpBoreBasic';
exec sp_rename 'tblTypes_PumpType', 'Types_PumpType';
exec sp_rename 'tblTypes_ReasonRepaired', 'Types_ReasonRepaired';
exec sp_rename 'tblTypes_SeatingLocation', 'Types_SeatingLocation';
exec sp_rename 'tblTypes_SeatingType', 'Types_SeatingType';
exec sp_rename 'tblTypes_SoldByOption', 'Types_SoldByOption';
exec sp_rename 'tblTypes_SpecialtyItems', 'Types_SpecialtyItems';
exec sp_rename 'tblTypes_StandingValve', 'Types_StandingValve';
exec sp_rename 'tblTypes_StandingValveCages', 'Types_StandingValveCages';
exec sp_rename 'tblTypes_Strainers', 'Types_Strainers';
exec sp_rename 'tblTypes_TicketCompletedBy', 'Types_TicketCompletedBy';
exec sp_rename 'tblTypes_TopSeals', 'Types_TopSeals';
exec sp_rename 'tblTypes_TravellingCages', 'Types_TravellingCages';
exec sp_rename 'tblTypes_TubingSize', 'Types_TubingSize';
exec sp_rename 'tblTypes_UpperExtension', 'Types_UpperExtension';
exec sp_rename 'tblUsernameCustomerAccess', 'UsernameCustomerAccess';
exec sp_rename 'tblUserRoles', 'UserRoles';
exec sp_rename 'tblUsers', 'Users';
exec sp_rename 'tblWellLocation', 'Wells';

COMMIT TRANSACTION

BEGIN TRANSACTION
	exec sp_rename 'Assemblies.KPAssemblyID', 'AssemblyID';
	exec sp_rename 'Assemblies.KFCategoryID', 'PartCategoryID';
	exec sp_rename 'Assemblies.AssemblyDescription', 'Description';
	exec sp_rename 'AssemblyPartDefs.KPPartsAssembliesID', 'AssemblyPartDefID';
	exec sp_rename 'AssemblyPartDefs.KFAssemblyID', 'AssemblyID';
	exec sp_rename 'AssemblyPartDefs.KFPartsID', 'PartTemplateID';
	exec sp_rename 'Customers.KPCustomerID', 'CustomerID';
	exec sp_rename 'CustomerPartSpecials.PartID', 'PartTemplateID';
	exec sp_rename 'DeliveryTickets.KPShopTicketID', 'DeliveryTicketID';
	exec sp_rename 'DeliveryTickets.KFWellLocationID', 'WellID';
	exec sp_rename 'DeliveryTickets.KFCustomerID', 'CustomerID';
	exec sp_rename 'Leases.KPLeaseID', 'LeaseID';
	exec sp_rename 'LineItems.KPLineItemID', 'LineItemID';
	exec sp_rename 'LineItems.KFShopTicketID', 'DeliveryTicketID';
	exec sp_rename 'LineItems.KFPartID', 'PartTemplateID';
	exec sp_rename 'LineItems.KFRepairTicketID', 'PartInspectionID';
	exec sp_rename 'LineItems.SalesTax', 'CollectSalesTax';
	exec sp_rename 'Materials.KPMaterialsID', 'MaterialID';
	exec sp_rename 'Materials.MaterialsName', 'MaterialName';
	exec sp_rename 'PartCategories.KPCategoryID', 'PartCategoryID';
	exec sp_rename 'PartInspections.KPRepairID', 'PartInspectionID';
	exec sp_rename 'PartInspections.KFShopTicketID', 'DeliveryTicketID';
	exec sp_rename 'PartInspections.KFPartReplacedID', 'PartReplacedID';
	exec sp_rename 'PartInspections.KFPartFailedID', 'PartFailedID';
	exec sp_rename 'PartInspections.KFParentAssemblyID', 'ParentAssemblyID';	
	exec sp_rename 'PartInspections.ReplaceQuantity', 'ReplacementQuantity';
	exec sp_rename 'PartInspections.Status', 'Result';
	exec sp_rename 'PartTemplates.KPPartID', 'PartTemplateID';
	exec sp_rename 'PartTemplates.KFPartCategory', 'PartCategoryID';
	exec sp_rename 'PartTemplates.KFMaterialsID', 'MaterialID';
	exec sp_rename 'PartTemplates.KFOptionID', 'SoldByOptionID';
	exec sp_rename 'PartTemplates.PartNumber', 'Number';
	exec sp_rename 'PartTemplates.KFAssemblyID', 'RelatedAssemblyID';
	exec sp_rename 'Pumps.KPPumpID', 'PumpID';
	exec sp_rename 'Pumps.KFWellLocationID', 'InstalledInWellID';
	exec sp_rename 'Pumps.KFPumpTemplateID', 'PumpTemplateID';
	exec sp_rename 'PumpTemplates.KPPumpTemplateID', 'PumpTemplateID';
	exec sp_rename 'PumpTemplates.PumpTemplateNumberNew', 'ConciseSpecificationSummary';
	exec sp_rename 'PumpTemplates.PumpTemplateNumber', 'VerboseSpecificationSummary';
	exec sp_rename 'PumpTemplates.TravelingValve', 'StandingValveCages';
	exec sp_rename 'PumpTemplates.Cages', 'TravellingCages';
	exec sp_rename 'TemplatePartDefs.KPTemplatePartsJoinID', 'TemplatePartDefID';
	exec sp_rename 'TemplatePartDefs.KFPartsID', 'PartTemplateID';
	exec sp_rename 'TemplatePartDefs.KFPumpTemplateID', 'PumpTemplateID';
	exec sp_rename 'Wells.KPWellLocationID', 'WellID';
	exec sp_rename 'Wells.KFLeaseID', 'LeaseID';
	exec sp_rename 'Wells.KFCustomerID', 'CustomerID';
COMMIT TRANSACTION

BEGIN TRANSACTION
	exec sp_rename '[FK_dbo.tblAcePumpProfiles_dbo.tblCustomers_CustomerID]', 'FK_dbo.AcePumpProfiles_dbo.Customers_CustomerID';
	exec sp_rename '[FK_dbo.tblAcePumpProfiles_dbo.tblUsers_UserID]', 'FK_dbo.AcePumpProfiles_dbo.Users_UserID';
	exec sp_rename '[PK_tblAcePumpProfiles]', 'PK_dbo.AcePumpProfiles';
	exec sp_rename '[PK_dbo.tblApiTokens]', 'PK_dbo.ApiTokens';
	exec sp_rename '[FK_dbo.tblApiTokens_dbo.tblUsers_UserID]', 'FK_dbo.ApiTokens_dbo.Users_UserID';
	exec sp_rename '[FK_dbo.tblAssemblies_dbo.tblPartsCategory_KFCategoryID]', 'FK_dbo.Assemblies_dbo.PartCategories_PartCategoryID';
	exec sp_rename '[PK_dbo.tblAssemblies]', 'PK_dbo.Assemblies';
	exec sp_rename 'Assemblies.IX_KFCategoryID', 'IX_PartCategoryID', 'INDEX';

	exec sp_rename '[PK_dbo.tblPartsAssemblies]', 'PK_dbo.AssemblyPartDefs';
	exec sp_rename '[FK_dbo.tblPartsAssemblies_dbo.tblAssemblies_KFAssemblyID]', 'FK_dbo.AssemblyPartDefs_dbo.Assemblies_AssemblyID';
	exec sp_rename '[FK_dbo.tblPartsAssemblies_dbo.tblParts_KFPartsID]', 'FK_dbo.AssemblyPartDefs_dbo.PartTemplates_PartTemplateID';
	exec sp_rename 'AssemblyPartDefs.IX_KFAssemblyID', 'IX_AssemblyID', 'INDEX';
	exec sp_rename 'AssemblyPartDefs.IX_KFPartsID', 'IX_PartTemplateID', 'INDEX';

	exec sp_rename '[PK_dbo.tblCountySalesTaxRates]', 'PK_dbo.CountySalesTaxRates';

	exec sp_rename '[PK_dbo.tblCustomerPartSpecials]', 'PK_dbo.CustomerPartSpecials';
	exec sp_rename '[FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID]', 'FK_dbo.CustomerPartSpecials_dbo.Customers_CustomerID';
	exec sp_rename '[FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID]', 'FK_dbo.CustomerPartSpecials_dbo.PartTemplates_PartTemplateID';	
	exec sp_rename 'CustomerPartSpecials.IX_PartID', 'IX_PartTemplateID', 'INDEX';

	exec sp_rename '[PK_dbo.tblCustomers]', 'PK_dbo.Customers';
	exec sp_rename '[FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID]', 'FK_dbo.Customers_dbo.CountySalesTaxRates_CountySalesTaxRateID';
	exec sp_rename '[FK_dbo.tblCustomers_dbo.tblQbInvoiceClasses_QbInvoiceClassID]', 'FK_dbo.Customers_dbo.QbInvoiceClasses_QbInvoiceClassID';	

	exec sp_rename '[PK_dbo.tblDeliveryTicketImageUploads]', 'PK_dbo.DeliveryTicketImageUploads';
	exec sp_rename '[FK_dbo.tblDeliveryTicketImageUploads_dbo.tblShopTickets_DeliveryTicketID]', 'FK_dbo.DeliveryTicketImageUploads_dbo.DeliveryTickets_DeliveryTicketID';

	exec sp_rename '[PK_dbo.tblShopTickets]', 'PK_dbo.DeliveryTickets';
	exec sp_rename '[FK_dbo.tblShopTickets_dbo.tblCountySalesTaxRates_CountySalesTaxRateID]', 'FK_dbo.DeliveryTickets_dbo.CountySalesTaxRates_CountySalesTaxRateID';
	exec sp_rename '[FK_dbo.tblShopTickets_dbo.tblCustomers_KFCustomerID]', 'FK_dbo.DeliveryTickets_dbo.Customers_CustomerID';
	exec sp_rename '[FK_dbo.tblShopTickets_dbo.tblPumps_PumpDispatchedID]', 'FK_dbo.DeliveryTickets_dbo.Pumps_PumpDispatchedID';
	exec sp_rename '[FK_dbo.tblShopTickets_dbo.tblPumps_PumpFailedID]', 'FK_dbo.DeliveryTickets_dbo.Pumps_PumpFailedID';
	exec sp_rename '[FK_dbo.tblShopTickets_dbo.tblWellLocation_KFWellLocationID]', 'FK_dbo.DeliveryTickets_dbo.Wells_WellID';
	exec sp_rename 'DeliveryTickets.IX_KFCustomerID', 'IX_CustomerID', 'INDEX';
	exec sp_rename 'DeliveryTickets.IX_KFWellLocationID', 'IX_WellID', 'INDEX';

	exec sp_rename '[PK_dbo.tblLeaseLocations]', 'PK_dbo.Leases';

	exec sp_rename '[PK_dbo.tblLineItems]', 'PK_dbo.LineItems';
	exec sp_rename '[FK_dbo.tblLineItems_dbo.tblRepairTickets_KFRepairTicketID]', 'FK_dbo.LineItems_dbo.PartInspections_PartInspectionID';
	exec sp_rename '[FK_dbo.tblLineItems_dbo.tblShopTickets_KFShopTicketID]', 'FK_dbo.LineItems_dbo.DeliveryTickets_DeliveryTicketID';
	exec sp_rename '[FK_dbo.tblLineItems_dbo.tblParts_KFPartID]', 'FK_dbo.LineItems_dbo.PartTemplates_PartTemplateID';
	exec sp_rename 'LineItems.IX_KFPartID', 'IX_PartTemplateID', 'INDEX';
	exec sp_rename 'LineItems.IX_KFRepairTicketID', 'IX_PartInspectionID', 'INDEX';
	exec sp_rename 'LineItems.IX_KFShopTicketID', 'IX_DeliveryTicketID', 'INDEX';

	exec sp_rename '[PK_dbo.tblMaterials]', 'PK_dbo.Materials';
	exec sp_rename '[PK_dbo.tblPartsCategory]', 'PK_dbo.PartCategories';

	exec sp_rename '[PK_dbo.tblRepairTickets]', 'PK_dbo.PartInspections';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID]', 'FK_dbo.PartInspections_dbo.Parts_ReplacedWithInventoryPartID';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblParts_KFPartFailedID]', 'FK_dbo.PartInspections_dbo.PartTemplates_PartFailedID';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblParts_KFPartReplacedID]', 'FK_dbo.PartInspections_dbo.PartTemplates_PartReplacedID';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblRepairTickets_KFParentAssemblyID]', 'FK_dbo.PartInspections_dbo.PartInspections_ParentAssemblyID';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblShopTickets_KFShopTicketID]', 'FK_dbo.PartInspections_dbo.DeliveryTickets_DeliveryTicketID';
	exec sp_rename '[FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID]', 'FK_dbo.PartInspections_dbo.TemplatePartDefs_TemplatePartDefID';
	exec sp_rename '[FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID]', 'FK_dbo.Parts_dbo.PartTemplates_PartTemplateID';
	exec sp_rename 'PartInspections.IX_KFParentAssemblyID', 'IX_ParentAssemblyID', 'INDEX';
	exec sp_rename 'PartInspections.IX_KFPartFailedID', 'IX_PartFailedID', 'INDEX';
	exec sp_rename 'PartInspections.IX_KFPartReplacedID', 'IX_PartReplacedID', 'INDEX';
	exec sp_rename 'PartInspections.IX_KFShopTicketID', 'IX_DeliveryTicketID', 'INDEX';

	exec sp_rename '[PK_dbo.tblPartRuntimes]', 'PK_dbo.PartRuntimes';
	exec sp_rename '[FK_dbo.tblPartRuntimes_dbo.tblPumps_PumpID]', 'FK_dbo.PartRuntimes_dbo.Pumps_PumpID';
	exec sp_rename '[FK_dbo.tblPartRuntimes_dbo.tblRepairTickets_RuntimeEndedByInspectionID]', 'FK_dbo.PartRuntimes_dbo.PartInspections_RuntimeEndedByInspectionID';
	exec sp_rename '[FK_dbo.tblPartRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID]', 'FK_dbo.PartRuntimes_dbo.DeliveryTickets_RuntimeStartedByTicketID';
	exec sp_rename '[FK_dbo.tblPartRuntimes_dbo.tblTemplatePartsJoin_TemplatePartDefID]', 'FK_dbo.PartRuntimes_dbo.TemplatePartDefs_TemplatePartDefID';

	exec sp_rename '[PK_dbo.tblPartRuntimeSegments]', 'PK_dbo.PartRuntimeSegments';
	exec sp_rename '[FK_dbo.tblPartRuntimeSegments_dbo.tblPartRuntimes_RuntimeID]', 'FK_dbo.PartRuntimeSegments_dbo.PartRuntimes_RuntimeID';
	exec sp_rename '[FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentEndedByTicketID]', 'FK_dbo.PartRuntimeSegments_dbo.DeliveryTickets_SegmentEndedByTicketID';
	exec sp_rename '[FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentStartedByTicketID]', 'FK_dbo.PartRuntimeSegments_dbo.DeliveryTickets_SegmentStartedByTicketID';

	exec sp_rename '[PK_dbo.tblPartInstances]', 'PK_dbo.Parts';
	exec sp_rename '[FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID]', 'FK_dbo.Parts_dbo.Customers_CustomerID';


	exec sp_rename '[PK_dbo.tblParts]', 'PK_dbo.PartTemplates';
	exec sp_rename '[FK_dbo.tblParts_dbo.tblAssemblies_KFAssemblyID]', 'FK_dbo.PartTemplates_dbo.Assemblies_RelatedAssemblyID';
	exec sp_rename '[FK_dbo.tblParts_dbo.tblMaterials_KFMaterialsID]', 'FK_dbo.PartTemplates_dbo.Materials_MaterialID';
	exec sp_rename '[FK_dbo.tblParts_dbo.tblPartsCategory_KFPartCategory]', 'FK_dbo.PartTemplates_dbo.PartCategories_PartCategoryID';
	exec sp_rename '[FK_dbo.tblParts_dbo.tblTypes_SoldByOption_KFOptionID]', 'FK_dbo.PartTemplates_dbo.Types_SoldByOption_SoldByOptionID';
	exec sp_rename 'PartTemplates.IX_KFAssemblyID', 'IX_RelatedAssemblyID', 'INDEX';
	exec sp_rename 'PartTemplates.IX_KFMaterialsID', 'IX_MaterialID', 'INDEX';
	exec sp_rename 'PartTemplates.IX_KFOptionID', 'IX_SoldByOptionID', 'INDEX';
	exec sp_rename 'PartTemplates.IX_KFPartCategory', 'IX_PartCategoryID', 'INDEX';

	exec sp_rename '[PK_dbo.tblPumpRuntimes]', 'PK_dbo.PumpRuntimes';
	exec sp_rename '[FK_dbo.tblPumpRuntimes_dbo.tblPumps_PumpID]', 'FK_dbo.PumpRuntimes_dbo.Pumps_PumpID';
	exec sp_rename '[FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeEndedByTicketID]', 'FK_dbo.PumpRuntimes_dbo.DeliveryTickets_RuntimeEndedByTicketID';
	exec sp_rename '[FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID]', 'FK_dbo.PumpRuntimes_dbo.DeliveryTickets_RuntimeStartedByTicketID';

	exec sp_rename '[PK_dbo.tblPumps]', 'PK_dbo.Pumps';
	exec sp_rename '[FK_dbo.tblPumps_dbo.tblPumpTemplates_KFPumpTemplateID]', 'FK_dbo.Pumps_dbo.PumpTemplates_PumpTemplateID';
	exec sp_rename '[FK_dbo.tblPumps_dbo.tblWellLocation_KFWellLocationID]', 'FK_dbo.Pumps_dbo.Wells_InstalledInWellID';
	exec sp_rename 'Pumps.IX_KFPumpTemplateID', 'IX_PumpTemplateID', 'INDEX';
	exec sp_rename 'Pumps.IX_KFWellLocationID', 'IX_InstalledInWellID', 'INDEX';

	exec sp_rename '[PK_dbo.tblPumpTemplates]', 'PK_dbo.PumpTemplates';
	exec sp_rename '[PK_dbo.tblQbInvoiceClasses]', 'PK_dbo.QbInvoiceClasses';
	exec sp_rename '[PK_dbo.tblRoles]', 'PK_dbo.Roles';

	exec sp_rename '[PK_dbo.tblTemplatePartsJoin]', 'PK_dbo.TemplatePartDefs';
	exec sp_rename '[FK_dbo.tblTemplatePartsJoin_dbo.tblParts_KFPartsID]', 'FK_dbo.TemplatePartDefs_dbo.PartTemplates_PartTemplateID';
	exec sp_rename '[FK_dbo.tblTemplatePartsJoin_dbo.tblPumpTemplates_KFPumpTemplateID]', 'FK_dbo.TemplatePartDefs_dbo.PumpTemplates_PumpTemplateID';
	exec sp_rename 'TemplatePartDefs.IX_KFPartsID', 'IX_PartTemplateID', 'INDEX';
	exec sp_rename 'TemplatePartDefs.IX_KFPumpTemplateID', 'IX_PumpTemplateID', 'INDEX';

	exec sp_rename '[PK_tblTypes_SoldByOption]', 'PK_dbo.Types_SoldByOption';

	exec sp_rename '[PK_dbo.tblUsernameCustomerAccess]', 'PK_dbo.UsernameCustomerAccess';
	exec sp_rename '[FK_dbo.tblUsernameCustomerAccess_dbo.tblAcePumpProfiles_UserID]', 'FK_dbo.UsernameCustomerAccess_dbo.AcePumpProfiles_UserID';
	exec sp_rename '[FK_dbo.tblUsernameCustomerAccess_dbo.tblCustomers_CustomerID]', 'FK_dbo.UsernameCustomerAccess_dbo.Customers_CustomerID';

	exec sp_rename '[PK_dbo.tblUserRoles]', 'PK_dbo.UserRoles';
	exec sp_rename '[FK_dbo.tblUserRoles_dbo.tblRoles_RoleID]', 'FK_dbo.UserRoles_dbo.Roles_RoleID';
	exec sp_rename '[FK_dbo.tblUserRoles_dbo.tblUsers_UserID]', 'FK_dbo.UserRoles_dbo.Users_UserID';

	exec sp_rename '[PK_dbo.tblUsers]', 'PK_dbo.Users';

	exec sp_rename '[PK_dbo.tblWellLocation]', 'PK_dbo.Wells';
	exec sp_rename '[FK_dbo.tblWellLocation_dbo.tblCustomers_KFCustomerID]', 'FK_dbo.Wells_dbo.Customers_CustomerID';
	exec sp_rename '[FK_dbo.tblWellLocation_dbo.tblLeaseLocations_KFLeaseID]', 'FK_dbo.Wells_dbo.Leases_LeaseID';
	exec sp_rename 'Wells.IX_KFCustomerID', 'IX_CustomerID', 'INDEX';
	exec sp_rename 'Wells.IX_KFLeaseID', 'IX_LeaseID', 'INDEX';

COMMIT TRANSACTION