USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_PlateCorrections]    Script Date: 8/9/2019 9:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_PlateCorrections](
	[wrongPlate] [nvarchar](50) NOT NULL,
	[rightPlate] [nvarchar](50) NULL,
 CONSTRAINT [PK_PlateCorrections] PRIMARY KEY CLUSTERED 
(
	[wrongPlate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

