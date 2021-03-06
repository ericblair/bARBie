USE [bARBie]
GO

/****** Object:  Table [dbo].[FootballCompetitions]    Script Date: 20/03/2014 00:24:59 ******/
DROP TABLE [dbo].[FootballCompetitions]
GO

/****** Object:  Table [dbo].[FootballCompetitions]    Script Date: 20/03/2014 00:24:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FootballCompetitions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NULL,
	[Competition] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_FootballCompetitions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

