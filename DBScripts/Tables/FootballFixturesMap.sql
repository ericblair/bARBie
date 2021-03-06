USE [bARBie]
GO

ALTER TABLE [dbo].[FootballFixturesMap] DROP CONSTRAINT [FK_FootballFixturesMap_OddsCheckerFixtureID]
GO

ALTER TABLE [dbo].[FootballFixturesMap] DROP CONSTRAINT [FK_FootballFixturesMap_BetFairFixtureID]
GO

/****** Object:  Table [dbo].[FootballFixturesMap]    Script Date: 20/03/2014 00:25:13 ******/
DROP TABLE [dbo].[FootballFixturesMap]
GO

/****** Object:  Table [dbo].[FootballFixturesMap]    Script Date: 20/03/2014 00:25:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FootballFixturesMap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BetFairFixtureID] [int] NOT NULL,
	[OddsCheckerFixtureID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[FootballFixturesMap]  WITH CHECK ADD  CONSTRAINT [FK_FootballFixturesMap_BetFairFixtureID] FOREIGN KEY([BetFairFixtureID])
REFERENCES [dbo].[BetFairFootballFixtures] ([ID])
GO

ALTER TABLE [dbo].[FootballFixturesMap] CHECK CONSTRAINT [FK_FootballFixturesMap_BetFairFixtureID]
GO

ALTER TABLE [dbo].[FootballFixturesMap]  WITH CHECK ADD  CONSTRAINT [FK_FootballFixturesMap_OddsCheckerFixtureID] FOREIGN KEY([OddsCheckerFixtureID])
REFERENCES [dbo].[OddsCheckerFootballFixtures] ([ID])
GO

ALTER TABLE [dbo].[FootballFixturesMap] CHECK CONSTRAINT [FK_FootballFixturesMap_OddsCheckerFixtureID]
GO

