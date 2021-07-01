-- CW Case 2136, Create ReplacedWithInventoryPartID on inspections references part used from Inventory (PartInstance). 

IF dbo.fnColumnExists('tblRepairTickets', 'ReplacedWithInventoryPartID') = 0
BEGIN
   ALTER TABLE tblRepairTickets ADD ReplacedWithInventoryPartID int NULL;

   EXEC('CREATE INDEX [IX_ReplacedWithInventoryPartID] ON [tblRepairTickets] ([ReplacedWithInventoryPartID] ASC)');
   EXEC('ALTER TABLE [tblRepairTickets] ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID] FOREIGN KEY ([ReplacedWithInventoryPartID]) REFERENCES [tblPartInstances]([PartID]) ON DELETE NO ACTION ON UPDATE NO ACTION');
END
GO

IF dbo.fnColumnExists('tblRepairTickets', 'TemplatePartDefID') = 0
BEGIN
   DECLARE @NewTemplatePartDefID int;  
   DECLARE @castID varchar(20);
    
   ALTER TABLE tblRepairTickets ADD TemplatePartDefID int NULL;
   INSERT INTO tblTemplatePartsJoin (KFPartsID, KFPumpTemplateID) VALUES (null, null);
   SET @NewTemplatePartDefID = SCOPE_IDENTITY();

   SET @castID = CONVERT(varchar(20), @NewTemplatePartDefID)
   EXEC('UPDATE tblRepairTickets SET TemplatePartDefID = ' + @castID );   
   
   EXEC('ALTER TABLE tblRepairTickets ALTER COLUMN TemplatePartDefID INT NOT NULL');

   EXEC('CREATE INDEX [IX_TemplatePartDefID] ON [tblRepairTickets] ([TemplatePartDefID] ASC)');
   EXEC('ALTER TABLE [tblRepairTickets] ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID] FOREIGN KEY ([TemplatePartDefID]) REFERENCES [tblTemplatePartsJoin]([KPTemplatePartsJoinID]) ON DELETE CASCADE ON UPDATE NO ACTION');      
END
GO