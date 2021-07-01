-- CW Case 1784, Create UsesQuickbooksRunningInvoice column with False as default value. 

IF dbo.fnColumnExists('tblCustomers', 'UsesQuickbooksRunningInvoice') = 0
BEGIN
   ALTER TABLE tblCustomers ADD UsesQuickbooksRunningInvoice BIT NULL;
   EXEC('UPDATE tblCustomers SET UsesQuickbooksRunningInvoice = ''False''');  
   EXEC('ALTER TABLE tblCustomers ALTER COLUMN UsesQuickbooksRunningInvoice BIT NOT NULL');
END
GO