USE [bARBie]
GO
/****** Object:  Table [dbo].[Arbs_Football_MatchWinner]    Script Date: 12/03/2014 14:11:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Arbs_Football_MatchWinner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MatchDateTime] [datetime] NOT NULL,
	[HomeTeam] [nvarchar](100) NOT NULL,
	[AwayTeam] [nvarchar](100) NOT NULL,
	[BetFairOdds] [decimal](8, 2) NOT NULL,
	[BookieOdds] [decimal](8, 2) NOT NULL,
	[BetFairCash] [decimal](8, 2) NULL,
	[Bookie] [nvarchar](100) NOT NULL,
	[Predication] [nvarchar](50) NOT NULL,
	[BetFairUpdated] [datetime] NOT NULL,
	[OddsCheckerUpdated] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
 CONSTRAINT [PK_Arbs_Football_MatchWinner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
