-- CW Case 2121, Add RepairMode to Delivery Tickets. Does not allow nulls, default is 0.

IF dbo.fnColumnExists('tblShopTickets', 'RepairMode') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD RepairMode INT NULL;
   EXEC('UPDATE tblShopTickets SET RepairMode = 0');
   EXEC('ALTER TABLE tblShopTickets ALTER COLUMN RepairMode INT NOT NULL');
END
GO
