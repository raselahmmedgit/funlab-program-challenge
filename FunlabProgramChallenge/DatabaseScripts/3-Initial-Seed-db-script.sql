USE [FunlabProgramChallenge]
GO
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


--Role
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES (N'2ac48615-4620-476b-899f-d99aa3138f4b', N'Admin', N'ADMIN', N'2a5e2652-6afe-4bcc-ad00-3e41bb381db3')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
VALUES (N'1e500ab8-4bd1-4dee-a7e4-d4f7c7a45b28', N'Member', N'USER', N'37f3a020-7e95-4874-b08f-6c0cd5928663')
GO
--Role

--User -- Qwer!234
INSERT [dbo].[AspNetUsers] ([Id], [AppUserId], [AccessFailedCount], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'c79fa52e-a3db-4bde-be90-7d5dd35836de', 2, 0, N'admin@mail.com', N'ADMIN@MAIL.COM', N'admin@mail.com', N'ADMIN@MAIL.COM', 0, N'$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m', N'ea19af2a-d7e7-42a0-8b56-7adf13cc4374', N'ef44ca0b-de20-4da1-9146-0f3fdf9c5a63', 0, 0, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [AppUserId], [AccessFailedCount], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled])
VALUES (N'28909eda-0b7e-44a6-a565-9f38a6641923', 3, 0, N'member@mail.com', N'MEMBER@MAIL.COM', N'member@mail.com', N'MEMBER@MAIL.COM', 0, N'$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m', N'3de7b7c2-08e0-412e-80ac-56f6de01f459', N'3d24e5b9-1924-4747-9365-946fa617d567', 0, 0, 0)
GO
--User -- Qwer!234

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
VALUES (1, N'28909eda-0b7e-44a6-a565-9f38a6641923', N'Rasel', N'Ahmmed', N'raselahmmed@mail.com', N'01911-045573', N'Dhaka', N'Dhaka', N'Rasel Ahmmed', N'007', N'26', N'12', N'1234', N'Bangladesh')
GO
SET IDENTITY_INSERT [dbo].[Member] OFF

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------