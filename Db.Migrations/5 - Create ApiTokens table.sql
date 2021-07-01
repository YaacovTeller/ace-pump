-- CW Case 1267, Create ApiTokens table

IF dbo.fnTableExists('tblApiTokens') = 0
BEGIN
	CREATE TABLE [tblApiTokens] (
	  [ApiTokenID] int IDENTITY (1,1) NOT NULL
	, [UserID] int NOT NULL
	, [AuthToken] nvarchar(4000) NULL
	, [IssuedOn] datetime NOT NULL
	, [ExpiresOn] datetime NOT NULL
	);	
	ALTER TABLE [tblApiTokens] ADD CONSTRAINT [PK_dbo.tblApiTokens] PRIMARY KEY ([ApiTokenID]);
	CREATE INDEX [IX_UserID] ON [tblApiTokens] ([UserID] ASC);
	ALTER TABLE [tblApiTokens] ADD CONSTRAINT [FK_dbo.tblApiTokens_dbo.tblUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [tblUsers]([UserID]) ON DELETE CASCADE ON UPDATE NO ACTION;
END
GO

