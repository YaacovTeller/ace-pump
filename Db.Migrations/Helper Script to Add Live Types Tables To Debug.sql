-- Note: This script will create all types tables. 
-- HOWEVER:
-- if the Types_SoldByOption table already exists, then it will throw at that point and not
-- create the tables in the rest of the script!
-- So first check if Types_SoldByOption already exists, and if it does, comment out the script for that table.
GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AcePump_Debug"
:setvar DefaultFilePrefix "AcePump_Debug"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating [dbo].[Types_BallsAndSeats]...';


GO
CREATE TABLE [dbo].[Types_BallsAndSeats] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_BarrelLength]...';


GO
CREATE TABLE [dbo].[Types_BarrelLength] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_BarrelMaterial]...';


GO
CREATE TABLE [dbo].[Types_BarrelMaterial] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_BarrelType]...';


GO
CREATE TABLE [dbo].[Types_BarrelType] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_BarrelWasher]...';


GO
CREATE TABLE [dbo].[Types_BarrelWasher] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_Collet]...';


GO
CREATE TABLE [dbo].[Types_Collet] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_HoldDownType]...';


GO
CREATE TABLE [dbo].[Types_HoldDownType] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvBallsAndSeatsCondition]...';


GO
CREATE TABLE [dbo].[Types_InvBallsAndSeatsCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvBallsCondition]...';


GO
CREATE TABLE [dbo].[Types_InvBallsCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvBarrelCondition]...';


GO
CREATE TABLE [dbo].[Types_InvBarrelCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvCagesCondition]...';


GO
CREATE TABLE [dbo].[Types_InvCagesCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvHoldDownCondition]...';


GO
CREATE TABLE [dbo].[Types_InvHoldDownCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvPlungerCondition]...';


GO
CREATE TABLE [dbo].[Types_InvPlungerCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvRodGuideCondition]...';


GO
CREATE TABLE [dbo].[Types_InvRodGuideCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvSeatsCondition]...';


GO
CREATE TABLE [dbo].[Types_InvSeatsCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_InvValveRodCondition]...';


GO
CREATE TABLE [dbo].[Types_InvValveRodCondition] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_KnockOut]...';


GO
CREATE TABLE [dbo].[Types_KnockOut] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    CONSTRAINT [PK_tblTypes_KnockOut] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_LowerExtension]...';


GO
CREATE TABLE [dbo].[Types_LowerExtension] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_OnOffTool]...';


GO
CREATE TABLE [dbo].[Types_OnOffTool] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PlungerFit]...';


GO
CREATE TABLE [dbo].[Types_PlungerFit] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PlungerLength]...';


GO
CREATE TABLE [dbo].[Types_PlungerLength] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PlungerMaterial]...';


GO
CREATE TABLE [dbo].[Types_PlungerMaterial] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PonyRods]...';


GO
CREATE TABLE [dbo].[Types_PonyRods] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PumpBoreBasic]...';


GO
CREATE TABLE [dbo].[Types_PumpBoreBasic] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_PumpType]...';


GO
CREATE TABLE [dbo].[Types_PumpType] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_ReasonRepaired]...';


GO
CREATE TABLE [dbo].[Types_ReasonRepaired] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    CONSTRAINT [PK_tblTypes_ReasonRepaired] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_SeatingLocation]...';


GO
CREATE TABLE [dbo].[Types_SeatingLocation] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_SeatingType]...';


GO
CREATE TABLE [dbo].[Types_SeatingType] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_SoldByOption]...';


--GO
--CREATE TABLE [dbo].[Types_SoldByOption] (
--    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
--    [DisplayText] NVARCHAR (500) NULL,
--    CONSTRAINT [PK_tblTypes_SoldByOption] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
--);


GO
PRINT N'Creating [dbo].[Types_SpecialtyItems]...';


GO
CREATE TABLE [dbo].[Types_SpecialtyItems] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_StandingValve]...';


GO
CREATE TABLE [dbo].[Types_StandingValve] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_StandingValveCages]...';


GO
CREATE TABLE [dbo].[Types_StandingValveCages] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    CONSTRAINT [PK_tblTypes_StandingValveCages] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_Strainers]...';


GO
CREATE TABLE [dbo].[Types_Strainers] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_TicketCompletedBy]...';


GO
CREATE TABLE [dbo].[Types_TicketCompletedBy] (
    [ItemTypeID]  INT             IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (4000) NULL,
    CONSTRAINT [PK_tblTypes_TicketCompletedBy] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_TopSeals]...';


GO
CREATE TABLE [dbo].[Types_TopSeals] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_TravellingCages]...';


GO
CREATE TABLE [dbo].[Types_TravellingCages] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_TubingSize]...';


GO
CREATE TABLE [dbo].[Types_TubingSize] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Creating [dbo].[Types_UpperExtension]...';


GO
CREATE TABLE [dbo].[Types_UpperExtension] (
    [ItemTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [DisplayText] NVARCHAR (500) NULL,
    CONSTRAINT [PK_tblTypes_UpperExtension] PRIMARY KEY CLUSTERED ([ItemTypeID] ASC)
);


GO
PRINT N'Update complete.';


GO