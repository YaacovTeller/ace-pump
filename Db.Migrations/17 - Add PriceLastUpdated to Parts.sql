-- CW Case 2127, Create PriceLastUpdated column with today as default value.

IF dbo.fnColumnExists('tblParts', 'PriceLastUpdated') = 0
BEGIN
   ALTER TABLE tblParts ADD PriceLastUpdated datetime null;
   EXEC('UPDATE tblParts SET PriceLastUpdated = GETDATE()');
   EXEC('ALTER TABLE tblParts ALTER COLUMN PriceLastUpdated datetime not null');
END
GO