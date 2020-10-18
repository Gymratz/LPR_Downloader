USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_AutoCheck]    Script Date: 10/18/2020 1:21:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_AutoCheck](
	[Plate] [nvarchar](50) NOT NULL,
	[code] [nvarchar](50) NULL,
	[vin] [nvarchar](100) NULL,
	[year] [nvarchar](50) NULL,
	[make] [nvarchar](50) NULL,
	[model] [nvarchar](150) NULL,
	[countryOfAssembly] [nvarchar](50) NULL,
	[body] [nvarchar](50) NULL,
	[vehicleClass] [nvarchar](50) NULL,
	[recordCount] [nvarchar](50) NULL,
	[scoreRangeLow] [nvarchar](50) NULL,
	[scoreRangeHigh] [nvarchar](50) NULL,
	[buybackAssurance] [nvarchar](50) NULL,
	[lemonRecord] [nvarchar](50) NULL,
	[accidentRecord] [nvarchar](50) NULL,
	[floodRecord] [nvarchar](50) NULL,
	[singleOwner] [nvarchar](50) NULL,
	[engine] [nvarchar](50) NULL,
	[status] [nvarchar](50) NULL,
 CONSTRAINT [PK_LPR_AutoCheck] PRIMARY KEY CLUSTERED 
(
	[Plate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


