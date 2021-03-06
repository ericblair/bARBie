USE [bARBie]
GO

ALTER TABLE [dbo].[BetFairFootballOdds] DROP CONSTRAINT [FK_BetFairFootballOdds_FixtureID]
GO

ALTER TABLE [dbo].[BetFairFootballOdds] DROP CONSTRAINT [FK_BetFairFootballOdds_CountryID]
GO

ALTER TABLE [dbo].[BetFairFootballOdds] DROP CONSTRAINT [FK_BetFairFootballOdds_CompetitionID]
GO

/****** Object:  Table [dbo].[BetFairFootballOdds]    Script Date: 20/03/2014 00:24:10 ******/
DROP TABLE [dbo].[BetFairFootballOdds]
GO

/****** Object:  Table [dbo].[BetFairFootballOdds]    Script Date: 20/03/2014 00:24:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetFairFootballOdds](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FixtureID] [int] NOT NULL,
	[CountryID] [int] NULL,
	[CompetitionID] [int] NOT NULL,
	[Prediction] [nvarchar](50) NOT NULL,
	[BackLow] [decimal](8, 2) NULL,
	[BackLowCash] [decimal](8, 2) NULL,
	[BackMid] [decimal](8, 2) NULL,
	[BackMidCash] [decimal](8, 2) NULL,
	[BackHigh] [decimal](8, 2) NULL,
	[BackHighCash] [decimal](8, 2) NULL,
	[LayLow] [decimal](8, 2) NULL,
	[LayLowCash] [decimal](8, 2) NULL,
	[LayMid] [decimal](8, 2) NULL,
	[LayMidCash] [decimal](8, 2) NULL,
	[LayHigh] [decimal](8, 2) NULL,
	[LayHighCash] [decimal](8, 2) NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_BetFairFootballOdds] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetFairFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_BetFairFootballOdds_CompetitionID] FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[FootballCompetitions] ([ID])
GO

ALTER TABLE [dbo].[BetFairFootballOdds] CHECK CONSTRAINT [FK_BetFairFootballOdds_CompetitionID]
GO

ALTER TABLE [dbo].[BetFairFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_BetFairFootballOdds_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([ID])
GO

ALTER TABLE [dbo].[BetFairFootballOdds] CHECK CONSTRAINT [FK_BetFairFootballOdds_CountryID]
GO

ALTER TABLE [dbo].[BetFairFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_BetFairFootballOdds_FixtureID] FOREIGN KEY([FixtureID])
REFERENCES [dbo].[BetFairFootballFixtures] ([ID])
GO

ALTER TABLE [dbo].[BetFairFootballOdds] CHECK CONSTRAINT [FK_BetFairFootballOdds_FixtureID]
GO

