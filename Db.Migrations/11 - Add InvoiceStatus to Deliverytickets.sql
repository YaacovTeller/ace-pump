-- CW Case 1801, Create InvoiceStatus column with "0" as default value. All

IF dbo.fnColumnExists('tblShopTickets', 'InvoiceStatus') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD InvoiceStatus INT NULL;
   EXEC('UPDATE tblShopTickets SET InvoiceStatus = 0 WHERE QuickbooksID IS NULL OR QuickbooksID =''''');
   EXEC('UPDATE tblShopTickets SET InvoiceStatus = 4 WHERE QuickbooksID IS NOT NULL AND QuickbooksID <> ''''');
   EXEC('ALTER TABLE tblShopTickets ALTER COLUMN InvoiceStatus INT NOT NULL');
END
GO