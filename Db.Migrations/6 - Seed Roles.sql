SET IDENTITY_INSERT [dbo].[tblRoles] ON 
GO

INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 1, N'_Netsmith_AccountManager', N'Account Manager'
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 1)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 2, N'_Netsmith_Impersonator', N'Impersonator' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 2)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 3, N'ReportViewer', N'Report Viewer' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 3)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 4, N'AcePump', N'Ace Pump Employee' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 4)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 5, N'Customer', N'Ace Pump Customer' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 5)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 6, N'TypeManager', N'Manage Drop Down Lists' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 6)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 7, N'SeeCost', N'See and edit cost and markup' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 7)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 8, N'SysAdmin', N'Programmer or Tech.'
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 8)
GO
INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText])  SELECT 10, N'DeliveryTicketSigner', N'Sign delivery tickets (Customer)'
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 10)
GO

SET IDENTITY_INSERT [dbo].[tblRoles] OFF
GO
