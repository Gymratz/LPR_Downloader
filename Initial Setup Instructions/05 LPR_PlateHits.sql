USE [LPR]
GO

/****** Object:  Table [dbo].[LPR_PlateHits]    Script Date: 10/18/2020 1:25:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LPR_PlateHits](
	[pk] [int] NOT NULL,
	[agent_type] [nvarchar](50) NOT NULL,
	[agent_uid] [nvarchar](50) NOT NULL,
	[best_confidence] [float] NOT NULL,
	[best_index] [tinyint] NOT NULL,
	[best_plate] [nvarchar](50) NOT NULL,
	[best_uuid] [nvarchar](100) NOT NULL,
	[camera] [nvarchar](50) NOT NULL,
	[camera_id] [smallint] NOT NULL,
	[company] [int] NOT NULL,
	[crop_location] [tinyint] NOT NULL,
	[direction_of_travel_degrees] [smallint] NOT NULL,
	[direction_of_travel_id] [smallint] NOT NULL,
	[epoch_time_end] [nvarchar](50) NOT NULL,
	[epoch_time_start] [nvarchar](50) NOT NULL,
	[gps_latitude] [nvarchar](50) NOT NULL,
	[gps_longitude] [nvarchar](50) NOT NULL,
	[hit_count] [smallint] NOT NULL,
	[img_height] [smallint] NOT NULL,
	[img_width] [smallint] NOT NULL,
	[plate_x1] [smallint] NOT NULL,
	[plate_x2] [smallint] NOT NULL,
	[plate_x3] [smallint] NOT NULL,
	[plate_x4] [smallint] NOT NULL,
	[plate_y1] [smallint] NOT NULL,
	[plate_y2] [smallint] NOT NULL,
	[plate_y3] [smallint] NOT NULL,
	[plate_y4] [smallint] NOT NULL,
	[processing_time_ms] [float] NOT NULL,
	[region] [nvarchar](50) NOT NULL,
	[region_confidence] [float] NOT NULL,
	[site] [nvarchar](50) NOT NULL,
	[site_id] [smallint] NOT NULL,
	[vehicle_body_type] [nvarchar](50) NOT NULL,
	[vehicle_body_type_confidence] [nvarchar](50) NOT NULL,
	[vehicle_color] [nvarchar](50) NOT NULL,
	[vehicle_color_confidence] [nvarchar](50) NOT NULL,
	[vehicle_make] [nvarchar](50) NOT NULL,
	[vehicle_make_confidence] [nvarchar](50) NOT NULL,
	[vehicle_make_model] [nvarchar](50) NOT NULL,
	[vehicle_make_model_confidence] [nvarchar](50) NOT NULL,
	[vehicle_region_height] [smallint] NOT NULL,
	[vehicle_region_width] [smallint] NOT NULL,
	[vehicle_region_x] [smallint] NOT NULL,
	[vehicle_region_y] [smallint] NOT NULL,
 CONSTRAINT [PK_PlateHits] PRIMARY KEY CLUSTERED 
(
	[pk] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


