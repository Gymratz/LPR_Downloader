USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_PlateHits_ToHide]    Script Date: 10/18/2020 1:26:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_PlateHits_ToHide](
	[pk] [int] NOT NULL,
	[reason] [nvarchar](50) NULL,
	[date_added] [datetime] NULL,
 CONSTRAINT [PK_PlateHits_ToHide] PRIMARY KEY CLUSTERED 
(
	[pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


