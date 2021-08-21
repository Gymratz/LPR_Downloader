USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_AutoHidePlates]    Script Date: 10/18/2020 1:22:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_AutoHidePlates](
	[Plate] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AutoHidePlates] PRIMARY KEY CLUSTERED 
(
	[Plate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

