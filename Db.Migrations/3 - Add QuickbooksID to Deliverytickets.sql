-- CW Case 1468, Create QuickbooksID column with "0" as default value.

IF dbo.fnColumnExists('tblShopTickets', 'QuickbooksID') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD QuickbooksID int null;
   EXEC('UPDATE tblShopTickets SET QuickbooksID = ''0''');
   EXEC('ALTER TABLE tblShopTickets ALTER COLUMN QuickbooksID int not null');
END
GO