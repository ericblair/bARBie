USE [bARBie]
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] DROP CONSTRAINT [FK_OddsCheckerFootballOdds_FixtureID]
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] DROP CONSTRAINT [FK_OddsCheckerFootballOdds_CountryID]
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] DROP CONSTRAINT [FK_OddsCheckerFootballOdds_CompetitionID]
GO

/****** Object:  Table [dbo].[OddsCheckerFootballOdds]    Script Date: 20/03/2014 00:25:51 ******/
DROP TABLE [dbo].[OddsCheckerFootballOdds]
GO

/****** Object:  Table [dbo].[OddsCheckerFootballOdds]    Script Date: 20/03/2014 00:25:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OddsCheckerFootballOdds](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FixtureID] [int] NOT NULL,
	[CountryID] [int] NULL,
	[CompetitionID] [int] NOT NULL,
	[Prediction] [nvarchar](50) NOT NULL,
	[Bet365] [decimal](8, 2) NULL,
	[SkyBet] [decimal](8, 2) NULL,
	[ToteSport] [decimal](8, 2) NULL,
	[BoyleSports] [decimal](8, 2) NULL,
	[BetFred] [decimal](8, 2) NULL,
	[SportingBet] [decimal](8, 2) NULL,
	[BetVictor] [decimal](8, 2) NULL,
	[PaddyPower] [decimal](8, 2) NULL,
	[StanJames] [decimal](8, 2) NULL,
	[888Sport] [decimal](8, 2) NULL,
	[Ladbrokes] [decimal](8, 2) NULL,
	[Coral] [decimal](8, 2) NULL,
	[WilliamHill] [decimal](8, 2) NULL,
	[Winner] [decimal](8, 2) NULL,
	[SpreadEx] [decimal](8, 2) NULL,
	[BetWay] [decimal](8, 2) NULL,
	[Bwin] [decimal](8, 2) NULL,
	[UniBet] [decimal](8, 2) NULL,
	[YouWin] [decimal](8, 2) NULL,
	[32RedBet] [decimal](8, 2) NULL,
	[BetFair] [decimal](8, 2) NULL,
	[BetDaq] [decimal](8, 2) NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_OddsCheckerFootballOdds] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerFootballOdds_CompetitionID] FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[FootballCompetitions] ([ID])
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] CHECK CONSTRAINT [FK_OddsCheckerFootballOdds_CompetitionID]
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerFootballOdds_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([ID])
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] CHECK CONSTRAINT [FK_OddsCheckerFootballOdds_CountryID]
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerFootballOdds_FixtureID] FOREIGN KEY([FixtureID])
REFERENCES [dbo].[OddsCheckerFootballFixtures] ([ID])
GO

ALTER TABLE [dbo].[OddsCheckerFootballOdds] CHECK CONSTRAINT [FK_OddsCheckerFootballOdds_FixtureID]
GO

