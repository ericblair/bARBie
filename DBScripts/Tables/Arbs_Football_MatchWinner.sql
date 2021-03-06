USE [bARBie]
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] DROP CONSTRAINT [FK_Arbs_Football_MatchWinner_FixtureMapID]
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] DROP CONSTRAINT [DF__Arbs_Foot__Paren__5DCAEF64]
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] DROP CONSTRAINT [DF__Arbs_Foot__Expir__5CD6CB2B]
GO

/****** Object:  Table [dbo].[Arbs_Football_MatchWinner]    Script Date: 20/03/2014 00:23:04 ******/
DROP TABLE [dbo].[Arbs_Football_MatchWinner]
GO

/****** Object:  Table [dbo].[Arbs_Football_MatchWinner]    Script Date: 20/03/2014 00:23:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Arbs_Football_MatchWinner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FixtureMapID] [int] NOT NULL,
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
	[Expired] [bit] NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_Arbs_Football_MatchWinner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] ADD  DEFAULT ((0)) FOR [Expired]
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] ADD  DEFAULT (NULL) FOR [ParentID]
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner]  WITH CHECK ADD  CONSTRAINT [FK_Arbs_Football_MatchWinner_FixtureMapID] FOREIGN KEY([FixtureMapID])
REFERENCES [dbo].[FootballFixturesMap] ([ID])
GO

ALTER TABLE [dbo].[Arbs_Football_MatchWinner] CHECK CONSTRAINT [FK_Arbs_Football_MatchWinner_FixtureMapID]
GO