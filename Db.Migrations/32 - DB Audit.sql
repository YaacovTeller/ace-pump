BEGIN TRANSACTION
GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AcePumpCopyWithData"
:setvar DefaultFilePrefix "AcePumpCopyWithData"
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
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Applications] TO [aspnet_Membership_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Applications] TO [aspnet_Profile_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Applications] TO [aspnet_Roles_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Applications] TO [aspnet_Personalization_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_MembershipUsers] TO [aspnet_Membership_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Profiles] TO [aspnet_Profile_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Roles] TO [aspnet_Roles_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Users] TO [aspnet_Membership_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Users] TO [aspnet_Profile_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Users] TO [aspnet_Roles_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_Users] TO [aspnet_Personalization_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_UsersInRoles] TO [aspnet_Roles_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_WebPartState_Paths] TO [aspnet_Personalization_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_WebPartState_Shared] TO [aspnet_Personalization_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping Permission...';


GO
REVOKE SELECT
    ON OBJECT::[dbo].[vw_aspnet_WebPartState_User] TO [aspnet_Personalization_ReportingAccess] CASCADE
    AS [dbo];


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KPContactID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[KFCustomerID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'KFCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[FirstName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[LastName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone1].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Phone2].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Email].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblCustomerContacts].[Title].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomerContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[KPDiscountID].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'KPDiscountID';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountAmount].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountAmount';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[TextFormat]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextFormat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[Description].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblDiscounts].[DiscountType].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblDiscounts', @level2type = N'COLUMN', @level2name = N'DiscountType';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[KPOptionID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'KPOptionID';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblPartsOptions].[OptionValue].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsOptions', @level2type = N'COLUMN', @level2name = N'OptionValue';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KPContactID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KPContactID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[KFVendorID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'KFVendorID';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[FirstName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[LastName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'LastName';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone1].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone1';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Phone2].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Phone2';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Email].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Email';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendorContacts].[Title].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendorContacts', @level2type = N'COLUMN', @level2name = N'Title';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[KPVendorID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'KPVendorID';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[VendorLogo].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'VendorLogo';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblVendors].[Website].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblVendors', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[qryCustomers].[MS_DiagramPane1]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DiagramPane1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'qryCustomers';


GO
PRINT N'Dropping [dbo].[qryCustomers].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DiagramPaneCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'qryCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerLogo].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerLogo';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DiscountID].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address1].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address1';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Address2].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Address2';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[City].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'City';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'CustomerName';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[KPCustomerID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'KPCustomerID';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Phone].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Phone';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[State].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'State';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Website].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Website';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Zip].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers', @level2type = N'COLUMN', @level2name = N'Zip';


GO
PRINT N'Dropping [dbo].[tblCustomers].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblCustomers].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblCustomers';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Quantity_Int].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Quantity_Int';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Expression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Expression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[MS_Format]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Format', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitPrice_Old].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitPrice_Old';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Description].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFPartID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFPartID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KFShopTicketID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KFShopTicketID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[KPLineItemID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'KPLineItemID';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[MS_Format]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Format', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[UnitDiscount].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems', @level2type = N'COLUMN', @level2name = N'UnitDiscount';


GO
PRINT N'Dropping [dbo].[tblLineItems].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[FCMinDesignVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinDesignVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[FCMinReadVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinReadVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[FCMinWriteVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinWriteVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblLineItems].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblLineItems';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountID].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountID';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Expression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Expression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[MS_Format]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Format', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[DiscountPrice].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'DiscountPrice';


GO
PRINT N'Dropping [dbo].[tblParts].[Tracked].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Tracked';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[VendorName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'VendorName';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[DefaultValue]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DefaultValue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[MS_Format]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Format', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Active].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Active';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[TextFormat]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextFormat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[Description].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'Description';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[DefaultValue]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DefaultValue', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFOptionID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFOptionID';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[MS_DecimalPlaces]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DecimalPlaces', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KFPartCategory].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KFPartCategory';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[KPPartID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'KPPartID';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts', @level2type = N'COLUMN', @level2name = N'PartNumber';


GO
PRINT N'Dropping [dbo].[tblParts].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[FCMinDesignVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinDesignVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[FCMinReadVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinReadVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[FCMinWriteVer]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FCMinWriteVer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblParts].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblParts';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[TextFormat]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextFormat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryDescription].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryDescription';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[MS_Description]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[CategoryName].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'CategoryName';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[KPCategoryID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory', @level2type = N'COLUMN', @level2name = N'KPCategoryID';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPartsCategory].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPartsCategory';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KFWellLocationID].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KFWellLocationID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[KPPumpID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'KPPumpID';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[GUID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[MS_DisplayControl]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DisplayControl', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[MS_IMEMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMEMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[MS_IMESentMode]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_IMESentMode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[PumpNumber].[UnicodeCompression]...';


GO
EXECUTE sp_dropextendedproperty @name = N'UnicodeCompression', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps', @level2type = N'COLUMN', @level2name = N'PumpNumber';


GO
PRINT N'Dropping [dbo].[tblPumps].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblPumps].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblPumps';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[AggregateType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AggregateType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[AllowZeroLength]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AllowZeroLength', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[AppendOnly]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AppendOnly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[CollatingOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CollatingOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[ColumnHidden]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnHidden', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[ColumnOrder]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[ColumnWidth]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ColumnWidth', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[CurrencyLCID]...';


GO
EXECUTE sp_dropextendedproperty @name = N'CurrencyLCID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[DataUpdatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DataUpdatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[OrdinalPosition]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrdinalPosition', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[Required]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Required', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[ResultType]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ResultType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[Size]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[SourceField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[SourceTable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'SourceTable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[TextAlign]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TextAlign', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[KPShopTicketID].[Type]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets', @level2type = N'COLUMN', @level2name = N'KPShopTicketID';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[AlternateBackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[AlternateBackThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[AlternateBackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'AlternateBackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[Attributes]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Attributes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[BackShade]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackShade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[BackTint]...';


GO
EXECUTE sp_dropextendedproperty @name = N'BackTint', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[DatasheetForeThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetForeThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[DatasheetGridlinesThemeColorIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DatasheetGridlinesThemeColorIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[DateCreated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DateCreated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[DisplayViewsOnSharePointSite]...';


GO
EXECUTE sp_dropextendedproperty @name = N'DisplayViewsOnSharePointSite', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[FilterOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'FilterOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[HideNewField]...';


GO
EXECUTE sp_dropextendedproperty @name = N'HideNewField', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[LastUpdated]...';


GO
EXECUTE sp_dropextendedproperty @name = N'LastUpdated', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[MS_DefaultView]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_DefaultView', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[MS_OrderByOn]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_OrderByOn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[MS_Orientation]...';


GO
EXECUTE sp_dropextendedproperty @name = N'MS_Orientation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[Name]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[OrderByOnLoad]...';


GO
EXECUTE sp_dropextendedproperty @name = N'OrderByOnLoad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[PublishToWeb]...';


GO
EXECUTE sp_dropextendedproperty @name = N'PublishToWeb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[ReadOnlyWhenDisconnected]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ReadOnlyWhenDisconnected', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[RecordCount]...';


GO
EXECUTE sp_dropextendedproperty @name = N'RecordCount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[ThemeFontIndex]...';


GO
EXECUTE sp_dropextendedproperty @name = N'ThemeFontIndex', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[TotalsRow]...';


GO
EXECUTE sp_dropextendedproperty @name = N'TotalsRow', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblShopTickets].[Updatable]...';


GO
EXECUTE sp_dropextendedproperty @name = N'Updatable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tblShopTickets';


GO
PRINT N'Dropping [dbo].[tblCustomers].[CustomerID]...';


GO
DROP INDEX [CustomerID]
    ON [dbo].[tblCustomers];


GO
PRINT N'Dropping [dbo].[tblCustomers].[tblCustomersDiscountID]...';


GO
DROP INDEX [tblCustomersDiscountID]
    ON [dbo].[tblCustomers];


GO
PRINT N'Dropping [dbo].[tblLineItems].[InvoiceID]...';


GO
DROP INDEX [InvoiceID]
    ON [dbo].[tblLineItems];


GO
PRINT N'Dropping [dbo].[tblLineItems].[LineItemID]...';


GO
DROP INDEX [LineItemID]
    ON [dbo].[tblLineItems];


GO
PRINT N'Dropping [dbo].[tblLineItems].[PartID]...';


GO
DROP INDEX [PartID]
    ON [dbo].[tblLineItems];


GO
PRINT N'Dropping [dbo].[tblParts].[PartID]...';


GO
DROP INDEX [PartID]
    ON [dbo].[tblParts];


GO
PRINT N'Dropping [dbo].[tblParts].[PartNumber]...';


GO
DROP INDEX [PartNumber]
    ON [dbo].[tblParts];


GO
PRINT N'Dropping [dbo].[tblParts].[tblPartsDiscountID]...';


GO
DROP INDEX [tblPartsDiscountID]
    ON [dbo].[tblParts];


GO
PRINT N'Dropping [dbo].[DF_tblCustomers_APINumberRequired_1]...';


GO
ALTER TABLE [dbo].[tblCustomers] DROP CONSTRAINT [DF_tblCustomers_APINumberRequired_1];


GO
PRINT N'Dropping [dbo].[DF_tblLineItems_ApprovedRepair]...';


GO
ALTER TABLE [dbo].[tblLineItems] DROP CONSTRAINT [DF_tblLineItems_ApprovedRepair];


GO
PRINT N'Dropping [dbo].[DF_tblLineItems_SalesTax]...';


GO
ALTER TABLE [dbo].[tblLineItems] DROP CONSTRAINT [DF_tblLineItems_SalesTax];


GO
PRINT N'Dropping [dbo].[DF__tblParts__QtyOpt__239E4DCF]...';


GO
ALTER TABLE [dbo].[tblParts] DROP CONSTRAINT [DF__tblParts__QtyOpt__239E4DCF];


GO
PRINT N'Dropping [dbo].[DF__tblParts__Active__25869641]...';


GO
ALTER TABLE [dbo].[tblParts] DROP CONSTRAINT [DF__tblParts__Active__25869641];


GO
PRINT N'Dropping [dbo].[DF_tblParts_IsAssembly]...';


GO
ALTER TABLE [dbo].[tblParts] DROP CONSTRAINT [DF_tblParts_IsAssembly];


GO
PRINT N'Dropping [dbo].[DF_tblPartsAssemblies_PartsQuantity]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] DROP CONSTRAINT [DF_tblPartsAssemblies_PartsQuantity];


GO
PRINT N'Dropping [dbo].[DF_tblShopTickets_TicketDate]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [DF_tblShopTickets_TicketDate];


GO
PRINT N'Dropping [dbo].[DF_tblRepairTickets_OpenTicket]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [DF_tblRepairTickets_OpenTicket];


GO
PRINT N'Dropping [dbo].[DF_tblPartsAssemblies_SortOrder]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] DROP CONSTRAINT [DF_tblPartsAssemblies_SortOrder];


GO
PRINT N'Dropping [dbo].[tblCustomers_FK00]...';


GO
ALTER TABLE [dbo].[tblCustomers] DROP CONSTRAINT [tblCustomers_FK00];


GO
PRINT N'Dropping [dbo].[FK_tblCustomers_tblQbInvoiceClasses_QbInvoiceClassID]...';


GO
ALTER TABLE [dbo].[tblCustomers] DROP CONSTRAINT [FK_tblCustomers_tblQbInvoiceClasses_QbInvoiceClassID];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID]...';


GO
ALTER TABLE [dbo].[tblCustomers] DROP CONSTRAINT [FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] DROP CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID];


GO
PRINT N'Dropping [dbo].[FK_tblShopTickets_tblCustomers]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [FK_tblShopTickets_tblCustomers];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblPartInstances] DROP CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID];


GO
PRINT N'Dropping [dbo].[FK_tblWellLocation_tblCustomers]...';


GO
ALTER TABLE [dbo].[tblWellLocation] DROP CONSTRAINT [FK_tblWellLocation_tblCustomers];


GO
PRINT N'Dropping [dbo].[FK_tblUsernameCustomerAccess_tblCustomers]...';


GO
ALTER TABLE [dbo].[tblUsernameCustomerAccess] DROP CONSTRAINT [FK_tblUsernameCustomerAccess_tblCustomers];


GO
PRINT N'Dropping [dbo].[FK_tblCustomerContacts_tblCustomers]...';


GO
ALTER TABLE [dbo].[tblCustomerContacts] DROP CONSTRAINT [FK_tblCustomerContacts_tblCustomers];


GO
PRINT N'Dropping [dbo].[FK_AcePumpProfiles_Customers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblAcePumpProfiles] DROP CONSTRAINT [FK_AcePumpProfiles_Customers_CustomerID];


GO
PRINT N'Dropping [dbo].[FK_LineItems_RepairTickets_KFRepairTicketID]...';


GO
ALTER TABLE [dbo].[tblLineItems] DROP CONSTRAINT [FK_LineItems_RepairTickets_KFRepairTicketID];


GO
PRINT N'Dropping [dbo].[FK_tblLineItems_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblLineItems] DROP CONSTRAINT [FK_tblLineItems_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblTemplatePartsJoin_tblParts]...';


GO
ALTER TABLE [dbo].[tblTemplatePartsJoin] DROP CONSTRAINT [FK_tblTemplatePartsJoin_tblParts];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID]...';


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] DROP CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID];


GO
PRINT N'Dropping [dbo].[FK_tblPartsAssemblies_tblParts]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] DROP CONSTRAINT [FK_tblPartsAssemblies_tblParts];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID]...';


GO
ALTER TABLE [dbo].[tblPartInstances] DROP CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimes_tblParts]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] DROP CONSTRAINT [FK_tblPartRuntimes_tblParts];


GO
PRINT N'Dropping [dbo].[FK_tblShopTickets_tblPumps_Dispatched]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [FK_tblShopTickets_tblPumps_Dispatched];


GO
PRINT N'Dropping [dbo].[FK_tblShopTickets_tblPumps_Failed]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [FK_tblShopTickets_tblPumps_Failed];


GO
PRINT N'Dropping [dbo].[FK_tblPumpHistory_tblPumps]...';


GO
ALTER TABLE [dbo].[tblPumpHistory] DROP CONSTRAINT [FK_tblPumpHistory_tblPumps];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimes_tblPumps]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] DROP CONSTRAINT [FK_tblPartRuntimes_tblPumps];


GO
PRINT N'Dropping [dbo].[FK_tblPumpRuntimes_tblPumps]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] DROP CONSTRAINT [FK_tblPumpRuntimes_tblPumps];


GO
PRINT N'Dropping [dbo].[FK_tblPumps_tblPumpTemplates]...';


GO
ALTER TABLE [dbo].[tblPumps] DROP CONSTRAINT [FK_tblPumps_tblPumpTemplates];


GO
PRINT N'Dropping [dbo].[FK_tblTemplatePartsJoin_tblPumpTemplates]...';


GO
ALTER TABLE [dbo].[tblTemplatePartsJoin] DROP CONSTRAINT [FK_tblTemplatePartsJoin_tblPumpTemplates];


GO
PRINT N'Dropping [dbo].[FK_tblRepairTickets_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] DROP CONSTRAINT [FK_tblRepairTickets_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] DROP CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID];


GO
PRINT N'Dropping [dbo].[FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] DROP CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimes_tblRepairTickets]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] DROP CONSTRAINT [FK_tblPartRuntimes_tblRepairTickets];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimeSegments_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] DROP CONSTRAINT [FK_tblPartRuntimeSegments_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimeSegments_tblShopTickets1]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] DROP CONSTRAINT [FK_tblPartRuntimeSegments_tblShopTickets1];


GO
PRINT N'Dropping [dbo].[FK_tblShopTickets_tblVendorLocations]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [FK_tblShopTickets_tblVendorLocations];


GO
PRINT N'Dropping [dbo].[FK_tblCountySalesTaxRates_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [FK_tblCountySalesTaxRates_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblDeliveryTicketImageUploads_tblShopTickets_DeliveryTicketID]...';


GO
ALTER TABLE [dbo].[tblDeliveryTicketImageUploads] DROP CONSTRAINT [FK_tblDeliveryTicketImageUploads_tblShopTickets_DeliveryTicketID];


GO
PRINT N'Dropping [dbo].[FK_tblPumpHistory_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblPumpHistory] DROP CONSTRAINT [FK_tblPumpHistory_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimes_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] DROP CONSTRAINT [FK_tblPartRuntimes_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblPumpRuntimes_tblShopTickets]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] DROP CONSTRAINT [FK_tblPumpRuntimes_tblShopTickets];


GO
PRINT N'Dropping [dbo].[FK_tblPumpRuntimes_tblShopTickets1]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] DROP CONSTRAINT [FK_tblPumpRuntimes_tblShopTickets1];


GO
PRINT N'Dropping [dbo].[FK_tblVendorContacts_tblVendorLocations]...';


GO
ALTER TABLE [dbo].[tblVendorContacts] DROP CONSTRAINT [FK_tblVendorContacts_tblVendorLocations];


GO
PRINT N'Dropping [dbo].[FK_tblVendorLocations_tblVendors1]...';


GO
ALTER TABLE [dbo].[tblVendorLocations] DROP CONSTRAINT [FK_tblVendorLocations_tblVendors1];


GO
PRINT N'Dropping [dbo].[FK_tblPartsAssemblies_tblAssemblies]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] DROP CONSTRAINT [FK_tblPartsAssemblies_tblAssemblies];


GO
PRINT N'Dropping [dbo].[FK_tblWellLocation_tblLeaseLocations]...';


GO
ALTER TABLE [dbo].[tblWellLocation] DROP CONSTRAINT [FK_tblWellLocation_tblLeaseLocations];


GO
PRINT N'Dropping [dbo].[FK_tblPartRuntimeSegments_tblPartRuntimes]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] DROP CONSTRAINT [FK_tblPartRuntimeSegments_tblPartRuntimes];


GO
PRINT N'Dropping [dbo].[FK_tblUsernameCustomerAccess_tblUsers]...';


GO
ALTER TABLE [dbo].[tblUsernameCustomerAccess] DROP CONSTRAINT [FK_tblUsernameCustomerAccess_tblUsers];


GO
PRINT N'Dropping [dbo].[FK_tblAcePumpProfiles_tblUsers]...';


GO
ALTER TABLE [dbo].[tblAcePumpProfiles] DROP CONSTRAINT [FK_tblAcePumpProfiles_tblUsers];


GO
PRINT N'Dropping [dbo].[aaaaatblCustomers_PK]...';


GO
ALTER TABLE [dbo].[tblCustomers] DROP CONSTRAINT [aaaaatblCustomers_PK];


GO
PRINT N'Dropping [dbo].[aaaaatblLineItems_PK]...';


GO
ALTER TABLE [dbo].[tblLineItems] DROP CONSTRAINT [aaaaatblLineItems_PK];


GO
PRINT N'Dropping [dbo].[aaaaatblParts_PK]...';


GO
ALTER TABLE [dbo].[tblParts] DROP CONSTRAINT [aaaaatblParts_PK];


GO
PRINT N'Dropping [dbo].[aaaaatblPartsCategory_PK]...';


GO
ALTER TABLE [dbo].[tblPartsCategory] DROP CONSTRAINT [aaaaatblPartsCategory_PK];


GO
PRINT N'Dropping [dbo].[aaaaatblPumps_PK]...';


GO
ALTER TABLE [dbo].[tblPumps] DROP CONSTRAINT [aaaaatblPumps_PK];


GO
PRINT N'Dropping [dbo].[aaaaatblRepairTickets_PK]...';


GO
ALTER TABLE [dbo].[tblShopTickets] DROP CONSTRAINT [aaaaatblRepairTickets_PK];


GO
PRINT N'Dropping [dbo].[tblCustomerContacts]...';


GO
DROP TABLE [dbo].[tblCustomerContacts];


GO
PRINT N'Dropping [dbo].[tblDiscounts]...';


GO
DROP TABLE [dbo].[tblDiscounts];


GO
PRINT N'Dropping [dbo].[tblEmployees]...';


GO
DROP TABLE [dbo].[tblEmployees];


GO
PRINT N'Dropping [dbo].[tblLineItemBackupCase988]...';


GO
DROP TABLE [dbo].[tblLineItemBackupCase988];


GO
PRINT N'Dropping [dbo].[tblPartsOptions]...';


GO
DROP TABLE [dbo].[tblPartsOptions];


GO
PRINT N'Dropping [dbo].[tblPumpHistory]...';


GO
DROP TABLE [dbo].[tblPumpHistory];


GO
PRINT N'Dropping [dbo].[tblPumpTemplateCategories]...';


GO
DROP TABLE [dbo].[tblPumpTemplateCategories];


GO
PRINT N'Dropping [dbo].[tblRepairTicketBackupCase988]...';


GO
DROP TABLE [dbo].[tblRepairTicketBackupCase988];


GO
PRINT N'Dropping [dbo].[tblShopTicketBackupCase1803]...';


GO
DROP TABLE [dbo].[tblShopTicketBackupCase1803];


GO
PRINT N'Dropping [dbo].[tblShopTicketBackupCase2334]...';


GO
DROP TABLE [dbo].[tblShopTicketBackupCase2334];


GO
PRINT N'Dropping [dbo].[tblShopTicketsBackupCase2339]...';


GO
DROP TABLE [dbo].[tblShopTicketsBackupCase2339];


GO
PRINT N'Dropping [dbo].[tblVendorContacts]...';


GO
DROP TABLE [dbo].[tblVendorContacts];


GO
PRINT N'Dropping [dbo].[tblVendorLocations]...';


GO
DROP TABLE [dbo].[tblVendorLocations];


GO
PRINT N'Dropping [dbo].[tblVendors]...';


GO
DROP TABLE [dbo].[tblVendors];


GO
PRINT N'Dropping [dbo].[qryCustomers]...';


GO
DROP VIEW [dbo].[qryCustomers];


GO
PRINT N'Dropping [dbo].[vw_aspnet_Applications]...';


GO
DROP VIEW [dbo].[vw_aspnet_Applications];


GO
PRINT N'Dropping [dbo].[vw_aspnet_MembershipUsers]...';


GO
DROP VIEW [dbo].[vw_aspnet_MembershipUsers];


GO
PRINT N'Dropping [dbo].[vw_aspnet_Profiles]...';


GO
DROP VIEW [dbo].[vw_aspnet_Profiles];


GO
PRINT N'Dropping [dbo].[vw_aspnet_Roles]...';


GO
DROP VIEW [dbo].[vw_aspnet_Roles];


GO
PRINT N'Dropping [dbo].[vw_aspnet_Users]...';


GO
DROP VIEW [dbo].[vw_aspnet_Users];


GO
PRINT N'Dropping [dbo].[vw_aspnet_UsersInRoles]...';


GO
DROP VIEW [dbo].[vw_aspnet_UsersInRoles];


GO
PRINT N'Dropping [dbo].[vw_aspnet_WebPartState_Paths]...';


GO
DROP VIEW [dbo].[vw_aspnet_WebPartState_Paths];


GO
PRINT N'Dropping [dbo].[vw_aspnet_WebPartState_Shared]...';


GO
DROP VIEW [dbo].[vw_aspnet_WebPartState_Shared];


GO
PRINT N'Dropping [dbo].[vw_aspnet_WebPartState_User]...';


GO
DROP VIEW [dbo].[vw_aspnet_WebPartState_User];


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Membership_BasicAccess', @membername = N'aspnet_Membership_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Membership_ReportingAccess', @membername = N'aspnet_Membership_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Personalization_BasicAccess', @membername = N'aspnet_Personalization_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Personalization_ReportingAccess', @membername = N'aspnet_Personalization_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Profile_BasicAccess', @membername = N'aspnet_Profile_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Profile_ReportingAccess', @membername = N'aspnet_Profile_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Roles_BasicAccess', @membername = N'aspnet_Roles_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'aspnet_Roles_ReportingAccess', @membername = N'aspnet_Roles_FullAccess';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_executor', @membername = N'AcePump_IUsr';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datareader', @membername = N'AcePump_IUsr';


GO
PRINT N'Dropping <unnamed>...';


GO
EXECUTE sp_droprolemember @rolename = N'db_datawriter', @membername = N'AcePump_IUsr';


GO
PRINT N'Dropping [aspnet_Membership_BasicAccess]...';


GO
DROP SCHEMA [aspnet_Membership_BasicAccess];


GO
PRINT N'Dropping [aspnet_Membership_FullAccess]...';


GO
DROP SCHEMA [aspnet_Membership_FullAccess];


GO
PRINT N'Dropping [aspnet_Membership_ReportingAccess]...';


GO
DROP SCHEMA [aspnet_Membership_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Personalization_BasicAccess]...';


GO
DROP SCHEMA [aspnet_Personalization_BasicAccess];


GO
PRINT N'Dropping [aspnet_Personalization_FullAccess]...';


GO
DROP SCHEMA [aspnet_Personalization_FullAccess];


GO
PRINT N'Dropping [aspnet_Personalization_ReportingAccess]...';


GO
DROP SCHEMA [aspnet_Personalization_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Profile_BasicAccess]...';


GO
DROP SCHEMA [aspnet_Profile_BasicAccess];


GO
PRINT N'Dropping [aspnet_Profile_FullAccess]...';


GO
DROP SCHEMA [aspnet_Profile_FullAccess];


GO
PRINT N'Dropping [aspnet_Profile_ReportingAccess]...';


GO
DROP SCHEMA [aspnet_Profile_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Roles_BasicAccess]...';


GO
DROP SCHEMA [aspnet_Roles_BasicAccess];


GO
PRINT N'Dropping [aspnet_Roles_FullAccess]...';


GO
DROP SCHEMA [aspnet_Roles_FullAccess];


GO
PRINT N'Dropping [aspnet_Roles_ReportingAccess]...';


GO
DROP SCHEMA [aspnet_Roles_ReportingAccess];


GO
PRINT N'Dropping [aspnet_WebEvent_FullAccess]...';


GO
DROP SCHEMA [aspnet_WebEvent_FullAccess];


GO
PRINT N'Dropping [aspnet_Membership_BasicAccess]...';


GO
DROP ROLE [aspnet_Membership_BasicAccess];


GO
PRINT N'Dropping [aspnet_Membership_FullAccess]...';


GO
DROP ROLE [aspnet_Membership_FullAccess];


GO
PRINT N'Dropping [aspnet_Membership_ReportingAccess]...';


GO
DROP ROLE [aspnet_Membership_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Personalization_BasicAccess]...';


GO
DROP ROLE [aspnet_Personalization_BasicAccess];


GO
PRINT N'Dropping [aspnet_Personalization_FullAccess]...';


GO
DROP ROLE [aspnet_Personalization_FullAccess];


GO
PRINT N'Dropping [aspnet_Personalization_ReportingAccess]...';


GO
DROP ROLE [aspnet_Personalization_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Profile_BasicAccess]...';


GO
DROP ROLE [aspnet_Profile_BasicAccess];


GO
PRINT N'Dropping [aspnet_Profile_FullAccess]...';


GO
DROP ROLE [aspnet_Profile_FullAccess];


GO
PRINT N'Dropping [aspnet_Profile_ReportingAccess]...';


GO
DROP ROLE [aspnet_Profile_ReportingAccess];


GO
PRINT N'Dropping [aspnet_Roles_BasicAccess]...';


GO
DROP ROLE [aspnet_Roles_BasicAccess];


GO
PRINT N'Dropping [aspnet_Roles_FullAccess]...';


GO
DROP ROLE [aspnet_Roles_FullAccess];


GO
PRINT N'Dropping [aspnet_Roles_ReportingAccess]...';


GO
DROP ROLE [aspnet_Roles_ReportingAccess];


GO
PRINT N'Dropping [aspnet_WebEvent_FullAccess]...';


GO
DROP ROLE [aspnet_WebEvent_FullAccess];


GO
PRINT N'Starting rebuilding table [dbo].[tblAssemblies]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblAssemblies] (
    [KPAssemblyID]        INT            IDENTITY (1, 1) NOT NULL,
    [KFCategoryID]        INT            NULL,
    [AssemblyNumber]      NVARCHAR (MAX) NULL,
    [AssemblyDescription] NVARCHAR (MAX) NULL,
    [Discount]            DECIMAL (5, 4) NULL,
    [Markup]              DECIMAL (5, 4) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblAssemblies1] PRIMARY KEY CLUSTERED ([KPAssemblyID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblAssemblies])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblAssemblies] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblAssemblies] ([KPAssemblyID], [KFCategoryID], [AssemblyNumber], [AssemblyDescription], [Discount], [Markup])
        SELECT   [KPAssemblyID],
                 [KFCategoryID],
                 [AssemblyNumber],
                 [AssemblyDescription],
                 [Discount],
                 [Markup]
        FROM     [dbo].[tblAssemblies]
        ORDER BY [KPAssemblyID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblAssemblies] OFF;
    END

DROP TABLE [dbo].[tblAssemblies];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblAssemblies]', N'tblAssemblies';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblAssemblies1]', N'PK_dbo.tblAssemblies', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblAssemblies].[IX_KFCategoryID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFCategoryID]
    ON [dbo].[tblAssemblies]([KFCategoryID] ASC);


GO
PRINT N'Altering [dbo].[tblCountySalesTaxRates]...';


GO
ALTER TABLE [dbo].[tblCountySalesTaxRates] ALTER COLUMN [CountyName] NVARCHAR (MAX) NULL;

ALTER TABLE [dbo].[tblCountySalesTaxRates] ALTER COLUMN [SalesTaxRate] DECIMAL (6, 5) NOT NULL;


GO
PRINT N'Altering [dbo].[tblCustomerPartSpecials]...';


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] DROP COLUMN [Discount_Original_Float];


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] ALTER COLUMN [Discount] DECIMAL (5, 4) NOT NULL;


GO
/*
The column [dbo].[tblCustomers].[Billingemail] is being dropped, data loss could occur.

The column [dbo].[tblCustomers].[CustomerLogo] is being dropped, data loss could occur.

The column [dbo].[tblCustomers].[DiscountID] is being dropped, data loss could occur.

The column [dbo].[tblCustomers].[KFVendorID] is being dropped, data loss could occur.

The column [dbo].[tblCustomers].[LogoBinary] is being dropped, data loss could occur.

The column [dbo].[tblCustomers].[LogoText] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblCustomers]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblCustomers] (
    [KPCustomerID]                 INT            IDENTITY (1, 1) NOT NULL,
    [QuickbooksID]                 NVARCHAR (MAX) NULL,
    [APINumberRequired]            BIT            NOT NULL,
    [DefaultSalesTaxRate]          DECIMAL (6, 5) NULL,
    [CountySalesTaxRateID]         INT            NULL,
    [UsesQuickbooksRunningInvoice] BIT            NOT NULL,
    [QbInvoiceClassID]             INT            NOT NULL,
    [UsesInventory]                BIT            NOT NULL,
    [CustomerName]                 NVARCHAR (MAX) NULL,
    [Address1]                     NVARCHAR (MAX) NULL,
    [Address2]                     NVARCHAR (MAX) NULL,
    [City]                         NVARCHAR (MAX) NULL,
    [State]                        NVARCHAR (MAX) NULL,
    [Zip]                          NVARCHAR (MAX) NULL,
    [Website]                      NVARCHAR (MAX) NULL,
    [Phone]                        NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblCustomers1] PRIMARY KEY CLUSTERED ([KPCustomerID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblCustomers])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblCustomers] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblCustomers] ([KPCustomerID], [CustomerName], [Address1], [Address2], [City], [State], [Zip], [Website], [Phone], [APINumberRequired], [DefaultSalesTaxRate], [CountySalesTaxRateID], [QuickbooksID], [UsesQuickbooksRunningInvoice], [QbInvoiceClassID], [UsesInventory])
        SELECT   [KPCustomerID],
                 [CustomerName],
                 [Address1],
                 [Address2],
                 [City],
                 [State],
                 [Zip],
                 [Website],
                 [Phone],
                 [APINumberRequired],
                 [DefaultSalesTaxRate],
                 [CountySalesTaxRateID],
                 [QuickbooksID],
                 [UsesQuickbooksRunningInvoice],
                 [QbInvoiceClassID],
                 [UsesInventory]
        FROM     [dbo].[tblCustomers]
        ORDER BY [KPCustomerID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblCustomers] OFF;
    END

DROP TABLE [dbo].[tblCustomers];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblCustomers]', N'tblCustomers';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblCustomers1]', N'PK_dbo.tblCustomers', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblCustomers].[IX_CountySalesTaxRateID]...';


GO
CREATE NONCLUSTERED INDEX [IX_CountySalesTaxRateID]
    ON [dbo].[tblCustomers]([CountySalesTaxRateID] ASC);


GO
PRINT N'Creating [dbo].[tblCustomers].[IX_QbInvoiceClassID]...';


GO
CREATE NONCLUSTERED INDEX [IX_QbInvoiceClassID]
    ON [dbo].[tblCustomers]([QbInvoiceClassID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblDeliveryTicketImageUploads]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblDeliveryTicketImageUploads] (
    [DeliveryTicketImageUploadID] INT            IDENTITY (1, 1) NOT NULL,
    [DeliveryTicketID]            INT            NOT NULL,
    [LargeImageName]              NVARCHAR (MAX) NULL,
    [SmallImageName]              NVARCHAR (MAX) NULL,
    [AppRelativePath]             NVARCHAR (MAX) NULL,
    [MimeType]                    NVARCHAR (MAX) NULL,
    [UploadedOn]                  DATETIME       NOT NULL,
    [UploadedBy]                  NVARCHAR (MAX) NULL,
    [Note]                        NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblDeliveryTicketImageUploads1] PRIMARY KEY CLUSTERED ([DeliveryTicketImageUploadID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblDeliveryTicketImageUploads])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDeliveryTicketImageUploads] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblDeliveryTicketImageUploads] ([DeliveryTicketImageUploadID], [DeliveryTicketID], [LargeImageName], [SmallImageName], [AppRelativePath], [MimeType], [UploadedOn], [UploadedBy], [Note])
        SELECT   [DeliveryTicketImageUploadID],
                 [DeliveryTicketID],
                 [LargeImageName],
                 [SmallImageName],
                 [AppRelativePath],
                 [MimeType],
                 [UploadedOn],
                 [UploadedBy],
                 [Note]
        FROM     [dbo].[tblDeliveryTicketImageUploads]
        ORDER BY [DeliveryTicketImageUploadID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDeliveryTicketImageUploads] OFF;
    END

DROP TABLE [dbo].[tblDeliveryTicketImageUploads];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblDeliveryTicketImageUploads]', N'tblDeliveryTicketImageUploads';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblDeliveryTicketImageUploads1]', N'PK_dbo.tblDeliveryTicketImageUploads', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblDeliveryTicketImageUploads].[IX_DeliveryTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_DeliveryTicketID]
    ON [dbo].[tblDeliveryTicketImageUploads]([DeliveryTicketID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblLeaseLocations]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblLeaseLocations] (
    [KPLeaseID]         INT            IDENTITY (1, 1) NOT NULL,
    [LocationName]      NVARCHAR (MAX) NULL,
    [IgnoreInReporting] BIT            NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblLeaseLocations1] PRIMARY KEY CLUSTERED ([KPLeaseID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblLeaseLocations])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblLeaseLocations] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblLeaseLocations] ([KPLeaseID], [LocationName], [IgnoreInReporting])
        SELECT   [KPLeaseID],
                 [LocationName],
                 [IgnoreInReporting]
        FROM     [dbo].[tblLeaseLocations]
        ORDER BY [KPLeaseID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblLeaseLocations] OFF;
    END

DROP TABLE [dbo].[tblLeaseLocations];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblLeaseLocations]', N'tblLeaseLocations';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblLeaseLocations1]', N'PK_dbo.tblLeaseLocations', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
/*
The column [dbo].[tblLineItems].[ApprovedRepair] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[Comments] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[KFRepairedBy] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[PartLookup] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[Quantity_Int] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[ReasonRepaired] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[RepairCode] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[SalesTaxCity] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[SalesTaxRate] is being dropped, data loss could occur.

The column [dbo].[tblLineItems].[UnitPrice_Old] is being dropped, data loss could occur.

The column Quantity on table [dbo].[tblLineItems] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The type for column Quantity in table [dbo].[tblLineItems] is currently  DECIMAL (18, 4) NULL but is being changed to  DECIMAL (18, 2) NOT NULL. Data loss could occur.

The column UnitDiscount on table [dbo].[tblLineItems] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column UnitPrice on table [dbo].[tblLineItems] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblLineItems]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblLineItems] (
    [KPLineItemID]     INT             IDENTITY (1, 1) NOT NULL,
    [KFShopTicketID]   INT             NOT NULL,
    [KFPartID]         INT             NULL,
    [KFRepairTicketID] INT             NULL,
    [SortOrder]        INT             NULL,
    [SalesTax]         BIT             NULL,
    [Quantity]         DECIMAL (18, 2) NOT NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [UnitPrice]        DECIMAL (18, 2) NOT NULL,
    [UnitDiscount]     DECIMAL (5, 4)  NOT NULL,
    [CustomerDiscount] DECIMAL (7, 4)  NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblLineItems1] PRIMARY KEY CLUSTERED ([KPLineItemID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblLineItems])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblLineItems] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblLineItems] ([KPLineItemID], [KFShopTicketID], [KFPartID], [Description], [UnitDiscount], [SalesTax], [SortOrder], [UnitPrice], [KFRepairTicketID], [Quantity], [CustomerDiscount])
        SELECT   [KPLineItemID],
                 [KFShopTicketID],
                 [KFPartID],
                 [Description],
                 [UnitDiscount],
                 [SalesTax],
                 [SortOrder],
                 [UnitPrice],
                 [KFRepairTicketID],
                 CAST ([Quantity] AS DECIMAL (18, 2)),
                 [CustomerDiscount]
        FROM     [dbo].[tblLineItems]
        ORDER BY [KPLineItemID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblLineItems] OFF;
    END

DROP TABLE [dbo].[tblLineItems];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblLineItems]', N'tblLineItems';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblLineItems1]', N'PK_dbo.tblLineItems', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblLineItems].[IX_KFShopTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFShopTicketID]
    ON [dbo].[tblLineItems]([KFShopTicketID] ASC);


GO
PRINT N'Creating [dbo].[tblLineItems].[IX_KFPartID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartID]
    ON [dbo].[tblLineItems]([KFPartID] ASC);


GO
PRINT N'Creating [dbo].[tblLineItems].[IX_KFRepairTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFRepairTicketID]
    ON [dbo].[tblLineItems]([KFRepairTicketID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblMaterials]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblMaterials] (
    [KPMaterialsID] INT            IDENTITY (1, 1) NOT NULL,
    [MaterialsName] NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblMaterials1] PRIMARY KEY CLUSTERED ([KPMaterialsID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblMaterials])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblMaterials] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblMaterials] ([KPMaterialsID], [MaterialsName])
        SELECT   [KPMaterialsID],
                 [MaterialsName]
        FROM     [dbo].[tblMaterials]
        ORDER BY [KPMaterialsID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblMaterials] OFF;
    END

DROP TABLE [dbo].[tblMaterials];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblMaterials]', N'tblMaterials';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblMaterials1]', N'PK_dbo.tblMaterials', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Starting rebuilding table [dbo].[tblPartRuntimes]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPartRuntimes] (
    [PartRuntimeID]              INT      IDENTITY (1, 1) NOT NULL,
    [PumpID]                     INT      NOT NULL,
    [TemplatePartDefID]          INT      NOT NULL,
    [Start]                      DATETIME NULL,
    [Finish]                     DATETIME NULL,
    [TotalDaysInGround]          INT      NULL,
    [RuntimeStartedByTicketID]   INT      NULL,
    [RuntimeEndedByInspectionID] INT      NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPartRuntimes1] PRIMARY KEY CLUSTERED ([PartRuntimeID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPartRuntimes])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartRuntimes] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPartRuntimes] ([PartRuntimeID], [PumpID], [TemplatePartDefID], [Start], [Finish], [TotalDaysInGround], [RuntimeStartedByTicketID], [RuntimeEndedByInspectionID])
        SELECT   [PartRuntimeID],
                 [PumpID],
                 [TemplatePartDefID],
                 [Start],
                 [Finish],
                 [TotalDaysInGround],
                 [RuntimeStartedByTicketID],
                 [RuntimeEndedByInspectionID]
        FROM     [dbo].[tblPartRuntimes]
        ORDER BY [PartRuntimeID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartRuntimes] OFF;
    END

DROP TABLE [dbo].[tblPartRuntimes];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPartRuntimes]', N'tblPartRuntimes';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPartRuntimes1]', N'PK_dbo.tblPartRuntimes', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblPartRuntimes].[IX_PumpID]...';


GO
CREATE NONCLUSTERED INDEX [IX_PumpID]
    ON [dbo].[tblPartRuntimes]([PumpID] ASC);


GO
PRINT N'Creating [dbo].[tblPartRuntimes].[IX_TemplatePartDefID]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplatePartDefID]
    ON [dbo].[tblPartRuntimes]([TemplatePartDefID] ASC);


GO
PRINT N'Creating [dbo].[tblPartRuntimes].[IX_RuntimeStartedByTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RuntimeStartedByTicketID]
    ON [dbo].[tblPartRuntimes]([RuntimeStartedByTicketID] ASC);


GO
PRINT N'Creating [dbo].[tblPartRuntimes].[IX_RuntimeEndedByInspectionID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RuntimeEndedByInspectionID]
    ON [dbo].[tblPartRuntimes]([RuntimeEndedByInspectionID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblPartRuntimeSegments]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPartRuntimeSegments] (
    [PartRuntimeSegmentID]     INT      IDENTITY (1, 1) NOT NULL,
    [RuntimeID]                INT      NOT NULL,
    [Start]                    DATETIME NULL,
    [Finish]                   DATETIME NULL,
    [LengthInDays]             INT      NULL,
    [SegmentStartedByTicketID] INT      NULL,
    [SegmentEndedByTicketID]   INT      NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPartRuntimeSegments1] PRIMARY KEY CLUSTERED ([PartRuntimeSegmentID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPartRuntimeSegments])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartRuntimeSegments] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPartRuntimeSegments] ([PartRuntimeSegmentID], [RuntimeID], [Start], [Finish], [LengthInDays], [SegmentStartedByTicketID], [SegmentEndedByTicketID])
        SELECT   [PartRuntimeSegmentID],
                 [RuntimeID],
                 [Start],
                 [Finish],
                 [LengthInDays],
                 [SegmentStartedByTicketID],
                 [SegmentEndedByTicketID]
        FROM     [dbo].[tblPartRuntimeSegments]
        ORDER BY [PartRuntimeSegmentID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartRuntimeSegments] OFF;
    END

DROP TABLE [dbo].[tblPartRuntimeSegments];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPartRuntimeSegments]', N'tblPartRuntimeSegments';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPartRuntimeSegments1]', N'PK_dbo.tblPartRuntimeSegments', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblPartRuntimeSegments].[IX_RuntimeID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RuntimeID]
    ON [dbo].[tblPartRuntimeSegments]([RuntimeID] ASC);


GO
PRINT N'Creating [dbo].[tblPartRuntimeSegments].[IX_SegmentStartedByTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_SegmentStartedByTicketID]
    ON [dbo].[tblPartRuntimeSegments]([SegmentStartedByTicketID] ASC);


GO
PRINT N'Creating [dbo].[tblPartRuntimeSegments].[IX_SegmentEndedByTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_SegmentEndedByTicketID]
    ON [dbo].[tblPartRuntimeSegments]([SegmentEndedByTicketID] ASC);


GO
/*
The column [dbo].[tblParts].[CheckedCategory] is being dropped, data loss could occur.

The column [dbo].[tblParts].[CheckedPrice] is being dropped, data loss could occur.

The column [dbo].[tblParts].[CheckedSoldBy] is being dropped, data loss could occur.

The column [dbo].[tblParts].[DiscountID] is being dropped, data loss could occur.

The column [dbo].[tblParts].[DiscountPrice] is being dropped, data loss could occur.

The column [dbo].[tblParts].[ElectCategory] is being dropped, data loss could occur.

The column [dbo].[tblParts].[FilterAssembly] is being dropped, data loss could occur.

The column [dbo].[tblParts].[IsAssembly] is being dropped, data loss could occur.

The column [dbo].[tblParts].[KFVendorLocationID] is being dropped, data loss could occur.

The column [dbo].[tblParts].[Tracked] is being dropped, data loss could occur.

The column [dbo].[tblParts].[VendorName] is being dropped, data loss could occur.

The column [dbo].[tblParts].[WeightOunces] is being dropped, data loss could occur.

The column [dbo].[tblParts].[WeightPounds] is being dropped, data loss could occur.

The column Cost on table [dbo].[tblParts] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Discount on table [dbo].[tblParts] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.

The column Markup on table [dbo].[tblParts] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblParts]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblParts] (
    [KPPartID]               INT             IDENTITY (1, 1) NOT NULL,
    [KFPartCategory]         INT             NULL,
    [KFAssemblyID]           INT             NULL,
    [KFOptionID]             INT             NULL,
    [KFMaterialsID]          INT             NULL,
    [PartNumber]             NVARCHAR (MAX)  NULL,
    [QuickbooksID]           NVARCHAR (MAX)  NULL,
    [Description]            NVARCHAR (MAX)  NULL,
    [Active]                 BIT             NOT NULL,
    [Manufacturer]           NVARCHAR (MAX)  NULL,
    [ManufacturerPartNumber] NVARCHAR (MAX)  NULL,
    [Cost]                   DECIMAL (10, 2) NOT NULL,
    [Markup]                 DECIMAL (5, 4)  NOT NULL,
    [Discount]               DECIMAL (5, 4)  NOT NULL,
    [Taxable]                BIT             NOT NULL,
    [PriceLastUpdated]       DATETIME        NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblParts1] PRIMARY KEY CLUSTERED ([KPPartID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblParts])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblParts] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblParts] ([KPPartID], [KFPartCategory], [KFMaterialsID], [KFAssemblyID], [KFOptionID], [PartNumber], [Description], [Active], [Manufacturer], [ManufacturerPartNumber], [Discount], [Cost], [Markup], [QuickbooksID], [PriceLastUpdated], [Taxable])
        SELECT   [KPPartID],
                 [KFPartCategory],
                 [KFMaterialsID],
                 [KFAssemblyID],
                 [KFOptionID],
                 [PartNumber],
                 [Description],
                 [Active],
                 [Manufacturer],
                 [ManufacturerPartNumber],
                 [Discount],
                 [Cost],
                 [Markup],
                 [QuickbooksID],
                 [PriceLastUpdated],
                 [Taxable]
        FROM     [dbo].[tblParts]
        ORDER BY [KPPartID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblParts] OFF;
    END

DROP TABLE [dbo].[tblParts];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblParts]', N'tblParts';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblParts1]', N'PK_dbo.tblParts', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblParts].[IX_KFPartCategory]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartCategory]
    ON [dbo].[tblParts]([KFPartCategory] ASC);


GO
PRINT N'Creating [dbo].[tblParts].[IX_KFAssemblyID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFAssemblyID]
    ON [dbo].[tblParts]([KFAssemblyID] ASC);


GO
PRINT N'Creating [dbo].[tblParts].[IX_KFOptionID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFOptionID]
    ON [dbo].[tblParts]([KFOptionID] ASC);


GO
PRINT N'Creating [dbo].[tblParts].[IX_KFMaterialsID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFMaterialsID]
    ON [dbo].[tblParts]([KFMaterialsID] ASC);


GO
/*
The column [dbo].[tblPartsAssemblies].[Filter] is being dropped, data loss could occur.

The column PartsQuantity on table [dbo].[tblPartsAssemblies] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblPartsAssemblies]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPartsAssemblies] (
    [KPPartsAssembliesID] INT IDENTITY (1, 1) NOT NULL,
    [KFAssemblyID]        INT NULL,
    [KFPartsID]           INT NULL,
    [PartsQuantity]       INT NOT NULL,
    [SortOrder]           INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPartsAssemblies1] PRIMARY KEY CLUSTERED ([KPPartsAssembliesID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPartsAssemblies])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartsAssemblies] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPartsAssemblies] ([KPPartsAssembliesID], [KFAssemblyID], [KFPartsID], [PartsQuantity], [SortOrder])
        SELECT   [KPPartsAssembliesID],
                 [KFAssemblyID],
                 [KFPartsID],
                 [PartsQuantity],
                 [SortOrder]
        FROM     [dbo].[tblPartsAssemblies]
        ORDER BY [KPPartsAssembliesID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPartsAssemblies] OFF;
    END

DROP TABLE [dbo].[tblPartsAssemblies];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPartsAssemblies]', N'tblPartsAssemblies';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPartsAssemblies1]', N'PK_dbo.tblPartsAssemblies', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblPartsAssemblies].[IX_KFAssemblyID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFAssemblyID]
    ON [dbo].[tblPartsAssemblies]([KFAssemblyID] ASC);


GO
PRINT N'Creating [dbo].[tblPartsAssemblies].[IX_KFPartsID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartsID]
    ON [dbo].[tblPartsAssemblies]([KFPartsID] ASC);


GO
PRINT N'Altering [dbo].[tblPartsCategory]...';


GO
ALTER TABLE [dbo].[tblPartsCategory] DROP COLUMN [upsizeTS];


GO
ALTER TABLE [dbo].[tblPartsCategory] ALTER COLUMN [CategoryDescription] NVARCHAR (MAX) NULL;

ALTER TABLE [dbo].[tblPartsCategory] ALTER COLUMN [CategoryName] NVARCHAR (MAX) NULL;


GO
PRINT N'Creating [dbo].[PK_dbo.tblPartsCategory]...';


GO
ALTER TABLE [dbo].[tblPartsCategory]
    ADD CONSTRAINT [PK_dbo.tblPartsCategory] PRIMARY KEY CLUSTERED ([KPCategoryID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblPumpRuntimes]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPumpRuntimes] (
    [PumpRuntimeID]            INT      IDENTITY (1, 1) NOT NULL,
    [PumpID]                   INT      NOT NULL,
    [Start]                    DATETIME NULL,
    [Finish]                   DATETIME NULL,
    [LengthInDays]             INT      NULL,
    [RuntimeStartedByTicketID] INT      NULL,
    [RuntimeEndedByTicketID]   INT      NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPumpRuntimes1] PRIMARY KEY CLUSTERED ([PumpRuntimeID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPumpRuntimes])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumpRuntimes] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPumpRuntimes] ([PumpRuntimeID], [PumpID], [Start], [Finish], [LengthInDays], [RuntimeStartedByTicketID], [RuntimeEndedByTicketID])
        SELECT   [PumpRuntimeID],
                 [PumpID],
                 [Start],
                 [Finish],
                 [LengthInDays],
                 [RuntimeStartedByTicketID],
                 [RuntimeEndedByTicketID]
        FROM     [dbo].[tblPumpRuntimes]
        ORDER BY [PumpRuntimeID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumpRuntimes] OFF;
    END

DROP TABLE [dbo].[tblPumpRuntimes];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPumpRuntimes]', N'tblPumpRuntimes';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPumpRuntimes1]', N'PK_dbo.tblPumpRuntimes', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblPumpRuntimes].[IX_PumpID]...';


GO
CREATE NONCLUSTERED INDEX [IX_PumpID]
    ON [dbo].[tblPumpRuntimes]([PumpID] ASC);


GO
PRINT N'Creating [dbo].[tblPumpRuntimes].[IX_RuntimeStartedByTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RuntimeStartedByTicketID]
    ON [dbo].[tblPumpRuntimes]([RuntimeStartedByTicketID] ASC);


GO
PRINT N'Creating [dbo].[tblPumpRuntimes].[IX_RuntimeEndedByTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RuntimeEndedByTicketID]
    ON [dbo].[tblPumpRuntimes]([RuntimeEndedByTicketID] ASC);


GO
/*
The column [dbo].[tblPumps].[KFCustomerID] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblPumps]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPumps] (
    [KPPumpID]         INT            IDENTITY (1, 1) NOT NULL,
    [KFWellLocationID] INT            NULL,
    [KFPumpTemplateID] INT            NULL,
    [PumpNumber]       NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPumps1] PRIMARY KEY CLUSTERED ([KPPumpID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPumps])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumps] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPumps] ([KPPumpID], [PumpNumber], [KFWellLocationID], [KFPumpTemplateID])
        SELECT   [KPPumpID],
                 [PumpNumber],
                 [KFWellLocationID],
                 [KFPumpTemplateID]
        FROM     [dbo].[tblPumps]
        ORDER BY [KPPumpID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumps] OFF;
    END

DROP TABLE [dbo].[tblPumps];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPumps]', N'tblPumps';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPumps1]', N'PK_dbo.tblPumps', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblPumps].[IX_KFWellLocationID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFWellLocationID]
    ON [dbo].[tblPumps]([KFWellLocationID] ASC);


GO
PRINT N'Creating [dbo].[tblPumps].[IX_KFPumpTemplateID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPumpTemplateID]
    ON [dbo].[tblPumps]([KFPumpTemplateID] ASC);


GO
/*
The column [dbo].[tblPumpTemplates].[KFPumpTemplatePartsID] is being dropped, data loss could occur.

The column [dbo].[tblPumpTemplates].[KFVendorLocationID] is being dropped, data loss could occur.

The column [dbo].[tblPumpTemplates].[PumpTemplateSpec] is being dropped, data loss could occur.

The column [dbo].[tblPumpTemplates].[ShowPumpTemplate] is being dropped, data loss could occur.

The type for column Discount in table [dbo].[tblPumpTemplates] is currently  DECIMAL (5, 4) NULL but is being changed to  DECIMAL (18, 2) NULL. Data loss could occur.

The type for column Markup in table [dbo].[tblPumpTemplates] is currently  DECIMAL (5, 4) NULL but is being changed to  DECIMAL (18, 2) NULL. Data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblPumpTemplates]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblPumpTemplates] (
    [KPPumpTemplateID]      INT             IDENTITY (1, 1) NOT NULL,
    [PumpTemplateNumberNew] NVARCHAR (MAX)  NULL,
    [PumpTemplateNumber]    NVARCHAR (MAX)  NULL,
    [BarrelLength]          NVARCHAR (MAX)  NULL,
    [BarrelType]            NVARCHAR (MAX)  NULL,
    [BarrelMaterial]        NVARCHAR (MAX)  NULL,
    [BarrelWasher]          NVARCHAR (MAX)  NULL,
    [PlungerMaterial]       NVARCHAR (MAX)  NULL,
    [PlungerLength]         NVARCHAR (MAX)  NULL,
    [PlungerFit]            NVARCHAR (MAX)  NULL,
    [SeatingLocation]       NVARCHAR (MAX)  NULL,
    [SeatingType]           NVARCHAR (MAX)  NULL,
    [TubingSize]            NVARCHAR (MAX)  NULL,
    [PumpBoreBasic]         NVARCHAR (MAX)  NULL,
    [LowerExtension]        NVARCHAR (MAX)  NULL,
    [UpperExtension]        NVARCHAR (MAX)  NULL,
    [PumpType]              NVARCHAR (MAX)  NULL,
    [HoldDownType]          NVARCHAR (MAX)  NULL,
    [TravelingValve]        NVARCHAR (MAX)  NULL,
    [StandingValve]         NVARCHAR (MAX)  NULL,
    [BallsAndSeats]         NVARCHAR (MAX)  NULL,
    [Cages]                 NVARCHAR (MAX)  NULL,
    [Collet]                NVARCHAR (MAX)  NULL,
    [TopSeals]              NVARCHAR (MAX)  NULL,
    [OnOffTool]             NVARCHAR (MAX)  NULL,
    [SpecialtyItems]        NVARCHAR (MAX)  NULL,
    [PonyRods]              NVARCHAR (MAX)  NULL,
    [Strainers]             NVARCHAR (MAX)  NULL,
    [KnockOut]              NVARCHAR (MAX)  NULL,
    [Markup]                DECIMAL (18, 2) NULL,
    [Discount]              DECIMAL (18, 2) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblPumpTemplates1] PRIMARY KEY CLUSTERED ([KPPumpTemplateID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblPumpTemplates])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumpTemplates] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblPumpTemplates] ([KPPumpTemplateID], [PumpTemplateNumberNew], [PumpTemplateNumber], [Discount], [Markup], [TubingSize], [PumpBoreBasic], [BarrelLength], [LowerExtension], [UpperExtension], [PumpType], [BarrelType], [BarrelMaterial], [SeatingLocation], [SeatingType], [PlungerMaterial], [PlungerLength], [PlungerFit], [HoldDownType], [TravelingValve], [StandingValve], [BallsAndSeats], [Cages], [BarrelWasher], [Collet], [TopSeals], [OnOffTool], [SpecialtyItems], [PonyRods], [Strainers], [KnockOut])
        SELECT   [KPPumpTemplateID],
                 [PumpTemplateNumberNew],
                 [PumpTemplateNumber],
                 CAST ([Discount] AS DECIMAL (18, 2)),
                 CAST ([Markup] AS DECIMAL (18, 2)),
                 [TubingSize],
                 [PumpBoreBasic],
                 [BarrelLength],
                 [LowerExtension],
                 [UpperExtension],
                 [PumpType],
                 [BarrelType],
                 [BarrelMaterial],
                 [SeatingLocation],
                 [SeatingType],
                 [PlungerMaterial],
                 [PlungerLength],
                 [PlungerFit],
                 [HoldDownType],
                 [TravelingValve],
                 [StandingValve],
                 [BallsAndSeats],
                 [Cages],
                 [BarrelWasher],
                 [Collet],
                 [TopSeals],
                 [OnOffTool],
                 [SpecialtyItems],
                 [PonyRods],
                 [Strainers],
                 [KnockOut]
        FROM     [dbo].[tblPumpTemplates]
        ORDER BY [KPPumpTemplateID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblPumpTemplates] OFF;
    END

DROP TABLE [dbo].[tblPumpTemplates];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblPumpTemplates]', N'tblPumpTemplates';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblPumpTemplates1]', N'PK_dbo.tblPumpTemplates', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[tblQbInvoiceClasses]...';


GO
ALTER TABLE [dbo].[tblQbInvoiceClasses] ALTER COLUMN [FullName] NVARCHAR (MAX) NULL;


GO
/*
The column [dbo].[tblRepairTickets].[IsAssembly] is being dropped, data loss could occur.

The column [dbo].[tblRepairTickets].[Notes] is being dropped, data loss could occur.

The column [dbo].[tblRepairTickets].[TimeRepaired] is being dropped, data loss could occur.

The column KFShopTicketID on table [dbo].[tblRepairTickets] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblRepairTickets]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblRepairTickets] (
    [KPRepairID]                  INT            IDENTITY (1, 1) NOT NULL,
    [KFShopTicketID]              INT            NOT NULL,
    [KFParentAssemblyID]          INT            NULL,
    [IsSplitAssembly]             BIT            NULL,
    [IsConvertible]               BIT            NULL,
    [KFPartReplacedID]            INT            NULL,
    [ReplaceQuantity]             INT            NULL,
    [KFPartFailedID]              INT            NULL,
    [Quantity]                    INT            NULL,
    [Status]                      NVARCHAR (MAX) NULL,
    [ReasonRepaired]              NVARCHAR (MAX) NULL,
    [Sort]                        INT            NULL,
    [ReplacedWithInventoryPartID] INT            NULL,
    [TemplatePartDefID]           INT            NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblRepairTickets1] PRIMARY KEY CLUSTERED ([KPRepairID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblRepairTickets])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblRepairTickets] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblRepairTickets] ([KPRepairID], [KFShopTicketID], [Quantity], [Status], [ReasonRepaired], [KFPartFailedID], [Sort], [KFPartReplacedID], [ReplaceQuantity], [KFParentAssemblyID], [IsSplitAssembly], [IsConvertible], [ReplacedWithInventoryPartID], [TemplatePartDefID])
        SELECT   [KPRepairID],
                 [KFShopTicketID],
                 [Quantity],
                 [Status],
                 [ReasonRepaired],
                 [KFPartFailedID],
                 [Sort],
                 [KFPartReplacedID],
                 [ReplaceQuantity],
                 [KFParentAssemblyID],
                 [IsSplitAssembly],
                 [IsConvertible],
                 [ReplacedWithInventoryPartID],
                 [TemplatePartDefID]
        FROM     [dbo].[tblRepairTickets]
        ORDER BY [KPRepairID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblRepairTickets] OFF;
    END

DROP TABLE [dbo].[tblRepairTickets];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblRepairTickets]', N'tblRepairTickets';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblRepairTickets1]', N'PK_dbo.tblRepairTickets', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_KFShopTicketID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFShopTicketID]
    ON [dbo].[tblRepairTickets]([KFShopTicketID] ASC);


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_KFParentAssemblyID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFParentAssemblyID]
    ON [dbo].[tblRepairTickets]([KFParentAssemblyID] ASC);


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_KFPartReplacedID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartReplacedID]
    ON [dbo].[tblRepairTickets]([KFPartReplacedID] ASC);


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_KFPartFailedID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartFailedID]
    ON [dbo].[tblRepairTickets]([KFPartFailedID] ASC);


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_ReplacedWithInventoryPartID]...';


GO
CREATE NONCLUSTERED INDEX [IX_ReplacedWithInventoryPartID]
    ON [dbo].[tblRepairTickets]([ReplacedWithInventoryPartID] ASC);


GO
PRINT N'Creating [dbo].[tblRepairTickets].[IX_TemplatePartDefID]...';


GO
CREATE NONCLUSTERED INDEX [IX_TemplatePartDefID]
    ON [dbo].[tblRepairTickets]([TemplatePartDefID] ASC);


GO
/*
The column [dbo].[tblShopTickets].[FilterWells] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[IsUserTypedRate] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[KFPumpDispatched] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[KFPumpFailed] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[KFRepairTicketID] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[KFVendorLocationID] is being dropped, data loss could occur.

The column [dbo].[tblShopTickets].[RepairedDate] is being dropped, data loss could occur.

The type for column PlungerBarrelWear in table [dbo].[tblShopTickets] is currently  NCHAR (306) NULL but is being changed to  CHAR (306) NULL. Data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblShopTickets]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblShopTickets] (
    [KPShopTicketID]            INT             IDENTITY (1, 1) NOT NULL,
    [KFWellLocationID]          INT             NULL,
    [KFCustomerID]              INT             NULL,
    [PumpFailedDate]            DATETIME        NULL,
    [PumpFailedID]              INT             NULL,
    [PumpDispatchedDate]        DATETIME        NULL,
    [PumpDispatchedID]          INT             NULL,
    [SalesTaxRate]              DECIMAL (6, 5)  NOT NULL,
    [CountySalesTaxRateID]      INT             NULL,
    [IsSignificantDesignChange] BIT             NULL,
    [CompletedBy]               NVARCHAR (MAX)  NULL,
    [RepairedBy]                NVARCHAR (MAX)  NULL,
    [ReasonStillOpen]           NVARCHAR (MAX)  NULL,
    [RepairMode]                INT             NOT NULL,
    [SignatureName]             NVARCHAR (MAX)  NULL,
    [SignatureDate]             DATETIME        NULL,
    [SignatureCompanyName]      NVARCHAR (MAX)  NULL,
    [Signature]                 VARBINARY (MAX) NULL,
    [TicketDate]                DATETIME        NULL,
    [CloseTicket]               BIT             NULL,
    [ShipVia]                   NVARCHAR (MAX)  NULL,
    [PONumber]                  NVARCHAR (MAX)  NULL,
    [OrderDate]                 DATETIME        NULL,
    [OrderTime]                 DATETIME        NULL,
    [ShipDate]                  DATETIME        NULL,
    [ShipTime]                  DATETIME        NULL,
    [OrderedBy]                 NVARCHAR (MAX)  NULL,
    [LastPull]                  NVARCHAR (MAX)  NULL,
    [Stroke]                    NVARCHAR (MAX)  NULL,
    [HoldDown]                  NVARCHAR (MAX)  NULL,
    [Notes]                     NVARCHAR (MAX)  NULL,
    [SortOrder]                 INT             NULL,
    [FilterAssembly]            INT             NULL,
    [Quote]                     BIT             NULL,
    [QuickbooksID]              NVARCHAR (MAX)  NULL,
    [QuickbooksInvoiceNumber]   NVARCHAR (MAX)  NULL,
    [InvoiceStatus]             INT             NOT NULL,
    [RepairComplete]            BIT             NOT NULL,
    [PlungerBarrelWear]         CHAR (306)      NULL,
    [InvBarrel]                 NVARCHAR (MAX)  NULL,
    [InvSVCages]                NVARCHAR (MAX)  NULL,
    [InvDVCages]                NVARCHAR (MAX)  NULL,
    [InvSVSeats]                NVARCHAR (MAX)  NULL,
    [InvDVSeats]                NVARCHAR (MAX)  NULL,
    [InvSVBalls]                NVARCHAR (MAX)  NULL,
    [InvDVBalls]                NVARCHAR (MAX)  NULL,
    [InvHoldDown]               NVARCHAR (MAX)  NULL,
    [InvValveRod]               NVARCHAR (MAX)  NULL,
    [InvPlunger]                NVARCHAR (MAX)  NULL,
    [InvPTVCages]               NVARCHAR (MAX)  NULL,
    [InvPDVCages]               NVARCHAR (MAX)  NULL,
    [InvPTVSeats]               NVARCHAR (MAX)  NULL,
    [InvPDVSeats]               NVARCHAR (MAX)  NULL,
    [InvPTVBalls]               NVARCHAR (MAX)  NULL,
    [InvPDVBalls]               NVARCHAR (MAX)  NULL,
    [InvRodGuide]               NVARCHAR (MAX)  NULL,
    [InvTypeBallandSeat]        NVARCHAR (MAX)  NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblShopTickets1] PRIMARY KEY CLUSTERED ([KPShopTicketID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblShopTickets])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblShopTickets] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblShopTickets] ([KPShopTicketID], [KFCustomerID], [KFWellLocationID], [TicketDate], [CloseTicket], [ShipVia], [PONumber], [OrderDate], [ShipDate], [OrderedBy], [LastPull], [Stroke], [HoldDown], [Notes], [SortOrder], [FilterAssembly], [InvBarrel], [InvSVCages], [InvDVCages], [InvSVSeats], [InvDVSeats], [InvSVBalls], [InvDVBalls], [InvHoldDown], [InvValveRod], [InvPlunger], [InvPTVCages], [InvPDVCages], [InvPTVSeats], [InvPDVSeats], [InvPTVBalls], [InvPDVBalls], [InvRodGuide], [InvTypeBallandSeat], [CompletedBy], [RepairedBy], [ReasonStillOpen], [PumpFailedDate], [PumpFailedID], [PumpDispatchedDate], [PumpDispatchedID], [SignatureName], [SignatureDate], [Signature], [Quote], [SignatureCompanyName], [IsSignificantDesignChange], [QuickbooksID], [QuickbooksInvoiceNumber], [CountySalesTaxRateID], [InvoiceStatus], [OrderTime], [ShipTime], [PlungerBarrelWear], [RepairMode], [RepairComplete], [SalesTaxRate])
        SELECT   [KPShopTicketID],
                 [KFCustomerID],
                 [KFWellLocationID],
                 [TicketDate],
                 [CloseTicket],
                 [ShipVia],
                 [PONumber],
                 [OrderDate],
                 [ShipDate],
                 [OrderedBy],
                 [LastPull],
                 [Stroke],
                 [HoldDown],
                 [Notes],
                 [SortOrder],
                 [FilterAssembly],
                 [InvBarrel],
                 [InvSVCages],
                 [InvDVCages],
                 [InvSVSeats],
                 [InvDVSeats],
                 [InvSVBalls],
                 [InvDVBalls],
                 [InvHoldDown],
                 [InvValveRod],
                 [InvPlunger],
                 [InvPTVCages],
                 [InvPDVCages],
                 [InvPTVSeats],
                 [InvPDVSeats],
                 [InvPTVBalls],
                 [InvPDVBalls],
                 [InvRodGuide],
                 [InvTypeBallandSeat],
                 [CompletedBy],
                 [RepairedBy],
                 [ReasonStillOpen],
                 [PumpFailedDate],
                 [PumpFailedID],
                 [PumpDispatchedDate],
                 [PumpDispatchedID],
                 [SignatureName],
                 [SignatureDate],
                 [Signature],
                 [Quote],
                 [SignatureCompanyName],
                 [IsSignificantDesignChange],
                 [QuickbooksID],
                 [QuickbooksInvoiceNumber],
                 [CountySalesTaxRateID],
                 [InvoiceStatus],
                 [OrderTime],
                 [ShipTime],
                 [PlungerBarrelWear],
                 [RepairMode],
                 [RepairComplete],
                 [SalesTaxRate]
        FROM     [dbo].[tblShopTickets]
        ORDER BY [KPShopTicketID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblShopTickets] OFF;
    END

DROP TABLE [dbo].[tblShopTickets];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblShopTickets]', N'tblShopTickets';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblShopTickets1]', N'PK_dbo.tblShopTickets', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblShopTickets].[IX_KFWellLocationID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFWellLocationID]
    ON [dbo].[tblShopTickets]([KFWellLocationID] ASC);


GO
PRINT N'Creating [dbo].[tblShopTickets].[IX_KFCustomerID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFCustomerID]
    ON [dbo].[tblShopTickets]([KFCustomerID] ASC);


GO
PRINT N'Creating [dbo].[tblShopTickets].[IX_PumpFailedID]...';


GO
CREATE NONCLUSTERED INDEX [IX_PumpFailedID]
    ON [dbo].[tblShopTickets]([PumpFailedID] ASC);


GO
PRINT N'Creating [dbo].[tblShopTickets].[IX_PumpDispatchedID]...';


GO
CREATE NONCLUSTERED INDEX [IX_PumpDispatchedID]
    ON [dbo].[tblShopTickets]([PumpDispatchedID] ASC);


GO
PRINT N'Creating [dbo].[tblShopTickets].[IX_CountySalesTaxRateID]...';


GO
CREATE NONCLUSTERED INDEX [IX_CountySalesTaxRateID]
    ON [dbo].[tblShopTickets]([CountySalesTaxRateID] ASC);


GO
/*
The column [dbo].[tblTemplatePartsJoin].[NewSortOrder] is being dropped, data loss could occur.

The column Quantity on table [dbo].[tblTemplatePartsJoin] must be changed from NULL to NOT NULL. If the table contains data, the ALTER script may not work. To avoid this issue, you must add values to this column for all rows or mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblTemplatePartsJoin]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblTemplatePartsJoin] (
    [KPTemplatePartsJoinID] INT IDENTITY (1, 1) NOT NULL,
    [KFPartsID]             INT NULL,
    [KFPumpTemplateID]      INT NULL,
    [Quantity]              INT NOT NULL,
    [SortOrder]             INT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblTemplatePartsJoin1] PRIMARY KEY CLUSTERED ([KPTemplatePartsJoinID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblTemplatePartsJoin])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblTemplatePartsJoin] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblTemplatePartsJoin] ([KPTemplatePartsJoinID], [KFPartsID], [KFPumpTemplateID], [Quantity], [SortOrder])
        SELECT   [KPTemplatePartsJoinID],
                 [KFPartsID],
                 [KFPumpTemplateID],
                 [Quantity],
                 [SortOrder]
        FROM     [dbo].[tblTemplatePartsJoin]
        ORDER BY [KPTemplatePartsJoinID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblTemplatePartsJoin] OFF;
    END

DROP TABLE [dbo].[tblTemplatePartsJoin];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblTemplatePartsJoin]', N'tblTemplatePartsJoin';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblTemplatePartsJoin1]', N'PK_dbo.tblTemplatePartsJoin', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblTemplatePartsJoin].[IX_KFPartsID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPartsID]
    ON [dbo].[tblTemplatePartsJoin]([KFPartsID] ASC);


GO
PRINT N'Creating [dbo].[tblTemplatePartsJoin].[IX_KFPumpTemplateID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFPumpTemplateID]
    ON [dbo].[tblTemplatePartsJoin]([KFPumpTemplateID] ASC);


GO
/*
The column [dbo].[tblUsernameCustomerAccess].[UsernameCustomerAccessID] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblUsernameCustomerAccess]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblUsernameCustomerAccess] (
    [UserID]     INT NOT NULL,
    [CustomerID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblUsernameCustomerAccess1] PRIMARY KEY CLUSTERED ([UserID] ASC, [CustomerID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblUsernameCustomerAccess])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_tblUsernameCustomerAccess] ([UserID], [CustomerID])
        SELECT   [UserID],
                 [CustomerID]
        FROM     [dbo].[tblUsernameCustomerAccess]
        ORDER BY [UserID] ASC, [CustomerID] ASC;
    END

DROP TABLE [dbo].[tblUsernameCustomerAccess];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblUsernameCustomerAccess]', N'tblUsernameCustomerAccess';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblUsernameCustomerAccess1]', N'PK_dbo.tblUsernameCustomerAccess', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblUsernameCustomerAccess].[IX_UserID]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[tblUsernameCustomerAccess]([UserID] ASC);


GO
PRINT N'Creating [dbo].[tblUsernameCustomerAccess].[IX_CustomerID]...';


GO
CREATE NONCLUSTERED INDEX [IX_CustomerID]
    ON [dbo].[tblUsernameCustomerAccess]([CustomerID] ASC);


GO
/*
The column [dbo].[tblUserRoles].[UserRoleID] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblUserRoles]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblUserRoles] (
    [RoleID] INT NOT NULL,
    [UserID] INT NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblUserRoles1] PRIMARY KEY CLUSTERED ([RoleID] ASC, [UserID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblUserRoles])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_tblUserRoles] ([RoleID], [UserID])
        SELECT   [RoleID],
                 [UserID]
        FROM     [dbo].[tblUserRoles]
        ORDER BY [RoleID] ASC, [UserID] ASC;
    END

DROP TABLE [dbo].[tblUserRoles];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblUserRoles]', N'tblUserRoles';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblUserRoles1]', N'PK_dbo.tblUserRoles', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblUserRoles].[IX_RoleID]...';


GO
CREATE NONCLUSTERED INDEX [IX_RoleID]
    ON [dbo].[tblUserRoles]([RoleID] ASC);


GO
PRINT N'Creating [dbo].[tblUserRoles].[IX_UserID]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[tblUserRoles]([UserID] ASC);


GO
/*
The column [dbo].[tblWellLocation].[KFVendorLocationID] is being dropped, data loss could occur.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblWellLocation]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblWellLocation] (
    [KPWellLocationID] INT            IDENTITY (1, 1) NOT NULL,
    [KFLeaseID]        INT            NULL,
    [KFCustomerID]     INT            NULL,
    [WellNumber]       NVARCHAR (MAX) NULL,
    [APINumber]        NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblWellLocation1] PRIMARY KEY CLUSTERED ([KPWellLocationID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblWellLocation])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblWellLocation] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblWellLocation] ([KPWellLocationID], [KFLeaseID], [KFCustomerID], [WellNumber], [APINumber])
        SELECT   [KPWellLocationID],
                 [KFLeaseID],
                 [KFCustomerID],
                 [WellNumber],
                 [APINumber]
        FROM     [dbo].[tblWellLocation]
        ORDER BY [KPWellLocationID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblWellLocation] OFF;
    END

DROP TABLE [dbo].[tblWellLocation];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblWellLocation]', N'tblWellLocation';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblWellLocation1]', N'PK_dbo.tblWellLocation', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblWellLocation].[IX_KFLeaseID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFLeaseID]
    ON [dbo].[tblWellLocation]([KFLeaseID] ASC);


GO
PRINT N'Creating [dbo].[tblWellLocation].[IX_KFCustomerID]...';


GO
CREATE NONCLUSTERED INDEX [IX_KFCustomerID]
    ON [dbo].[tblWellLocation]([KFCustomerID] ASC);


GO
PRINT N'Starting rebuilding table [dbo].[tblRoles]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblRoles] (
    [RoleID]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (MAX) NULL,
    [DisplayText] NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_dbo.tblRoles1] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblRoles])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblRoles] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblRoles] ([RoleID], [RoleName], [DisplayText])
        SELECT   [RoleID],
                 [RoleName],
                 [DisplayText]
        FROM     [dbo].[tblRoles]
        ORDER BY [RoleID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblRoles] OFF;
    END

DROP TABLE [dbo].[tblRoles];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblRoles]', N'tblRoles';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_dbo.tblRoles1]', N'PK_dbo.tblRoles', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating [dbo].[tblAcePumpProfiles].[IX_CustomerID]...';


GO
CREATE NONCLUSTERED INDEX [IX_CustomerID]
    ON [dbo].[tblAcePumpProfiles]([CustomerID] ASC);


GO
PRINT N'Creating [dbo].[tblAcePumpProfiles].[IX_UserID]...';


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[tblAcePumpProfiles]([UserID] ASC);


GO
PRINT N'Creating [dbo].[FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID]...';


GO
ALTER TABLE [dbo].[tblCustomers] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID] FOREIGN KEY ([CountySalesTaxRateID]) REFERENCES [dbo].[tblCountySalesTaxRates] ([CountySalesTaxRateID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblPartInstances] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblCustomers_dbo.tblQbInvoiceClasses_QbInvoiceClassID]...';


GO
ALTER TABLE [dbo].[tblCustomers] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblCustomers_dbo.tblQbInvoiceClasses_QbInvoiceClassID] FOREIGN KEY ([QbInvoiceClassID]) REFERENCES [dbo].[tblQbInvoiceClasses] ([QbInvoiceClassID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblLineItems_dbo.tblRepairTickets_KFRepairTicketID]...';


GO
ALTER TABLE [dbo].[tblLineItems] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblLineItems_dbo.tblRepairTickets_KFRepairTicketID] FOREIGN KEY ([KFRepairTicketID]) REFERENCES [dbo].[tblRepairTickets] ([KPRepairID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblLineItems_dbo.tblParts_KFPartID]...';


GO
ALTER TABLE [dbo].[tblLineItems] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblLineItems_dbo.tblParts_KFPartID] FOREIGN KEY ([KFPartID]) REFERENCES [dbo].[tblParts] ([KPPartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblLineItems_dbo.tblShopTickets_KFShopTicketID]...';


GO
ALTER TABLE [dbo].[tblLineItems] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblLineItems_dbo.tblShopTickets_KFShopTicketID] FOREIGN KEY ([KFShopTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID]...';


GO
ALTER TABLE [dbo].[tblCustomerPartSpecials] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID] FOREIGN KEY ([PartID]) REFERENCES [dbo].[tblParts] ([KPPartID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID]...';


GO
ALTER TABLE [dbo].[tblPartInstances] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID] FOREIGN KEY ([PartTemplateID]) REFERENCES [dbo].[tblParts] ([KPPartID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblParts_dbo.tblMaterials_KFMaterialsID]...';


GO
ALTER TABLE [dbo].[tblParts] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblParts_dbo.tblMaterials_KFMaterialsID] FOREIGN KEY ([KFMaterialsID]) REFERENCES [dbo].[tblMaterials] ([KPMaterialsID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblParts_dbo.tblPartsCategory_KFPartCategory]...';


GO
ALTER TABLE [dbo].[tblParts] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblParts_dbo.tblPartsCategory_KFPartCategory] FOREIGN KEY ([KFPartCategory]) REFERENCES [dbo].[tblPartsCategory] ([KPCategoryID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblParts_dbo.tblAssemblies_KFAssemblyID]...';


GO
ALTER TABLE [dbo].[tblParts] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblParts_dbo.tblAssemblies_KFAssemblyID] FOREIGN KEY ([KFAssemblyID]) REFERENCES [dbo].[tblAssemblies] ([KPAssemblyID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblParts_dbo.tblTypes_SoldByOption_KFOptionID]...';


GO
ALTER TABLE [dbo].[tblParts] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblParts_dbo.tblTypes_SoldByOption_KFOptionID] FOREIGN KEY ([KFOptionID]) REFERENCES [dbo].[tblTypes_SoldByOption] ([ItemTypeID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPumps_dbo.tblWellLocation_KFWellLocationID]...';


GO
ALTER TABLE [dbo].[tblPumps] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPumps_dbo.tblWellLocation_KFWellLocationID] FOREIGN KEY ([KFWellLocationID]) REFERENCES [dbo].[tblWellLocation] ([KPWellLocationID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPumps_dbo.tblPumpTemplates_KFPumpTemplateID]...';


GO
ALTER TABLE [dbo].[tblPumps] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPumps_dbo.tblPumpTemplates_KFPumpTemplateID] FOREIGN KEY ([KFPumpTemplateID]) REFERENCES [dbo].[tblPumpTemplates] ([KPPumpTemplateID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID] FOREIGN KEY ([ReplacedWithInventoryPartID]) REFERENCES [dbo].[tblPartInstances] ([PartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID] FOREIGN KEY ([TemplatePartDefID]) REFERENCES [dbo].[tblTemplatePartsJoin] ([KPTemplatePartsJoinID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblRepairTickets_KFParentAssemblyID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblRepairTickets_KFParentAssemblyID] FOREIGN KEY ([KFParentAssemblyID]) REFERENCES [dbo].[tblRepairTickets] ([KPRepairID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblParts_KFPartFailedID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblParts_KFPartFailedID] FOREIGN KEY ([KFPartFailedID]) REFERENCES [dbo].[tblParts] ([KPPartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblParts_KFPartReplacedID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblParts_KFPartReplacedID] FOREIGN KEY ([KFPartReplacedID]) REFERENCES [dbo].[tblParts] ([KPPartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblRepairTickets_dbo.tblShopTickets_KFShopTicketID]...';


GO
ALTER TABLE [dbo].[tblRepairTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblShopTickets_KFShopTicketID] FOREIGN KEY ([KFShopTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblShopTickets_dbo.tblCountySalesTaxRates_CountySalesTaxRateID]...';


GO
ALTER TABLE [dbo].[tblShopTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblCountySalesTaxRates_CountySalesTaxRateID] FOREIGN KEY ([CountySalesTaxRateID]) REFERENCES [dbo].[tblCountySalesTaxRates] ([CountySalesTaxRateID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblShopTickets_dbo.tblCustomers_KFCustomerID]...';


GO
ALTER TABLE [dbo].[tblShopTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblCustomers_KFCustomerID] FOREIGN KEY ([KFCustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblShopTickets_dbo.tblPumps_PumpDispatchedID]...';


GO
ALTER TABLE [dbo].[tblShopTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblPumps_PumpDispatchedID] FOREIGN KEY ([PumpDispatchedID]) REFERENCES [dbo].[tblPumps] ([KPPumpID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblShopTickets_dbo.tblPumps_PumpFailedID]...';


GO
ALTER TABLE [dbo].[tblShopTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblPumps_PumpFailedID] FOREIGN KEY ([PumpFailedID]) REFERENCES [dbo].[tblPumps] ([KPPumpID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblShopTickets_dbo.tblWellLocation_KFWellLocationID]...';


GO
ALTER TABLE [dbo].[tblShopTickets] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblWellLocation_KFWellLocationID] FOREIGN KEY ([KFWellLocationID]) REFERENCES [dbo].[tblWellLocation] ([KPWellLocationID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblUserRoles_dbo.tblRoles_RoleID]...';


GO
ALTER TABLE [dbo].[tblUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblUserRoles_dbo.tblRoles_RoleID] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[tblRoles] ([RoleID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblUserRoles_dbo.tblUsers_UserID]...';


GO
ALTER TABLE [dbo].[tblUserRoles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblUserRoles_dbo.tblUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tblUsers] ([UserID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblAssemblies_dbo.tblPartsCategory_KFCategoryID]...';


GO
ALTER TABLE [dbo].[tblAssemblies] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblAssemblies_dbo.tblPartsCategory_KFCategoryID] FOREIGN KEY ([KFCategoryID]) REFERENCES [dbo].[tblPartsCategory] ([KPCategoryID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblDeliveryTicketImageUploads_dbo.tblShopTickets_DeliveryTicketID]...';


GO
ALTER TABLE [dbo].[tblDeliveryTicketImageUploads] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblDeliveryTicketImageUploads_dbo.tblShopTickets_DeliveryTicketID] FOREIGN KEY ([DeliveryTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimes_dbo.tblPumps_PumpID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblPumps_PumpID] FOREIGN KEY ([PumpID]) REFERENCES [dbo].[tblPumps] ([KPPumpID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimes_dbo.tblRepairTickets_RuntimeEndedByInspectionID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblRepairTickets_RuntimeEndedByInspectionID] FOREIGN KEY ([RuntimeEndedByInspectionID]) REFERENCES [dbo].[tblRepairTickets] ([KPRepairID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID] FOREIGN KEY ([RuntimeStartedByTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimes_dbo.tblTemplatePartsJoin_TemplatePartDefID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblTemplatePartsJoin_TemplatePartDefID] FOREIGN KEY ([TemplatePartDefID]) REFERENCES [dbo].[tblTemplatePartsJoin] ([KPTemplatePartsJoinID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimeSegments_dbo.tblPartRuntimes_RuntimeID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblPartRuntimes_RuntimeID] FOREIGN KEY ([RuntimeID]) REFERENCES [dbo].[tblPartRuntimes] ([PartRuntimeID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentEndedByTicketID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentEndedByTicketID] FOREIGN KEY ([SegmentEndedByTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentStartedByTicketID]...';


GO
ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentStartedByTicketID] FOREIGN KEY ([SegmentStartedByTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartsAssemblies_dbo.tblAssemblies_KFAssemblyID]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartsAssemblies_dbo.tblAssemblies_KFAssemblyID] FOREIGN KEY ([KFAssemblyID]) REFERENCES [dbo].[tblAssemblies] ([KPAssemblyID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPartsAssemblies_dbo.tblParts_KFPartsID]...';


GO
ALTER TABLE [dbo].[tblPartsAssemblies] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPartsAssemblies_dbo.tblParts_KFPartsID] FOREIGN KEY ([KFPartsID]) REFERENCES [dbo].[tblParts] ([KPPartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPumpRuntimes_dbo.tblPumps_PumpID]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblPumps_PumpID] FOREIGN KEY ([PumpID]) REFERENCES [dbo].[tblPumps] ([KPPumpID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeEndedByTicketID]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeEndedByTicketID] FOREIGN KEY ([RuntimeEndedByTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID]...';


GO
ALTER TABLE [dbo].[tblPumpRuntimes] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID] FOREIGN KEY ([RuntimeStartedByTicketID]) REFERENCES [dbo].[tblShopTickets] ([KPShopTicketID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblTemplatePartsJoin_dbo.tblParts_KFPartsID]...';


GO
ALTER TABLE [dbo].[tblTemplatePartsJoin] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblTemplatePartsJoin_dbo.tblParts_KFPartsID] FOREIGN KEY ([KFPartsID]) REFERENCES [dbo].[tblParts] ([KPPartID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblTemplatePartsJoin_dbo.tblPumpTemplates_KFPumpTemplateID]...';


GO
ALTER TABLE [dbo].[tblTemplatePartsJoin] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblTemplatePartsJoin_dbo.tblPumpTemplates_KFPumpTemplateID] FOREIGN KEY ([KFPumpTemplateID]) REFERENCES [dbo].[tblPumpTemplates] ([KPPumpTemplateID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblUsernameCustomerAccess_dbo.tblAcePumpProfiles_UserID]...';


GO
ALTER TABLE [dbo].[tblUsernameCustomerAccess] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblUsernameCustomerAccess_dbo.tblAcePumpProfiles_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tblAcePumpProfiles] ([UserID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblUsernameCustomerAccess_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblUsernameCustomerAccess] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblUsernameCustomerAccess_dbo.tblCustomers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_dbo.tblWellLocation_dbo.tblCustomers_KFCustomerID]...';


GO
ALTER TABLE [dbo].[tblWellLocation] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblWellLocation_dbo.tblCustomers_KFCustomerID] FOREIGN KEY ([KFCustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblWellLocation_dbo.tblLeaseLocations_KFLeaseID]...';


GO
ALTER TABLE [dbo].[tblWellLocation] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblWellLocation_dbo.tblLeaseLocations_KFLeaseID] FOREIGN KEY ([KFLeaseID]) REFERENCES [dbo].[tblLeaseLocations] ([KPLeaseID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblAcePumpProfiles_dbo.tblCustomers_CustomerID]...';


GO
ALTER TABLE [dbo].[tblAcePumpProfiles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblAcePumpProfiles_dbo.tblCustomers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[tblCustomers] ([KPCustomerID]);


GO
PRINT N'Creating [dbo].[FK_dbo.tblAcePumpProfiles_dbo.tblUsers_UserID]...';


GO
ALTER TABLE [dbo].[tblAcePumpProfiles] WITH NOCHECK
    ADD CONSTRAINT [FK_dbo.tblAcePumpProfiles_dbo.tblUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tblUsers] ([UserID]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[tblCustomers] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblCustomers_dbo.tblCountySalesTaxRates_CountySalesTaxRateID];

ALTER TABLE [dbo].[tblCustomerPartSpecials] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblCustomers_CustomerID];

ALTER TABLE [dbo].[tblPartInstances] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblCustomers_CustomerID];

ALTER TABLE [dbo].[tblCustomers] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblCustomers_dbo.tblQbInvoiceClasses_QbInvoiceClassID];

ALTER TABLE [dbo].[tblLineItems] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblLineItems_dbo.tblRepairTickets_KFRepairTicketID];

ALTER TABLE [dbo].[tblLineItems] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblLineItems_dbo.tblParts_KFPartID];

ALTER TABLE [dbo].[tblLineItems] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblLineItems_dbo.tblShopTickets_KFShopTicketID];

ALTER TABLE [dbo].[tblCustomerPartSpecials] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblCustomerPartSpecials_dbo.tblParts_PartID];

ALTER TABLE [dbo].[tblPartInstances] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartInstances_dbo.tblParts_PartTemplateID];

ALTER TABLE [dbo].[tblParts] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblParts_dbo.tblMaterials_KFMaterialsID];

ALTER TABLE [dbo].[tblParts] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblParts_dbo.tblPartsCategory_KFPartCategory];

ALTER TABLE [dbo].[tblParts] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblParts_dbo.tblAssemblies_KFAssemblyID];

ALTER TABLE [dbo].[tblParts] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblParts_dbo.tblTypes_SoldByOption_KFOptionID];

ALTER TABLE [dbo].[tblPumps] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPumps_dbo.tblWellLocation_KFWellLocationID];

ALTER TABLE [dbo].[tblPumps] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPumps_dbo.tblPumpTemplates_KFPumpTemplateID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblPartInstances_ReplacedWithInventoryPartID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblTemplatePartsJoin_TemplatePartDefID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblRepairTickets_KFParentAssemblyID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblParts_KFPartFailedID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblParts_KFPartReplacedID];

ALTER TABLE [dbo].[tblRepairTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblRepairTickets_dbo.tblShopTickets_KFShopTicketID];

ALTER TABLE [dbo].[tblShopTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblCountySalesTaxRates_CountySalesTaxRateID];

ALTER TABLE [dbo].[tblShopTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblCustomers_KFCustomerID];

ALTER TABLE [dbo].[tblShopTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblPumps_PumpDispatchedID];

ALTER TABLE [dbo].[tblShopTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblPumps_PumpFailedID];

ALTER TABLE [dbo].[tblShopTickets] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblShopTickets_dbo.tblWellLocation_KFWellLocationID];

ALTER TABLE [dbo].[tblUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblUserRoles_dbo.tblRoles_RoleID];

ALTER TABLE [dbo].[tblUserRoles] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblUserRoles_dbo.tblUsers_UserID];

ALTER TABLE [dbo].[tblAssemblies] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblAssemblies_dbo.tblPartsCategory_KFCategoryID];

ALTER TABLE [dbo].[tblDeliveryTicketImageUploads] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblDeliveryTicketImageUploads_dbo.tblShopTickets_DeliveryTicketID];

ALTER TABLE [dbo].[tblPartRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblPumps_PumpID];

ALTER TABLE [dbo].[tblPartRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblRepairTickets_RuntimeEndedByInspectionID];

ALTER TABLE [dbo].[tblPartRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID];

ALTER TABLE [dbo].[tblPartRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimes_dbo.tblTemplatePartsJoin_TemplatePartDefID];

ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblPartRuntimes_RuntimeID];

ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentEndedByTicketID];

ALTER TABLE [dbo].[tblPartRuntimeSegments] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartRuntimeSegments_dbo.tblShopTickets_SegmentStartedByTicketID];

ALTER TABLE [dbo].[tblPartsAssemblies] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartsAssemblies_dbo.tblAssemblies_KFAssemblyID];

ALTER TABLE [dbo].[tblPartsAssemblies] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPartsAssemblies_dbo.tblParts_KFPartsID];

ALTER TABLE [dbo].[tblPumpRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblPumps_PumpID];

ALTER TABLE [dbo].[tblPumpRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeEndedByTicketID];

ALTER TABLE [dbo].[tblPumpRuntimes] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblPumpRuntimes_dbo.tblShopTickets_RuntimeStartedByTicketID];

ALTER TABLE [dbo].[tblTemplatePartsJoin] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblTemplatePartsJoin_dbo.tblParts_KFPartsID];

ALTER TABLE [dbo].[tblTemplatePartsJoin] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblTemplatePartsJoin_dbo.tblPumpTemplates_KFPumpTemplateID];

ALTER TABLE [dbo].[tblUsernameCustomerAccess] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblUsernameCustomerAccess_dbo.tblAcePumpProfiles_UserID];

ALTER TABLE [dbo].[tblUsernameCustomerAccess] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblUsernameCustomerAccess_dbo.tblCustomers_CustomerID];

ALTER TABLE [dbo].[tblWellLocation] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblWellLocation_dbo.tblCustomers_KFCustomerID];

ALTER TABLE [dbo].[tblWellLocation] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblWellLocation_dbo.tblLeaseLocations_KFLeaseID];

ALTER TABLE [dbo].[tblAcePumpProfiles] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblAcePumpProfiles_dbo.tblCustomers_CustomerID];

ALTER TABLE [dbo].[tblAcePumpProfiles] WITH CHECK CHECK CONSTRAINT [FK_dbo.tblAcePumpProfiles_dbo.tblUsers_UserID];


GO
PRINT N'Update complete.';


GO
COMMIT TRANSACTION