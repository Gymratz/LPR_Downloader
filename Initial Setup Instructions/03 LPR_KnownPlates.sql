USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_KnownPlates]    Script Date: 8/9/2019 9:08:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_KnownPlates](
	[Plate] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[Status] [nvarchar](100) NULL,
	[Alert_Address] [nvarchar](250) NULL,
 CONSTRAINT [PK_KnownPlates] PRIMARY KEY CLUSTERED 
(
	[Plate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

