SET IDENTITY_INSERT [dbo].[tblRoles] ON 
GO

INSERT [dbo].[tblRoles] ([RoleID], [RoleName], [DisplayText]) SELECT 11, N'ApiQuickbooksUser', N'Connect Sync For QB with PTP' 
WHERE NOT EXISTS(SELECT 1 FROM [dbo].[tblRoles] WHERE ROLEID = 11)
GO

SET IDENTITY_INSERT [dbo].[tblRoles] OFF
GO
