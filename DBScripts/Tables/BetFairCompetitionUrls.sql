USE [bARBie]
GO

ALTER TABLE [dbo].[BetFairCompetitionUrls] DROP CONSTRAINT [FK_BetFairCompetitionUrls_CountryID]
GO

ALTER TABLE [dbo].[BetFairCompetitionUrls] DROP CONSTRAINT [FK_BetFairCompetitionUrls_CompetitionID]
GO

/****** Object:  Table [dbo].[BetFairCompetitionUrls]    Script Date: 20/03/2014 00:23:35 ******/
DROP TABLE [dbo].[BetFairCompetitionUrls]
GO

/****** Object:  Table [dbo].[BetFairCompetitionUrls]    Script Date: 20/03/2014 00:23:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BetFairCompetitionUrls](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NULL,
	[CompetitionID] [int] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_BetFairCompetitionUrls] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[BetFairCompetitionUrls]  WITH CHECK ADD  CONSTRAINT [FK_BetFairCompetitionUrls_CompetitionID] FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[FootballCompetitions] ([ID])
GO

ALTER TABLE [dbo].[BetFairCompetitionUrls] CHECK CONSTRAINT [FK_BetFairCompetitionUrls_CompetitionID]
GO

ALTER TABLE [dbo].[BetFairCompetitionUrls]  WITH CHECK ADD  CONSTRAINT [FK_BetFairCompetitionUrls_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([ID])
GO

ALTER TABLE [dbo].[BetFairCompetitionUrls] CHECK CONSTRAINT [FK_BetFairCompetitionUrls_CountryID]
GO

