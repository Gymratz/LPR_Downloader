USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_ImportHistory]    Script Date: 10/18/2020 1:23:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_ImportHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Import_Time] [datetime] NULL,
	[Count_Imported] [int] NULL,
	[Count_Skipped] [int] NULL,
 CONSTRAINT [PK_ImportHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


