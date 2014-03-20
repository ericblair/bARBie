USE [bARBie]
GO

/****** Object:  Table [dbo].[Arbs_Football_MatchWinner_Expired]    Script Date: 20/03/2014 19:39:24 ******/
DROP TABLE [dbo].[Arbs_Football_MatchWinner_Expired]
GO

/****** Object:  Table [dbo].[Arbs_Football_MatchWinner_Expired]    Script Date: 20/03/2014 19:39:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Arbs_Football_MatchWinner_Expired](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OriginalID] [int] NOT NULL,
	[MatchDateTime] [datetime] NOT NULL,
	[HomeTeam] [nvarchar](100) NOT NULL,
	[AwayTeam] [nvarchar](100) NOT NULL,
	[Bookie] [nvarchar](100) NOT NULL,
	[BookieOdds] [decimal](8, 2) NOT NULL,
	[BetFairLayLevel] [nvarchar](5) NOT NULL,
	[BetFairOdds] [decimal](8, 2) NOT NULL,
	[BetFairCash] [decimal](8, 2) NULL,
	[Predication] [nvarchar](50) NOT NULL,
	[BetFairUpdated] [datetime] NOT NULL,
	[OddsCheckerUpdated] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_Arbs_Football_MatchWinner_Expired] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

