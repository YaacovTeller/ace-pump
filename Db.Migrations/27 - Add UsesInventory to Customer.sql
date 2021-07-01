-- CW Case 2260 Default is false

IF dbo.fnColumnExists('tblCustomers', 'UsesInventory') = 0
BEGIN
   ALTER TABLE tblCustomers ADD UsesInventory bit NULL;
   EXEC('UPDATE tblCustomers SET UsesInventory = 0');
   EXEC('ALTER TABLE tblCustomers ALTER COLUMN UsesInventory bit NOT NULL');
END
GO
