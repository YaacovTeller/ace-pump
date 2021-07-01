-- CW Case 1869, Create QbInvoiceClasses table

IF dbo.fnTableExists('tblQbInvoiceClasses') = 0
BEGIN
	CREATE TABLE [tblQbInvoiceClasses] (
	  [QbInvoiceClassID] int IDENTITY (1,1) NOT NULL
	, [FullName] nvarchar(max) NOT NULL
	);
	ALTER TABLE [tblQbInvoiceClasses] ADD CONSTRAINT [PK_dbo.tblQbInvoiceClasses] PRIMARY KEY ([QbInvoiceClassID]);

	EXEC('INSERT INTO tblQbInvoiceClasses (FullName) VALUES (''BK-PUMP SHOP''), (''SM-PUMP SHOP'')');
END
GO

IF dbo.fnColumnExists('tblCustomers', 'QbInvoiceClassID') = 0
BEGIN
   ALTER TABLE tblCustomers ADD QbInvoiceClassID INT NULL;
   
   EXEC('UPDATE tblCustomers SET QbInvoiceClassID = 1 WHERE QbInvoiceClassID IS NULL AND CustomerName LIKE ''%SENECA%''');
   EXEC('UPDATE tblCustomers SET QbInvoiceClassID = 2 WHERE QbInvoiceClassID IS NULL AND (CustomerName NOT LIKE ''%SENECA%'' OR CustomerName IS NULL)');
   EXEC('ALTER TABLE tblCustomers ALTER COLUMN QbInvoiceClassID INT NOT NULL');
   EXEC('ALTER TABLE tblCustomers ADD CONSTRAINT FK_tblCustomers_tblQbInvoiceClasses_QbInvoiceClassID FOREIGN KEY (QbInvoiceClassID) REFERENCES tblQbInvoiceClasses (QbInvoiceClassID)');
END
GO



