--CW Case 2148 Script Date: 6/12/2017 9:07 PM  -
IF dbo.fnTableExists('tblPartInstances') = 0
BEGIN
	CREATE TABLE [tblPartInstances] (
		  [PartID] int IDENTITY (1,1) NOT NULL
		, [PartTemplateID] int NOT NULL
		, [CustomerID] int NOT NULL
	);
	ALTER TABLE [tblPartInstances] ADD CONSTRAINT [PK_dbo.tblPartInstances] PRIMARY KEY ([PartID]);
	CREATE INDEX [IX_CustomerID] ON [tblPartInstances] ([CustomerID] ASC);
	CREATE INDEX [IX_PartTemplateID] ON [tblPartInstances] ([PartTemplateID] ASC);
	ALTER TABLE [tblPartInstances] ADD CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [tblCustomers]([KPCustomerID]) ON DELETE CASCADE ON UPDATE NO ACTION;
	ALTER TABLE [tblPartInstances] ADD CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID] FOREIGN KEY ([PartTemplateID]) REFERENCES [tblParts]([KPPartID]) ON DELETE CASCADE ON UPDATE NO ACTION;
END
GO
