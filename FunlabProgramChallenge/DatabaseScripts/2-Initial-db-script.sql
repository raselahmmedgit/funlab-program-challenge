USE [FunlabProgramChallenge]
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
