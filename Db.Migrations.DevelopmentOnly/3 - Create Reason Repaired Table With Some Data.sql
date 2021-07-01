USE [AcePump_Debug]
GO
/****** Object:  Table [dbo].[Types_ReasonRepaired]    Script Date: 5/24/2017 9:40:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Types_ReasonRepaired](
	[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[DisplayText] [nvarchar](max) NULL,
 CONSTRAINT [PK_Types_ReasonRepaired] PRIMARY KEY CLUSTERED 
(
	[ItemTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Types_ReasonRepaired] ON 

GO
INSERT [dbo].[Types_ReasonRepaired] ([ItemTypeID], [DisplayText]) VALUES (1, N'TORN')
GO
INSERT [dbo].[Types_ReasonRepaired] ([ItemTypeID], [DisplayText]) VALUES (2, N'PACKED')
GO
INSERT [dbo].[Types_ReasonRepaired] ([ItemTypeID], [DisplayText]) VALUES (3, N'BROKEN')
GO
INSERT [dbo].[Types_ReasonRepaired] ([ItemTypeID], [DisplayText]) VALUES (4, N'SCORED')
GO
INSERT [dbo].[Types_ReasonRepaired] ([ItemTypeID], [DisplayText]) VALUES (5, N'WORN')
GO
SET IDENTITY_INSERT [dbo].[Types_ReasonRepaired] OFF
GO
