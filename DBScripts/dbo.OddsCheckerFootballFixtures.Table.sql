USE [bARBie]
GO
/****** Object:  Table [dbo].[OddsCheckerFootballFixtures]    Script Date: 12/03/2014 14:11:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OddsCheckerFootballFixtures](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NULL,
	[CompetitionID] [int] NOT NULL,
	[MatchDateTime] [datetime] NOT NULL,
	[HomeTeam] [nvarchar](100) NOT NULL,
	[AwayTeam] [nvarchar](100) NOT NULL,
	[MatchWinnerOddsUrl] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OddsCheckerFootballFixtures] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[OddsCheckerFootballFixtures]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerFootballFixtures_CompetitionID] FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[FootballCompetitions] ([ID])
GO
ALTER TABLE [dbo].[OddsCheckerFootballFixtures] CHECK CONSTRAINT [FK_OddsCheckerFootballFixtures_CompetitionID]
GO
ALTER TABLE [dbo].[OddsCheckerFootballFixtures]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerFootballFixtures_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[OddsCheckerFootballFixtures] CHECK CONSTRAINT [FK_OddsCheckerFootballFixtures_CountryID]
GO
