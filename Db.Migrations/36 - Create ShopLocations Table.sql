IF dbo.fnTableExists('ShopLocations') = 0
BEGIN
	CREATE TABLE [dbo].[ShopLocations](
		[ShopLocationID] [int] IDENTITY(1,1) NOT NULL,
		[Prefix] [nvarchar](max) NULL,
		[Name] [nvarchar](max) NULL,
		[StartingPumpNumber] [int] NOT NULL,
	 CONSTRAINT [PK_dbo.ShopLocations] PRIMARY KEY CLUSTERED 
	(
		[ShopLocationID] ASC
	)
	)
END
GO

/*this is for acepump which already has the location column. Clear it!
Some things to consider:
- there 1 pumps in the main db where the pumpnumber is null
- SELECT * FROM dbo.Pumps WHERE pumpnumber is null
pumpID=6475
pumptemplateID = 1165
I don't see a dt connected to it

*/
IF dbo.fnColumnExists('Pumps', 'ShopLocationID') = 1
BEGIN
	ALTER TABLE dbo.Pumps DROP ShopLocationID;
END
/*-------------------------------------*/

IF dbo.fnColumnExists('Pumps', 'ShopLocationID') = 0
BEGIN
	ALTER TABLE dbo.Pumps ADD ShopLocationID INT null;
END
GO

INSERT INTO [dbo].[ShopLocations] ([Prefix] ,[Name] ,[StartingPumpNumber]) VALUES ('SM', 'Santa Maria' ,'1000')
INSERT INTO [dbo].[ShopLocations] ([Prefix] ,[Name] ,[StartingPumpNumber]) VALUES ('B', 'Bakersfield' ,'3000')
INSERT INTO [dbo].[ShopLocations] ([Prefix] ,[Name] ,[StartingPumpNumber]) VALUES ('V', 'Ventura' ,'100')
 /*
Since they started manually adding a prefix to pump numbers, we have to  make sure to treat them carefully.
There are 614 pumps like this.
*/
SELECT * FROM dbo.Pumps WHERE TRY_CAST(pumpnumber as int) is null

select * from dbo.pumps where  LOWER(LEFT(pumpnumber, 1)) = 'v'
UPDATE dbo.Pumps set ShopLocationID = (select ShopLocationID from dbo.ShopLocations where Prefix = N'V') 
 where  LOWER(LEFT(pumpnumber, 1)) = 'v'

select * from dbo.pumps where  LOWER(LEFT(pumpnumber, 1)) = 'b'
UPDATE dbo.Pumps set ShopLocationID = (select ShopLocationID from dbo.ShopLocations where Prefix = N'B') 
 where  LOWER(LEFT(pumpnumber, 1)) = 'b'
/*
NOW we have to do all the shoplocation and pumpnumber changes in 
the main db, before we add the not null and the constraints, indexes
*/
UPDATE dbo.Pumps set ShopLocationID = (select ShopLocationID from dbo.ShopLocations where Prefix = N'SM') 
 where  TRY_CAST(pumpnumber AS INT ) < 3000

UPDATE dbo.Pumps set ShopLocationID = (select ShopLocationID from dbo.ShopLocations where Prefix = N'B') 
WHERE TRY_CAST(pumpnumber AS INT ) > 2999 
--here remove prefix from all the ones already entered in the db (there were B and V pumps)
update pumps set pumpnumber = right(pumpnumber, len(pumpnumber) -1)  WHERE TRY_CAST(pumpnumber as int) is null 

/* Now continue with setup */
ALTER TABLE dbo.Pumps ALTER COLUMN PumpNumber nvarchar(446)
/* Might need to drop index */
DROP INDEX [IX_ShopLocationID] ON [dbo].[Pumps]
ALTER TABLE dbo.Pumps ALTER COLUMN ShopLocationID int not null
/*Might already have constraing */
ALTER TABLE dbo.Pumps ADD CONSTRAINT [FK_dbo.Pumps_dbo.ShopLocations_ShopLocationID] FOREIGN KEY ([ShopLocationID]) REFERENCES ShopLocations([ShopLocationID]) ON DELETE CASCADE ON UPDATE NO ACTION
/*Need to recreate index if dropped it */
CREATE NONCLUSTERED INDEX [IX_ShopLocationID] ON [dbo].[Pumps]
([ShopLocationID] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE UNIQUE NONCLUSTERED INDEX [UQ_PumpNumberPerShopLocation] ON [dbo].[Pumps]([PumpNumber] ASC,[ShopLocationID] ASC)

select pumpnumber, shoplocationID  from pumps group by pumpnumber, shoplocationid having count(*) > 1







