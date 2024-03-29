USE [DB_CMS]
GO
/****** Object:  Table [dbo].[T_Action]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Action](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[controllername] [varchar](100) NOT NULL,
	[actionname] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[T_Action] ON
INSERT [dbo].[T_Action] ([id], [controllername], [actionname]) VALUES (100000, N'Home', N'Index')
SET IDENTITY_INSERT [dbo].[T_Action] OFF
/****** Object:  Table [dbo].[T_Role]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Role](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[T_Role] ON
INSERT [dbo].[T_Role] ([id], [name]) VALUES (100000, N'管理员')
SET IDENTITY_INSERT [dbo].[T_Role] OFF
/****** Object:  Table [dbo].[T_Links]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Links](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[name] [varchar](120) NOT NULL,
	[url] [varchar](120) NOT NULL,
	[createtime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Users]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Users](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](32) NOT NULL,
	[salt] [varchar](32) NOT NULL,
	[createtime] [datetime] NOT NULL,
	[lastlogintime] [datetime] NULL,
	[lastloginip] [varchar](32) NULL,
	[lastloginarea] [varchar](100) NULL,
	[type] [int] NOT NULL,
 CONSTRAINT [PK__T_Users__3213E83F7F60ED59] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UQ__T_Users__F3DBC572023D5A04] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[T_Users] ON
INSERT [dbo].[T_Users] ([id], [username], [password], [salt], [createtime], [lastlogintime], [lastloginip], [lastloginarea], [type]) VALUES (100046, N'gaoning', N'E3CBE33818773A93F59B9392735FD1CB', N'Mw7pTE', CAST(0x0000A48300000000 AS DateTime), CAST(0x0000A4840110F74D AS DateTime), N'127.0.0.1', N'本地', 0)
INSERT [dbo].[T_Users] ([id], [username], [password], [salt], [createtime], [lastlogintime], [lastloginip], [lastloginarea], [type]) VALUES (100048, N'laohuan', N'84F2F8B83488BD3CACE28EF3FE8948F', N'k75upk', CAST(0x0000A484010DE9FF AS DateTime), CAST(0x0000A48401110F16 AS DateTime), N'127.0.0.1', N'本地', 1)
SET IDENTITY_INSERT [dbo].[T_Users] OFF
/****** Object:  Table [dbo].[T_User_Role]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_User_Role](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[uid] [int] NULL,
	[rid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[T_User_Role] ON
INSERT [dbo].[T_User_Role] ([id], [uid], [rid]) VALUES (100000, 100046, 100000)
SET IDENTITY_INSERT [dbo].[T_User_Role] OFF
/****** Object:  Table [dbo].[T_Categories]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Categories](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[name] [varchar](120) NOT NULL,
	[parentid] [int] NOT NULL,
	[uid] [int] NULL,
	[createtime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Articles]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_Articles](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[title] [varchar](300) NOT NULL,
	[body] [text] NOT NULL,
	[abstract] [varchar](300) NULL,
	[from] [varchar](300) NULL,
	[author] [varchar](300) NULL,
	[cateid] [int] NULL,
	[uid] [int] NULL,
	[createtime] [datetime] NOT NULL,
 CONSTRAINT [PK__T_Articl__3213E83F07020F21] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_Action_Role]    Script Date: 04/24/2015 16:35:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Action_Role](
	[id] [int] IDENTITY(100000,1) NOT NULL,
	[aid] [int] NULL,
	[rid] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[T_Action_Role] ON
INSERT [dbo].[T_Action_Role] ([id], [aid], [rid]) VALUES (100000, 100000, 100000)
SET IDENTITY_INSERT [dbo].[T_Action_Role] OFF
/****** Object:  Default [DF__T_Categor__paren__0CBAE877]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Categories] ADD  DEFAULT ((0)) FOR [parentid]
GO
/****** Object:  Default [DF__T_Links__url__117F9D94]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Links] ADD  DEFAULT ('#') FOR [url]
GO
/****** Object:  Default [DF__T_Users__lastlog__0425A276]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Users] ADD  CONSTRAINT [DF__T_Users__lastlog__0425A276]  DEFAULT ('2000-1-1') FOR [lastlogintime]
GO
/****** Object:  ForeignKey [FK_T_Action_Role_T_Action]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Action_Role]  WITH CHECK ADD  CONSTRAINT [FK_T_Action_Role_T_Action] FOREIGN KEY([aid])
REFERENCES [dbo].[T_Action] ([id])
GO
ALTER TABLE [dbo].[T_Action_Role] CHECK CONSTRAINT [FK_T_Action_Role_T_Action]
GO
/****** Object:  ForeignKey [FK_T_Action_Role_T_Role]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Action_Role]  WITH CHECK ADD  CONSTRAINT [FK_T_Action_Role_T_Role] FOREIGN KEY([rid])
REFERENCES [dbo].[T_Role] ([id])
GO
ALTER TABLE [dbo].[T_Action_Role] CHECK CONSTRAINT [FK_T_Action_Role_T_Role]
GO
/****** Object:  ForeignKey [FK_T_Articles_T_Users]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Articles]  WITH CHECK ADD  CONSTRAINT [FK_T_Articles_T_Users] FOREIGN KEY([uid])
REFERENCES [dbo].[T_Users] ([id])
GO
ALTER TABLE [dbo].[T_Articles] CHECK CONSTRAINT [FK_T_Articles_T_Users]
GO
/****** Object:  ForeignKey [FK_T_Categories_T_Users]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_Categories]  WITH CHECK ADD  CONSTRAINT [FK_T_Categories_T_Users] FOREIGN KEY([uid])
REFERENCES [dbo].[T_Users] ([id])
GO
ALTER TABLE [dbo].[T_Categories] CHECK CONSTRAINT [FK_T_Categories_T_Users]
GO
/****** Object:  ForeignKey [FK_T_User_Role_T_Role]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_T_User_Role_T_Role] FOREIGN KEY([rid])
REFERENCES [dbo].[T_Role] ([id])
GO
ALTER TABLE [dbo].[T_User_Role] CHECK CONSTRAINT [FK_T_User_Role_T_Role]
GO
/****** Object:  ForeignKey [FK_T_User_Role_T_Users]    Script Date: 04/24/2015 16:35:52 ******/
ALTER TABLE [dbo].[T_User_Role]  WITH CHECK ADD  CONSTRAINT [FK_T_User_Role_T_Users] FOREIGN KEY([uid])
REFERENCES [dbo].[T_Users] ([id])
GO
ALTER TABLE [dbo].[T_User_Role] CHECK CONSTRAINT [FK_T_User_Role_T_Users]
GO
