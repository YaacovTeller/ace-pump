-- CW Case 2136, Create RepairComplete on shoptickets. Make sure to first make a backup of the repair tickets like so: SELECT KPRepairID, RepairComplete from tblRepairTickets

IF dbo.fnColumnExists('tblShopTickets', 'RepairComplete') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD RepairComplete bit NULL;
   EXEC('UPDATE tblShopTickets SET RepairComplete = 0');   
   EXEC('UPDATE a set a.RepairComplete = 1 FROM tblShopTickets a WHERE EXISTS (SELECT 1 FROM tblRepairTickets AS b WHERE (a.KPShopTicketID = b.KFShopTicketID) AND (b.Complete IS NOT NULL) AND (1 = b.Complete))');  
   EXEC('ALTER TABLE tblShopTickets ALTER COLUMN RepairComplete bit NOT NULL');
   EXEC('ALTER TABLE tblRepairTickets DROP COLUMN Complete');
END
GO
