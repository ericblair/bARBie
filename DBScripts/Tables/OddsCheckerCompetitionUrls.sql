USE [bARBie]
GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls] DROP CONSTRAINT [FK_OddsCheckerCompetitionUrls_CountryID]
GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls] DROP CONSTRAINT [FK_OddsCheckerCompetitionUrls_CompetitionID]
GO

/****** Object:  Table [dbo].[OddsCheckerCompetitionUrls]    Script Date: 20/03/2014 00:25:26 ******/
DROP TABLE [dbo].[OddsCheckerCompetitionUrls]
GO

/****** Object:  Table [dbo].[OddsCheckerCompetitionUrls]    Script Date: 20/03/2014 00:25:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OddsCheckerCompetitionUrls](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NULL,
	[CompetitionID] [int] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OddsCheckerCompetitionUrls] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerCompetitionUrls_CompetitionID] FOREIGN KEY([CompetitionID])
REFERENCES [dbo].[FootballCompetitions] ([ID])
GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls] CHECK CONSTRAINT [FK_OddsCheckerCompetitionUrls_CompetitionID]
GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls]  WITH CHECK ADD  CONSTRAINT [FK_OddsCheckerCompetitionUrls_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([ID])
GO

ALTER TABLE [dbo].[OddsCheckerCompetitionUrls] CHECK CONSTRAINT [FK_OddsCheckerCompetitionUrls_CountryID]
GO

