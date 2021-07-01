-- USERS
-- SQL Server
IF NOT EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'AcePump_IUsr')
BEGIN
	CREATE LOGIN [AcePump_IUsr] WITH PASSWORD = N'AcePump';
    CREATE USER [AcePump_IUsr] FOR LOGIN [AcePump_IUsr];
	EXECUTE sp_addrolemember [db_datareader], [AcePump_IUsr];
	EXECUTE sp_addrolemember [db_datawriter], [AcePump_IUsr];
END

-- USER FUNCTIONS
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (
		   SELECT *
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[dbo].[fnUdfExists]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
BEGIN
	DROP FUNCTION [dbo].[fnUdfExists]
END
GO

CREATE FUNCTION [dbo].[fnUdfExists]
(
	@name nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = (case when count(*) > 0 then 1 else 0 end)
	FROM   sys.objects
	WHERE  object_id = OBJECT_ID(N'[dbo].[' + @name + ']')
		AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' )

	RETURN @exists
END
GO

IF dbo.fnUdfExists('fnTableExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnTableExists]
END
GO

CREATE FUNCTION [dbo].[fnTableExists]
(
	@name nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND  TABLE_NAME = @name

	RETURN @exists
END
GO


IF dbo.fnUdfExists('fnColumnExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnColumnExists]
END
GO

CREATE FUNCTION [dbo].[fnColumnExists]
(
	@table nvarchar(50),
	@column nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM sys.columns 
    WHERE Name      = @column
      AND Object_ID = Object_ID(@table)

	RETURN @exists
END
GO


IF dbo.fnUdfExists('fnConstraintExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnConstraintExists]
END
GO

CREATE FUNCTION [dbo].[fnConstraintExists]
(
	@name nvarchar(100)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_NAME=@name

	RETURN @exists
END
GO

IF dbo.fnUdfExists('fnIndexExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnIndexExists]
END
GO

CREATE FUNCTION [dbo].[fnIndexExists]
(
	@name nvarchar(100),
	@tableName nvarchar(100)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM SYS.INDEXES
	WHERE NAME=@name AND object_id = OBJECT_ID(@tableName)    

	RETURN @exists
END
GO

-- =============================================
-- Author:		YY Kosbie
-- Create date: 3 Dec 2015
-- Description:	Rounds off and casts to a decimal(10, 4) for CLR support.  This function only supports
--              decimals up to decimal(28,10).  If the decimal you pass in has a greater precision or
--              scale, this funciton throws an error.
-- =============================================
IF dbo.fnUdfExists('ClrRound_10_4') = 1
BEGIN
	DROP FUNCTION [dbo].[ClrRound_10_4]
END
GO

CREATE FUNCTION [dbo].[ClrRound_10_4]
(
	@value decimal(28, 10)
)
RETURNS decimal(10,4)
AS
BEGIN
	DECLARE @converted decimal(10,4)

	SELECT @converted = cast(round(@value, 4) as decimal(10,4))

	RETURN @converted

END
GO

GRANT EXEC ON dbo.ClrRound_10_4 TO AcePump_IUsr
GO

-- TABLES
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF dbo.fnTableExists('tblAcePumpProfiles') = 0
BEGIN
	CREATE TABLE [dbo].[tblAcePumpProfiles](
		[UserID] [int] NOT NULL,
		[CustomerID] [int] NULL,
	 CONSTRAINT [PK_tblAcePumpProfiles] PRIMARY KEY CLUSTERED 
	(
		[UserID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblAssemblies') = 0
BEGIN
	CREATE TABLE [dbo].[tblAssemblies](
		[KPAssemblyID] [int] IDENTITY(1,1) NOT NULL,
		[KFCategoryID] [int] NULL,
		[AssemblyNumber] [nvarchar](50) NULL,
		[AssemblyDescription] [nvarchar](255) NULL,
		[Discount] [decimal](5, 4) NULL,
		[Markup] [decimal](5, 4) NULL,
	 CONSTRAINT [PK_tblPartsAssemblies] PRIMARY KEY CLUSTERED 
	(
		[KPAssemblyID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblCountySalesTaxRates') = 0
BEGIN
	CREATE TABLE [dbo].[tblCountySalesTaxRates](
		[CountySalesTaxRateID] [int] IDENTITY(1,1) NOT NULL,
		[CountyName] [nvarchar](4000) NULL,
		[SalesTaxRate] [numeric](6, 5) NOT NULL,
		[QuickbooksID] [nvarchar](max) NULL,
	 CONSTRAINT [PK_dbo.tblCountySalesTaxRates] PRIMARY KEY CLUSTERED 
	(
		[CountySalesTaxRateID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblCustomerContacts') = 0
BEGIN
	CREATE TABLE [dbo].[tblCustomerContacts](
		[KPContactID] [int] IDENTITY(1,1) NOT NULL,
		[KFCustomerID] [int] NULL,
		[FirstName] [nvarchar](255) NULL,
		[LastName] [nvarchar](255) NULL,
		[Phone1] [nvarchar](255) NULL,
		[Phone2] [nvarchar](255) NULL,
		[Email] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[upsizeTS] [timestamp] NULL,
	 CONSTRAINT [aaaaatblCustomerContacts_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPContactID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblCustomerPartSpecials') = 0
BEGIN
	CREATE TABLE [dbo].[tblCustomerPartSpecials](
		[CustomerPartSpecialID] [int] IDENTITY(1,1) NOT NULL,
		[CustomerID] [int] NOT NULL,
		[PartID] [int] NOT NULL,
		[Discount_Original_Float] [float] NULL,
		[Discount] [decimal](5, 4) NOT NULL,
	 CONSTRAINT [PK_dbo.tblCustomerPartSpecials] PRIMARY KEY CLUSTERED 
	(
		[CustomerPartSpecialID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblCustomers') = 0
BEGIN
	CREATE TABLE [dbo].[tblCustomers](
		[KPCustomerID] [int] IDENTITY(1,1) NOT NULL,
		[KFVendorID] [int] NULL,
		[CustomerName] [nvarchar](255) NULL,
		[Address1] [nvarchar](255) NULL,
		[Address2] [nvarchar](255) NULL,
		[City] [nvarchar](255) NULL,
		[State] [nvarchar](255) NULL,
		[Zip] [nvarchar](255) NULL,
		[Website] [nvarchar](255) NULL,
		[CustomerLogo] [nvarchar](255) NULL,
		[Phone] [nvarchar](255) NULL,
		[LogoBinary] [varbinary](max) NULL,
		[LogoText] [nvarchar](50) NULL,
		[Billingemail] [nvarchar](50) NULL,
		[APINumberRequired] [bit] NOT NULL CONSTRAINT [DF_tblCustomers_APINumberRequired]  DEFAULT ((0)),
		[DefaultSalesTaxRate] [decimal](6, 5) NULL,
		[CountySalesTaxRateID] [int] NULL,
		[QuickbooksID] [nvarchar](4000) NULL,
	 CONSTRAINT [aaaaatblCustomers_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPCustomerID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblDeliveryTicketImageUploads') = 0
BEGIN
	CREATE TABLE [dbo].[tblDeliveryTicketImageUploads](
		[DeliveryTicketImageUploadID] [int] IDENTITY(1,1) NOT NULL,
		[DeliveryTicketID] [int] NOT NULL,
		[LargeImageName] [nvarchar](4000) NULL,
		[SmallImageName] [nvarchar](4000) NULL,
		[AppRelativePath] [nvarchar](4000) NULL,
		[MimeType] [nvarchar](4000) NULL,
		[UploadedOn] [datetime] NOT NULL,
		[UploadedBy] [nvarchar](4000) NULL,
		[Note] [nvarchar](4000) NULL,
	 CONSTRAINT [PK_tblDeliveryTicketImageUploads] PRIMARY KEY CLUSTERED 
	(
		[DeliveryTicketImageUploadID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblEmployees') = 0
BEGIN
	CREATE TABLE [dbo].[tblEmployees](
		[KPEmployees] [int] IDENTITY(1,1) NOT NULL,
		[KFVendorLocationsID] [int] NULL,
		[FirstName] [nvarchar](50) NULL,
		[LastName] [nvarchar](50) NULL,
	 CONSTRAINT [PK_tblEmployees] PRIMARY KEY CLUSTERED 
	(
		[KPEmployees] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblInvoices') = 0
BEGIN
	CREATE TABLE [dbo].[tblInvoices](
		[KPInvoiceID] [int] IDENTITY(1,1) NOT NULL,
		[KFShopTicketID] [int] NULL,
		[DateIssued] [date] NULL,
		[flagInvoiceEstimate] [nchar](10) NULL,
		[SalesTaxRate] [decimal](6, 5) NULL,
	 CONSTRAINT [aaaaatblInvoices_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPInvoiceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblLeaseLocations') = 0
BEGIN
	CREATE TABLE [dbo].[tblLeaseLocations](
		[KPLeaseID] [int] IDENTITY(1,1) NOT NULL,
		[LocationName] [nvarchar](50) NULL,
		[IgnoreInReporting] [bit] NULL,
	 CONSTRAINT [PK_tblWellLocations] PRIMARY KEY CLUSTERED 
	(
		[KPLeaseID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblLineItems') = 0
BEGIN
	CREATE TABLE [dbo].[tblLineItems](
		[KPLineItemID] [int] IDENTITY(1,1) NOT NULL,
		[KFShopTicketID] [int] NOT NULL,
		[KFPartID] [int] NULL,
		[KFRepairedBy] [int] NULL,
		[Description] [nvarchar](255) NULL,
		[UnitPrice_Old] [decimal](18, 0) NULL,
		[UnitDiscount] [decimal](5, 4) NULL,
		[ReasonRepaired] [nvarchar](50) NULL,
		[Comments] [nvarchar](max) NULL,
		[ApprovedRepair] [bit] NULL CONSTRAINT [DF_tblLineItems_ApprovedRepair]  DEFAULT ((0)),
		[SalesTax] [bit] NULL CONSTRAINT [DF_tblLineItems_SalesTax]  DEFAULT ((0)),
		[SalesTaxCity] [nchar](20) NULL,
		[SalesTaxRate] [decimal](5, 4) NULL,
		[SortOrder] [int] NULL,
		[RepairCode] [nchar](10) NULL,
		[PartLookup] [nvarchar](50) NULL,
		[UnitPrice] [decimal](18, 2) NULL,
		[KFRepairTicketID] [int] NULL,
		[Quantity] [decimal](18, 2) NULL,
		[CustomerDiscount] [decimal](7, 4) NULL,
	 CONSTRAINT [aaaaatblLineItems_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPLineItemID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblMaterials') = 0
BEGIN
	CREATE TABLE [dbo].[tblMaterials](
		[KPMaterialsID] [int] NOT NULL,
		[MaterialsName] [nchar](25) NULL,
	 CONSTRAINT [PK_Materials] PRIMARY KEY CLUSTERED 
	(
		[KPMaterialsID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPartRuntimes') = 0
BEGIN
	CREATE TABLE [dbo].[tblPartRuntimes](
		[PartRuntimeID] [int] IDENTITY(1,1) NOT NULL,
		[PumpID] [int] NOT NULL,
		[TemplatePartDefID] [int] NOT NULL,
		[Start] [date] NULL,
		[Finish] [date] NULL,
		[TotalDaysInGround] [int] NULL,
		[RuntimeStartedByTicketID] [int] NULL,
		[RuntimeEndedByInspectionID] [int] NULL,
		[CandidateRuntimeID] [int] NOT NULL,
	 CONSTRAINT [PK_tblPartRuntimes] PRIMARY KEY CLUSTERED 
	(
		[PartRuntimeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPartRuntimeSegments') = 0
BEGIN
	CREATE TABLE [dbo].[tblPartRuntimeSegments](
		[PartRuntimeSegmentID] [int] IDENTITY(1,1) NOT NULL,
		[RuntimeID] [int] NOT NULL,
		[Start] [date] NULL,
		[Finish] [date] NULL,
		[LengthInDays] [int] NULL,
		[SegmentStartedByTicketID] [int] NULL,
		[SegmentEndedByTicketID] [int] NULL,
	 CONSTRAINT [PK_tblPartRuntimeSegments] PRIMARY KEY CLUSTERED 
	(
		[PartRuntimeSegmentID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblParts') = 0
BEGIN
	CREATE TABLE [dbo].[tblParts](
		[KPPartID] [int] IDENTITY(1,1) NOT NULL,
		[KFPartCategory] [int] NULL,
		[KFMaterialsID] [int] NULL,
		[KFAssemblyID] [int] NULL,
		[KFVendorLocationID] [int] NULL,
		[KFOptionID] [int] NULL CONSTRAINT [DF__tblParts__QtyOpt__239E4DCF]  DEFAULT ((1)),
		[PartNumber] [nvarchar](255) NULL,
		[Tracked] [nchar](10) NULL,
		[Description] [nvarchar](max) NULL,
		[Cost] [float] NULL,
		[Markup] [float] NULL CONSTRAINT [DF__tblParts__Profit__24927208]  DEFAULT ((0)),
		[Discount] [float] NULL,
		[VendorName] [nvarchar](255) NULL,
		[DiscountPrice] [decimal](18, 4) NULL,
		[Active] [bit] NOT NULL CONSTRAINT [DF__tblParts__Active__25869641]  DEFAULT ((1)),
		[IsAssembly] [bit] NULL CONSTRAINT [DF_tblParts_IsAssembly]  DEFAULT ((0)),
		[CheckedCategory] [bit] NULL,
		[CheckedPrice] [bit] NULL,
		[CheckedSoldBy] [bit] NULL,
		[WeightPounds] [int] NULL,
		[WeightOunces] [int] NULL,
		[Manufacturer] [nvarchar](50) NULL,
		[ManufacturerPartNumber] [nvarchar](50) NULL,
		[ElectCategory] [bit] NULL,
		[FilterPart]  AS (left([PartNumber],(5))),
		[FilterAssembly] [nchar](10) NULL,
		[Discount_as_Decimal] [decimal](5, 4) NOT NULL,
		[Cost_As_Decimal] [decimal](10, 2) NOT NULL,
		[Markup_As_Decimal] [decimal](5, 4) NOT NULL,
		[QuickbooksID] [nvarchar](4000) NULL,
	 CONSTRAINT [aaaaatblParts_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPPartID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPartsAssemblies') = 0
BEGIN
	CREATE TABLE [dbo].[tblPartsAssemblies](
		[KPPartsAssembliesID] [int] IDENTITY(1,1) NOT NULL,
		[KFAssemblyID] [int] NULL,
		[KFPartsID] [int] NULL,
		[PartsQuantity] [int] NULL CONSTRAINT [DF_tblPartsAssemblies_PartsQuantity]  DEFAULT ((1)),
		[Filter] [nvarchar](100) NULL,
		[SortOrder] [int] NOT NULL CONSTRAINT [DF_tblPartsAssemblies_SortOrder]  DEFAULT ((0)),
	 CONSTRAINT [PK_tblPartsAssemblies_1] PRIMARY KEY CLUSTERED 
	(
		[KPPartsAssembliesID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPartsCategory') = 0
BEGIN
	CREATE TABLE [dbo].[tblPartsCategory](
		[KPCategoryID] [int] IDENTITY(1,1) NOT NULL,
		[CategoryName] [nvarchar](255) NULL,
		[CategoryDescription] [ntext] NULL,
		[upsizeTS] [timestamp] NULL,
	 CONSTRAINT [aaaaatblPartsCategory_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPCategoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPartsOptions') = 0
BEGIN
	CREATE TABLE [dbo].[tblPartsOptions](
		[KPOptionID] [int] IDENTITY(1,1) NOT NULL,
		[OptionValue] [nvarchar](255) NULL,
	 CONSTRAINT [aaaaatblPartsOptions_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPOptionID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpHistory') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpHistory](
		[KPPumpHistoryID] [int] IDENTITY(1,1) NOT NULL,
		[KFShopTicketID] [int] NULL,
		[KFPumpID] [int] NULL,
		[HistoryDate] [date] NULL,
		[HistoryType] [nchar](10) NULL,
	 CONSTRAINT [PK_tblPumpHistory] PRIMARY KEY CLUSTERED 
	(
		[KPPumpHistoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpRuntimes') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpRuntimes](
		[PumpRuntimeID] [int] IDENTITY(1,1) NOT NULL,
		[PumpID] [int] NOT NULL,
		[Start] [date] NULL,
		[Finish] [date] NULL,
		[LengthInDays] [int] NULL,
		[RuntimeStartedByTicketID] [int] NULL,
		[RuntimeEndedByTicketID] [int] NULL,
	 CONSTRAINT [PK_PumpRuntimes] PRIMARY KEY CLUSTERED 
	(
		[PumpRuntimeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumps') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumps](
		[KPPumpID] [int] IDENTITY(1,1) NOT NULL,
		[PumpNumber] [nvarchar](25) NULL,
		[KFWellLocationID] [int] NULL,
		[KFPumpTemplateID] [int] NULL,
		[KFVendorLocationID] [int] NULL,
		[KFCustomerID] [int] NULL,
		[PumpNumberSort] [int] NULL,
		[PumpNumberTwo] [int] NULL,
	 CONSTRAINT [aaaaatblPumps_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPPumpID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpSpecs') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpSpecs](
		[KPPumpSpecsID] [int] IDENTITY(1,1) NOT NULL,
		[KFPumpTemplateCategoryID] [int] NULL,
		[ShortCode] [nchar](3) NULL,
		[Description] [nchar](100) NULL,
	 CONSTRAINT [PK_tblPumpTemplateDefinitions] PRIMARY KEY CLUSTERED 
	(
		[KPPumpSpecsID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpTemplateCategories') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpTemplateCategories](
		[KPPumpTemplateCategoryID] [int] IDENTITY(1,1) NOT NULL,
		[CategoryName] [nchar](40) NULL,
	 CONSTRAINT [PK_tblPumpTemplateCategories] PRIMARY KEY CLUSTERED 
	(
		[KPPumpTemplateCategoryID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpTemplates') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpTemplates](
		[KPPumpTemplateID] [int] IDENTITY(1,1) NOT NULL,
		[KFPumpTemplatePartsID] [int] NULL,
		[KFVendorLocationID] [int] NULL,
		[PumpTemplateNumberNew] [nchar](150) NULL,
		[PumpTemplateNumber] [nchar](150) NULL,
		[PumpTemplateSpec] [nchar](100) NULL,
		[Discount] [decimal](5, 4) NULL,
		[Markup] [decimal](5, 4) NULL,
		[TubingSize] [nchar](20) NULL,
		[PumpBoreBasic] [nchar](20) NULL,
		[BarrelLength] [nchar](20) NULL,
		[LowerExtension] [nchar](20) NULL,
		[UpperExtension] [nchar](20) NULL,
		[PumpType] [nchar](20) NULL,
		[BarrelType] [nchar](40) NULL,
		[BarrelMaterial] [nchar](40) NULL,
		[SeatingLocation] [nchar](40) NULL,
		[SeatingType] [nchar](20) NULL,
		[PlungerMaterial] [nchar](20) NULL,
		[PlungerLength] [nchar](20) NULL,
		[PlungerFit] [nchar](20) NULL,
		[HoldDownType] [nchar](20) NULL,
		[TravelingValve] [nchar](40) NULL,
		[StandingValve] [nchar](40) NULL,
		[BallsAndSeats] [nchar](40) NULL,
		[Cages] [nchar](50) NULL,
		[BarrelWasher] [nchar](20) NULL,
		[Collet] [nchar](20) NULL,
		[TopSeals] [nchar](20) NULL,
		[OnOffTool] [nchar](20) NULL,
		[SpecialtyItems] [nchar](20) NULL,
		[PonyRods] [nchar](20) NULL,
		[Strainers] [nchar](20) NULL,
		[KnockOut] [nchar](20) NULL,
		[ShowPumpTemplate] [int] NULL,
	 CONSTRAINT [PK_tblPumpTemplates] PRIMARY KEY CLUSTERED 
	(
		[KPPumpTemplateID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblPumpTemplateSpecsJoin') = 0
BEGIN
	CREATE TABLE [dbo].[tblPumpTemplateSpecsJoin](
		[KPPumpTemplateSpecsJoin] [int] IDENTITY(1,1) NOT NULL,
		[KFPumpTemplateID] [int] NULL,
		[KFPumpSpecsID] [int] NULL,
	 CONSTRAINT [PK_tblPumpTemplateDefJoin] PRIMARY KEY CLUSTERED 
	(
		[KPPumpTemplateSpecsJoin] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblRepairTickets') = 0
BEGIN
	CREATE TABLE [dbo].[tblRepairTickets](
		[KPRepairID] [int] IDENTITY(1,1) NOT NULL,
		[KFShopTicketID] [int] NULL,
		[Quantity] [int] NULL,
		[Status] [nvarchar](10) NULL,
		[ReasonRepaired] [nvarchar](100) NULL,
		[TimeRepaired] [timestamp] NULL,
		[KFPartFailedID] [int] NULL,
		[Sort] [int] NULL,
		[KFPartReplacedID] [int] NULL,
		[ReplaceQuantity] [int] NULL,
		[Complete] [int] NULL,
		[IsAssembly] [int] NULL,
		[Notes] [nchar](200) NULL,
		[KFParentAssemblyID] [int] NULL,
		[FailedTemplatePartDefID] [int] NULL,
		[IsSplitAssembly] [bit] NULL,
		[IsConvertible] [bit] NULL,
	 CONSTRAINT [PK_dbo.tblRepairTickets] PRIMARY KEY CLUSTERED 
	(
		[KPRepairID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblRoles') = 0
BEGIN
	CREATE TABLE [dbo].[tblRoles](
		[RoleID] [int] IDENTITY(1,1) NOT NULL,
		[RoleName] [nvarchar](max) NULL,
		[DisplayText] [nvarchar](max) NULL,
	 CONSTRAINT [PK_tblRoles] PRIMARY KEY CLUSTERED 
	(
		[RoleID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblShopTickets') = 0
BEGIN
	CREATE TABLE [dbo].[tblShopTickets](
		[KPShopTicketID] [int] IDENTITY(1,1) NOT NULL,
		[KFCustomerID] [int] NULL,
		[KFWellLocationID] [int] NULL,
		[TicketDate] [date] NULL CONSTRAINT [DF_tblShopTickets_TicketDate]  DEFAULT (getdate()),
		[CloseTicket] [bit] NULL CONSTRAINT [DF_tblRepairTickets_OpenTicket]  DEFAULT ((0)),
		[ShipVia] [nvarchar](50) NULL,
		[PONumber] [nvarchar](50) NULL,
		[OrderDate] [date] NULL,
		[ShipDate] [date] NULL,
		[OrderedBy] [nvarchar](50) NULL,
		[LastPull] [nvarchar](50) NULL,
		[Stroke] [nvarchar](50) NULL,
		[KFPumpDispatched] [nchar](25) NULL,
		[KFPumpFailed] [nchar](25) NULL,
		[HoldDown] [nchar](25) NULL,
		[FilterWells] [nchar](20) NULL,
		[Notes] [nchar](200) NULL,
		[SortOrder] [int] NULL,
		[FilterAssembly] [int] NULL,
		[InvBarrel] [nchar](50) NULL,
		[InvSVCages] [nchar](50) NULL,
		[InvDVCages] [nchar](50) NULL,
		[InvSVSeats] [nchar](50) NULL,
		[InvDVSeats] [nchar](50) NULL,
		[InvSVBalls] [nchar](50) NULL,
		[InvDVBalls] [nchar](50) NULL,
		[InvHoldDown] [nchar](50) NULL,
		[InvValveRod] [nchar](50) NULL,
		[InvPlunger] [nchar](50) NULL,
		[InvPTVCages] [nchar](50) NULL,
		[InvPDVCages] [nchar](50) NULL,
		[InvPTVSeats] [nchar](50) NULL,
		[InvPDVSeats] [nchar](50) NULL,
		[InvPTVBalls] [nchar](50) NULL,
		[InvPDVBalls] [nchar](50) NULL,
		[InvRodGuide] [nchar](50) NULL,
		[InvTypeBallandSeat] [nchar](50) NULL,
		[KFVendorLocationID] [int] NULL,
		[CompletedBy] [nvarchar](max) NULL,
		[KFRepairTicketID] [int] NULL,
		[RepairedBy] [nvarchar](max) NULL,
		[RepairedDate] [date] NULL,
		[ReasonStillOpen] [nvarchar](max) NULL,
		[PumpFailedDate] [date] NULL,
		[PumpFailedID] [int] NULL,
		[PumpDispatchedDate] [date] NULL,
		[PumpDispatchedID] [int] NULL,
		[SignatureName] [nvarchar](max) NULL,
		[SignatureDate] [date] NULL,
		[Signature] [varbinary](max) NULL,
		[Quote] [bit] NULL,
		[SignatureCompanyName] [nvarchar](max) NULL,
		[IsSignificantDesignChange] [bit] NULL,
		[IsUserTypedRate] [bit] NOT NULL,
		[QuickbooksID] [nvarchar](4000) NULL,
		[QuickbooksInvoiceNumber] [nvarchar](4000) NULL,
		[CountySalesTaxRateID] [int] NULL,
	 CONSTRAINT [aaaaatblRepairTickets_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPShopTicketID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTemplatePartsJoin') = 0
BEGIN
	CREATE TABLE [dbo].[tblTemplatePartsJoin](
		[KPTemplatePartsJoinID] [int] IDENTITY(1,1) NOT NULL,
		[KFPartsID] [int] NULL,
		[KFPumpTemplateID] [int] NULL,
		[Quantity] [int] NULL,
		[SortOrder] [int] NULL,
		[NewSortOrder] [int] NULL,
	 CONSTRAINT [PK_tblPumpTemplateParts] PRIMARY KEY CLUSTERED 
	(
		[KPTemplatePartsJoinID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_BallsAndSeats') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_BallsAndSeats](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_BarrelLength') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_BarrelLength](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_BarrelMaterial') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_BarrelMaterial](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_BarrelType') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_BarrelType](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_BarrelWasher') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_BarrelWasher](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_Collet') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_Collet](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_HoldDownType') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_HoldDownType](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvBallsAndSeatsCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvBallsAndSeatsCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvBallsCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvBallsCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvBarrelCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvBarrelCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvCagesCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvCagesCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvHoldDownCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvHoldDownCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvPlungerCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvPlungerCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvRodGuideCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvRodGuideCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvSeatsCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvSeatsCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_InvValveRodCondition') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_InvValveRodCondition](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_KnockOut') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_KnockOut](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_tblTypes_KnockOut] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_LowerExtension') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_LowerExtension](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_OnOffTool') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_OnOffTool](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PlungerFit') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PlungerFit](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PlungerLength') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PlungerLength](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PlungerMaterial') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PlungerMaterial](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PonyRods') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PonyRods](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PumpBoreBasic') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PumpBoreBasic](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_PumpType') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_PumpType](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_ReasonRepaired') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_ReasonRepaired](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_tblTypes_ReasonRepaired] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_SeatingLocation') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_SeatingLocation](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_SeatingType') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_SeatingType](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_SoldByOption') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_SoldByOption](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_tblTypes_SoldByOption] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_SpecialtyItems') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_SpecialtyItems](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_StandingValve') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_StandingValve](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_StandingValveCages') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_StandingValveCages](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_tblTypes_StandingValveCages] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_Strainers') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_Strainers](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_TicketCompletedBy') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_TicketCompletedBy](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](4000) NULL,
	 CONSTRAINT [PK_tblTypes_TicketCompletedBy] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_TopSeals') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_TopSeals](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_TravellingCages') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_TravellingCages](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_TubingSize') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_TubingSize](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblTypes_UpperExtension') = 0
BEGIN
	CREATE TABLE [dbo].[tblTypes_UpperExtension](
		[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
		[DisplayText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_tblTypes_UpperExtension] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblUsernameCustomerAccess') = 0
BEGIN
	CREATE TABLE [dbo].[tblUsernameCustomerAccess](
		[UsernameCustomerAccessID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NOT NULL,
		[CustomerID] [int] NOT NULL,
	 CONSTRAINT [PK_tblUsernameCustomerAccess] PRIMARY KEY CLUSTERED 
	(
		[UsernameCustomerAccessID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblUserRoles') = 0
BEGIN
	CREATE TABLE [dbo].[tblUserRoles](
		[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NOT NULL,
		[RoleID] [int] NOT NULL,
	 CONSTRAINT [PK_tblUserRoles] PRIMARY KEY CLUSTERED 
	(
		[UserRoleID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF dbo.fnTableExists('tblUsers') = 0
BEGIN
	CREATE TABLE [dbo].[tblUsers](
		[UserID] [int] IDENTITY(1,1) NOT NULL,
		[Username] [nvarchar](max) NOT NULL,
		[PasswordSalt] [nvarchar](max) NOT NULL,
		[HashedPassword] [nvarchar](max) NOT NULL,
		[PasswordVerificationToken] [nvarchar](max) NULL,
		[PasswordVerificationTokenExpirationDate] [nvarchar](max) NULL,
		[ConfirmationToken] [nvarchar](max) NULL,
		[PasswordFailuresSinceLastSuccess] [int] NULL,
		[LastPasswordFailureDate] [date] NULL,
		[LastPasswordChangedDate] [date] NULL,
		[LastLoginDate] [date] NULL,
		[LastLockoutDate] [date] NULL,
		[CreateDate] [date] NULL,
		[LastActivityDate] [date] NULL,
		[IsLockedOut] [bit] NULL,
		[IsApproved] [bit] NULL,
		[FirstName] [nvarchar](max) NULL,
		[LastName] [nvarchar](max) NULL,
		[Email] [nvarchar](max) NULL,
		[Comment] [nvarchar](max) NULL,
	 CONSTRAINT [PK_tblUsers] PRIMARY KEY CLUSTERED 
	(
		[UserID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblVendorContacts') = 0
BEGIN
	CREATE TABLE [dbo].[tblVendorContacts](
		[KPContactID] [int] IDENTITY(1,1) NOT NULL,
		[KFVendorID] [int] NULL,
		[KFVendorLocationID] [int] NULL,
		[FirstName] [nvarchar](255) NULL,
		[LastName] [nvarchar](255) NULL,
		[Phone1] [nvarchar](255) NULL,
		[Phone2] [nvarchar](255) NULL,
		[Email] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[upsizeTS] [timestamp] NULL,
	 CONSTRAINT [aaaaatblVendorContacts_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPContactID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblVendorLocations') = 0
BEGIN
	CREATE TABLE [dbo].[tblVendorLocations](
		[KPVendorLocationID] [int] IDENTITY(1,1) NOT NULL,
		[KFVendorID] [int] NULL,
		[LocationName] [nchar](20) NULL,
		[Address1] [nvarchar](255) NULL,
		[Address2] [nvarchar](255) NULL,
		[City] [nvarchar](255) NULL,
		[State] [nvarchar](255) NULL,
		[Zip] [nvarchar](255) NULL,
	 CONSTRAINT [PK_tblVendorLocations] PRIMARY KEY CLUSTERED 
	(
		[KPVendorLocationID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblVendors') = 0
BEGIN
	CREATE TABLE [dbo].[tblVendors](
		[KPVendorID] [int] IDENTITY(1,1) NOT NULL,
		[VendorName] [nvarchar](255) NULL,
		[VendorLogo] [nvarchar](255) NULL,
		[Website] [nvarchar](255) NULL,
	 CONSTRAINT [aaaaatblVendors_PK] PRIMARY KEY NONCLUSTERED 
	(
		[KPVendorID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF dbo.fnTableExists('tblWellLocation') = 0
BEGIN
	CREATE TABLE [dbo].[tblWellLocation](
		[KPWellLocationID] [int] IDENTITY(1,1) NOT NULL,
		[KFLeaseID] [int] NULL,
		[KFVendorLocationID] [int] NULL,
		[KFCustomerID] [int] NULL,
		[WellNumber] [nchar](20) NULL,
		[APINumber] [nvarchar](max) NULL,
	 CONSTRAINT [PK_tblWellLocation] PRIMARY KEY CLUSTERED 
	(
		[KPWellLocationID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END


-- CONSTRAINTS
IF dbo.fnConstraintExists('FK_AcePumpProfiles_Customers_CustomerID') = 0
BEGIN
	ALTER TABLE [dbo].[tblAcePumpProfiles]  WITH CHECK ADD  CONSTRAINT [FK_AcePumpProfiles_Customers_CustomerID] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
END
GO

IF dbo.fnConstraintExists('FK_tblAcePumpProfiles_tblUsers') = 0
BEGIN
	ALTER TABLE [dbo].[tblAcePumpProfiles]  WITH CHECK ADD  CONSTRAINT [FK_tblAcePumpProfiles_tblUsers] FOREIGN KEY([UserID])
	REFERENCES [dbo].[tblUsers] ([UserID])
END
GO

IF dbo.fnConstraintExists('FK_tblCustomerContacts_tblCustomers') = 0
BEGIN
	ALTER TABLE [dbo].[tblCustomerContacts]  WITH CHECK ADD  CONSTRAINT [FK_tblCustomerContacts_tblCustomers] FOREIGN KEY([KFCustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
END
GO

IF dbo.fnConstraintExists('FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID') = 0
BEGIN
	ALTER TABLE [dbo].[tblCustomerPartSpecials]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
	ON DELETE CASCADE
END
GO

IF dbo.fnConstraintExists('FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID') = 0
BEGIN
	ALTER TABLE [dbo].[tblCustomerPartSpecials]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID] FOREIGN KEY([PartID])
	REFERENCES [dbo].[tblParts] ([KPPartID])
	ON DELETE CASCADE
END
GO

IF dbo.fnConstraintExists('FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID') = 0
BEGIN
	ALTER TABLE [dbo].[tblCustomers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID] FOREIGN KEY([CountySalesTaxRateID])
	REFERENCES [dbo].[tblCountySalesTaxRates] ([CountySalesTaxRateID])
END
GO

IF dbo.fnConstraintExists('FK_tblDeliveryTicketImageUploads_tblShopTickets_DeliveryTicketID') = 0
BEGIN
	ALTER TABLE [dbo].[tblDeliveryTicketImageUploads]  WITH CHECK ADD  CONSTRAINT [FK_tblDeliveryTicketImageUploads_tblShopTickets_DeliveryTicketID] FOREIGN KEY([DeliveryTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
	ON DELETE CASCADE
END
GO

IF dbo.fnConstraintExists('FK_tblInvoices_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblInvoices]  WITH NOCHECK ADD  CONSTRAINT [FK_tblInvoices_tblShopTickets] FOREIGN KEY([KFShopTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_LineItems_RepairTickets_KFRepairTicketID') = 0
BEGIN
	ALTER TABLE [dbo].[tblLineItems]  WITH CHECK ADD  CONSTRAINT [FK_LineItems_RepairTickets_KFRepairTicketID] FOREIGN KEY([KFRepairTicketID])
	REFERENCES [dbo].[tblRepairTickets] ([KPRepairID])
END
GO

IF dbo.fnConstraintExists('FK_tblLineItems_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblLineItems]  WITH NOCHECK ADD  CONSTRAINT [FK_tblLineItems_tblShopTickets] FOREIGN KEY([KFShopTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
	ON DELETE CASCADE
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimes_tblParts') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimes_tblParts] FOREIGN KEY([TemplatePartDefID])
	REFERENCES [dbo].[tblParts] ([KPPartID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimes_tblPumps') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimes_tblPumps] FOREIGN KEY([PumpID])
	REFERENCES [dbo].[tblPumps] ([KPPumpID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimes_tblRepairTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimes_tblRepairTickets] FOREIGN KEY([RuntimeEndedByInspectionID])
	REFERENCES [dbo].[tblRepairTickets] ([KPRepairID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimes_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimes_tblShopTickets] FOREIGN KEY([RuntimeStartedByTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimeSegments_tblPartRuntimes') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimeSegments]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimeSegments_tblPartRuntimes] FOREIGN KEY([RuntimeID])
	REFERENCES [dbo].[tblPartRuntimes] ([PartRuntimeID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimeSegments_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimeSegments]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimeSegments_tblShopTickets] FOREIGN KEY([SegmentEndedByTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartRuntimeSegments_tblShopTickets1') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartRuntimeSegments]  WITH CHECK ADD  CONSTRAINT [FK_tblPartRuntimeSegments_tblShopTickets1] FOREIGN KEY([SegmentStartedByTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartsAssemblies_tblAssemblies') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartsAssemblies]  WITH CHECK ADD  CONSTRAINT [FK_tblPartsAssemblies_tblAssemblies] FOREIGN KEY([KFAssemblyID])
	REFERENCES [dbo].[tblAssemblies] ([KPAssemblyID])
END
GO

IF dbo.fnConstraintExists('FK_tblPartsAssemblies_tblParts') = 0
BEGIN
	ALTER TABLE [dbo].[tblPartsAssemblies]  WITH CHECK ADD  CONSTRAINT [FK_tblPartsAssemblies_tblParts] FOREIGN KEY([KFPartsID])
	REFERENCES [dbo].[tblParts] ([KPPartID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpHistory_tblPumps') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpHistory_tblPumps] FOREIGN KEY([KFPumpID])
	REFERENCES [dbo].[tblPumps] ([KPPumpID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpHistory_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_tblPumpHistory_tblShopTickets] FOREIGN KEY([KFShopTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpRuntimes_tblPumps') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpRuntimes_tblPumps] FOREIGN KEY([PumpID])
	REFERENCES [dbo].[tblPumps] ([KPPumpID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpRuntimes_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpRuntimes_tblShopTickets] FOREIGN KEY([RuntimeStartedByTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpRuntimes_tblShopTickets1') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpRuntimes]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpRuntimes_tblShopTickets1] FOREIGN KEY([RuntimeEndedByTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumps_tblPumpTemplates') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumps]  WITH NOCHECK ADD  CONSTRAINT [FK_tblPumps_tblPumpTemplates] FOREIGN KEY([KFPumpTemplateID])
	REFERENCES [dbo].[tblPumpTemplates] ([KPPumpTemplateID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumps_tblVendorLocations') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumps]  WITH NOCHECK ADD  CONSTRAINT [FK_tblPumps_tblVendorLocations] FOREIGN KEY([KFVendorLocationID])
	REFERENCES [dbo].[tblVendorLocations] ([KPVendorLocationID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpTemplateDefinitions_tblPumpTemplateCategories') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpSpecs]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpTemplateDefinitions_tblPumpTemplateCategories] FOREIGN KEY([KFPumpTemplateCategoryID])
	REFERENCES [dbo].[tblPumpTemplateCategories] ([KPPumpTemplateCategoryID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpTemplateDefJoin_tblPumpTemplateDefinitions') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpTemplateSpecsJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_tblPumpTemplateDefJoin_tblPumpTemplateDefinitions] FOREIGN KEY([KFPumpSpecsID])
	REFERENCES [dbo].[tblPumpSpecs] ([KPPumpSpecsID])
END
GO

IF dbo.fnConstraintExists('FK_tblPumpTemplateDefJoin_tblPumpTemplates') = 0
BEGIN
	ALTER TABLE [dbo].[tblPumpTemplateSpecsJoin]  WITH CHECK ADD  CONSTRAINT [FK_tblPumpTemplateDefJoin_tblPumpTemplates] FOREIGN KEY([KFPumpTemplateID])
	REFERENCES [dbo].[tblPumpTemplates] ([KPPumpTemplateID])
END
GO

IF dbo.fnConstraintExists('FK_tblRepairTickets_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblRepairTickets]  WITH CHECK ADD  CONSTRAINT [FK_tblRepairTickets_tblShopTickets] FOREIGN KEY([KFShopTicketID])
	REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID])
	ON DELETE CASCADE
END
GO

IF dbo.fnConstraintExists('FK_tblCountySalesTaxRates_tblShopTickets') = 0
BEGIN
	ALTER TABLE [dbo].[tblShopTickets]  WITH CHECK ADD  CONSTRAINT [FK_tblCountySalesTaxRates_tblShopTickets] FOREIGN KEY([CountySalesTaxRateID])
	REFERENCES [dbo].[tblCountySalesTaxRates] ([CountySalesTaxRateID])
END
GO

IF dbo.fnConstraintExists('FK_tblShopTickets_tblCustomers') = 0
BEGIN
	ALTER TABLE [dbo].[tblShopTickets]  WITH NOCHECK ADD  CONSTRAINT [FK_tblShopTickets_tblCustomers] FOREIGN KEY([KFCustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
END
GO

IF dbo.fnConstraintExists('FK_tblShopTickets_tblPumps_Dispatched') = 0
BEGIN
	ALTER TABLE [dbo].[tblShopTickets]  WITH CHECK ADD  CONSTRAINT [FK_tblShopTickets_tblPumps_Dispatched] FOREIGN KEY([PumpDispatchedID])
	REFERENCES [dbo].[tblPumps] ([KPPumpID])
END
GO

IF dbo.fnConstraintExists('FK_tblShopTickets_tblPumps_Failed') = 0
BEGIN
	ALTER TABLE [dbo].[tblShopTickets]  WITH CHECK ADD  CONSTRAINT [FK_tblShopTickets_tblPumps_Failed] FOREIGN KEY([PumpFailedID])
	REFERENCES [dbo].[tblPumps] ([KPPumpID])
END
GO

IF dbo.fnConstraintExists('FK_tblShopTickets_tblVendorLocations') = 0
BEGIN
	ALTER TABLE [dbo].[tblShopTickets]  WITH NOCHECK ADD  CONSTRAINT [FK_tblShopTickets_tblVendorLocations] FOREIGN KEY([KFVendorLocationID])
	REFERENCES [dbo].[tblVendorLocations] ([KPVendorLocationID])
END
GO

IF dbo.fnConstraintExists('FK_tblTemplatePartsJoin_tblParts') = 0
BEGIN
	ALTER TABLE [dbo].[tblTemplatePartsJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_tblTemplatePartsJoin_tblParts] FOREIGN KEY([KFPartsID])
	REFERENCES [dbo].[tblParts] ([KPPartID])
END
GO

IF dbo.fnConstraintExists('FK_tblTemplatePartsJoin_tblPumpTemplates') = 0
BEGIN
	ALTER TABLE [dbo].[tblTemplatePartsJoin]  WITH NOCHECK ADD  CONSTRAINT [FK_tblTemplatePartsJoin_tblPumpTemplates] FOREIGN KEY([KFPumpTemplateID])
	REFERENCES [dbo].[tblPumpTemplates] ([KPPumpTemplateID])
END
GO

IF dbo.fnConstraintExists('FK_tblUsernameCustomerAccess_tblCustomers') = 0
BEGIN
	ALTER TABLE [dbo].[tblUsernameCustomerAccess]  WITH CHECK ADD  CONSTRAINT [FK_tblUsernameCustomerAccess_tblCustomers] FOREIGN KEY([CustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
END
GO

IF dbo.fnConstraintExists('FK_tblUsernameCustomerAccess_tblUsers') = 0
BEGIN
	ALTER TABLE [dbo].[tblUsernameCustomerAccess]  WITH CHECK ADD  CONSTRAINT [FK_tblUsernameCustomerAccess_tblUsers] FOREIGN KEY([UserID])
	REFERENCES [dbo].[tblUsers] ([UserID])
END
GO

IF dbo.fnConstraintExists('FK_tblVendorContacts_tblVendorLocations') = 0
BEGIN
	ALTER TABLE [dbo].[tblVendorContacts]  WITH CHECK ADD  CONSTRAINT [FK_tblVendorContacts_tblVendorLocations] FOREIGN KEY([KFVendorLocationID])
	REFERENCES [dbo].[tblVendorLocations] ([KPVendorLocationID])
END
GO

IF dbo.fnConstraintExists('FK_tblVendorLocations_tblVendors1') = 0
BEGIN
	ALTER TABLE [dbo].[tblVendorLocations]  WITH CHECK ADD  CONSTRAINT [FK_tblVendorLocations_tblVendors1] FOREIGN KEY([KFVendorID])
	REFERENCES [dbo].[tblVendors] ([KPVendorID])
END
GO

IF dbo.fnConstraintExists('FK_tblWellLocation_tblCustomers') = 0
BEGIN
	ALTER TABLE [dbo].[tblWellLocation]  WITH CHECK ADD  CONSTRAINT [FK_tblWellLocation_tblCustomers] FOREIGN KEY([KFCustomerID])
	REFERENCES [dbo].[tblCustomers] ([KPCustomerID])
END
GO

IF dbo.fnConstraintExists('FK_tblWellLocation_tblLeaseLocations') = 0
BEGIN
	ALTER TABLE [dbo].[tblWellLocation]  WITH NOCHECK ADD  CONSTRAINT [FK_tblWellLocation_tblLeaseLocations] FOREIGN KEY([KFLeaseID])
	REFERENCES [dbo].[tblLeaseLocations] ([KPLeaseID])
END
GO