--MT case 1425 initialise taxable column in parts table with value according to prefix

IF dbo.fnColumnExists('tblParts', 'Taxable') = 0
BEGIN
	ALTER TABLE dbo.tblParts ADD Taxable BIT null;

	EXEC('Update dbo.tblParts '+
		'SET Taxable = ('+
			'SELECT  '+
				'CASE '+
					'WHEN PartNumber like ''SUB%'' THEN ''True'' '+
					'WHEN PartNumber like ''SUR%'' THEN ''True'' ' +
					'WHEN PartNumber like ''LRP%'' THEN ''True'' ' +
					'WHEN PartNumber like ''LR%'' THEN ''False'' ' +
					'WHEN PartNumber like ''SU%'' THEN ''False'' ' +
					'WHEN PartNumber like ''FREIGHT%'' THEN ''False'' '+
					'WHEN PartNumber like ''UPS IN%'' THEN ''False'' '+
					'WHEN PartNumber like ''UPS OUT%'' THEN ''False'' '+
					'ELSE ''True'' ' +
				'END ' +
				')')
	EXEC('ALTER TABLE dbo.tblParts ALTER COLUMN Taxable BIT not null;')
END
GO