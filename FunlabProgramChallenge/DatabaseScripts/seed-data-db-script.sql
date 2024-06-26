CREATE DATABASE [FunlabProgramChallenge];
GO

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

USE [FunlabProgramChallenge]
GO

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](256) NULL,
	[LastName] [nvarchar](256) NULL,
	[EmailAddress] [nvarchar](256) NULL,
	[PhoneNumber] [nvarchar](256) NULL,
	[PresentAddress] [nvarchar](500) NULL,
	[PermanentAddress] [nvarchar](500) NULL,
	[CardName] [nvarchar](256) NULL,
	[CardNumber] [nvarchar](256) NULL,
	[CardExpirationYear] [nvarchar](128) NULL,
	[CardExpirationMonth] [nvarchar](128) NULL,
	[CardCvc] [nvarchar](128) NULL,
	[CardCountry] [nvarchar](128) NULL,
	
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


--Role
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES (N'2ac48615-4620-476b-899f-d99aa3138f4b', N'Admin', N'ADMIN', N'2a5e2652-6afe-4bcc-ad00-3e41bb381db3')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES (N'1e500ab8-4bd1-4dee-a7e4-d4f7c7a45b28', N'Member', N'MEMBER', N'37f3a020-7e95-4874-b08f-6c0cd5928663')
GO
--Role

--User --Qwer!234 --AQAAAAIAAYagAAAAEOp4Elah0dAU4I4mJnSzl3mMH3Fz/445us2FZYt3cV/IoJbaUJuFqJTETPuCxbSSyQ==
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'c79fa52e-a3db-4bde-be90-7d5dd35836de', 0, N'admin@mail.com', N'ADMIN@MAIL.COM', N'admin@mail.com', N'ADMIN@MAIL.COM', 0, N'AQAAAAIAAYagAAAAEOp4Elah0dAU4I4mJnSzl3mMH3Fz/445us2FZYt3cV/IoJbaUJuFqJTETPuCxbSSyQ==', N'ea19af2a-d7e7-42a0-8b56-7adf13cc4374', N'ef44ca0b-de20-4da1-9146-0f3fdf9c5a63', 0, 0, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'28909eda-0b7e-44a6-a565-9f38a6641923', 0, N'member@mail.com', N'MEMBER@MAIL.COM', N'member@mail.com', N'MEMBER@MAIL.COM', 0, N'AQAAAAIAAYagAAAAEOp4Elah0dAU4I4mJnSzl3mMH3Fz/445us2FZYt3cV/IoJbaUJuFqJTETPuCxbSSyQ==', N'3de7b7c2-08e0-412e-80ac-56f6de01f459', N'3d24e5b9-1924-4747-9365-946fa617d567', 0, 0, 0)
GO
--User --Qwer!234 --AQAAAAIAAYagAAAAEOp4Elah0dAU4I4mJnSzl3mMH3Fz/445us2FZYt3cV/IoJbaUJuFqJTETPuCxbSSyQ==

--User Role

INSERT [dbo].[AspNetUserRoles] ([RoleId], [UserId]) VALUES (N'2ac48615-4620-476b-899f-d99aa3138f4b', N'c79fa52e-a3db-4bde-be90-7d5dd35836de')
GO
INSERT [dbo].[AspNetUserRoles] ([RoleId], [UserId]) VALUES (N'2ac48615-4620-476b-899f-d99aa3138f4b', N'28909eda-0b7e-44a6-a565-9f38a6641923')
GO

INSERT [dbo].[AspNetUserRoles] ([RoleId], [UserId]) VALUES (N'1e500ab8-4bd1-4dee-a7e4-d4f7c7a45b28', N'28909eda-0b7e-44a6-a565-9f38a6641923')
GO
--User Role

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[Member] ON 
GO
INSERT [dbo].[Member] ([MemberId], [UserId], [FirstName], [LastName], [EmailAddress], [PhoneNumber], [PresentAddress], [PermanentAddress], [CardName], [CardNumber], [CardExpirationYear], [CardExpirationMonth], [CardCvc], [CardCountry]) 
VALUES (1, N'28909eda-0b7e-44a6-a565-9f38a6641923', N'Rasel', N'Ahmmed', N'raselofficial102010@outlook.com', N'01911-045573', N'Dhaka', N'Dhaka', N'Rasel Ahmmed', N'4242 4242 4242 4242', N'34', N'12', N'123', N'USD')
GO
SET IDENTITY_INSERT [dbo].[Member] OFF

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------