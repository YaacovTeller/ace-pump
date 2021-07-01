-- CW: We don't need the the following columns anymore: PumpNumberSort, PumpNumberTwo, VendorLocation

IF dbo.fnConstraintExists('FK_tblPumps_tblVendorLocations') = 1
BEGIN
	ALTER TABLE tblPumps DROP CONSTRAINT [FK_tblPumps_tblVendorLocations];
END
GO

IF dbo.fnConstraintExists('FK_dbo.tblPumps_dbo.tblVendorLocations_KFVendorLocationID') = 1
BEGIN
	ALTER TABLE tblPumps DROP CONSTRAINT [FK_dbo.tblPumps_dbo.tblVendorLocations_KFVendorLocationID];
END
GO

IF dbo.fnIndexExists('IX_KFVendorLocationID', 'dbo.tblPumps') = 1
BEGIN
	DROP INDEX dbo.tblPumps.IX_KFVendorLocationID;
END
GO

IF dbo.fnColumnExists('tblPumps', 'KFVendorLocationID') = 1
BEGIN
       ALTER TABLE tblPumps DROP COLUMN KFVendorLocationID
END
GO

IF dbo.fnColumnExists('tblPumps', 'PumpNumberSort') = 1
BEGIN
       ALTER TABLE tblPumps DROP COLUMN PumpNumberSort
END
GO

IF dbo.fnColumnExists('tblPumps', 'PumpNumberTwo') = 1
BEGIN
       ALTER TABLE tblPumps DROP COLUMN PumpNumberTwo
END
GO