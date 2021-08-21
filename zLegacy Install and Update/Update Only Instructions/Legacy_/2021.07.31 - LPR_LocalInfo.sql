USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_LocalInfo]    Script Date: 7/31/2021 4:11:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_LocalInfo](
	[UUID] [nvarchar](100) NULL,
	[best_color] [nvarchar](50) NULL,
	[best_make] [nvarchar](50) NULL,
	[best_model] [nvarchar](50) NULL
) ON [PRIMARY]
GO


