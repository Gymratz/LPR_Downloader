USE [LPR]
GO





---------------------------------------------LPR_AutoCheck---------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_AutoCheck')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_AutoCheck', 'LPR_AutoCheck_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_AutoCheck NVARCHAR(MAX) = N'';

		Select @SQL_LPR_AutoCheck += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_AutoCheck_OLD';

		Execute(@SQL_LPR_AutoCheck)

		-- Add new coluns since original version, in case people are on versions that don't have it, to allow copying
		IF COL_LENGTH('LPR_Autocheck_OLD', 'Color') IS NULL
			Alter Table LPR_Autocheck_OLD
			Add [Color] nvarchar(50) NULL
		
		IF COL_LENGTH('LPR_Autocheck_OLD', 'Date_Imported') IS NULL
			Alter Table LPR_Autocheck_OLD
			Add [Date_Imported] datetime NULL
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
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
		[Color] [nvarchar](50) NULL,
		[Date_Imported] [datetime] NULL,
	 CONSTRAINT [PK_LPR_AutoCheck_Prod] PRIMARY KEY CLUSTERED 
	(
		[Plate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_AutoCheck_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_AutoCheck]
			Select * From [LPR_AutoCheck_OLD]
		
		-- Delete old table
		Drop Table [LPR_AutoCheck_OLD]
	END



---------------------------------------------LPR_AutoHidePlates----------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_AutoHidePlates')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_AutoHidePlates', 'LPR_AutoHidePlates_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_AutoHidePlates NVARCHAR(MAX) = N'';

		Select @SQL_LPR_AutoHidePlates += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_AutoHidePlates_OLD';

		Execute(@SQL_LPR_AutoHidePlates)
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_AutoHidePlates](
		[Plate] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_AutoHidePlates_Prod] PRIMARY KEY CLUSTERED 
	(
		[Plate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_AutoHidePlates_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_AutoHidePlates]
			Select * From [LPR_AutoHidePlates_OLD]
		
		-- Delete old table
		Drop Table [LPR_AutoHidePlates_OLD]
	END


---------------------------------------------LPR_ImportHistory-----------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_ImportHistory')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_ImportHistory', 'LPR_ImportHistory_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_ImportHistory NVARCHAR(MAX) = N'';

		Select @SQL_LPR_ImportHistory += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_ImportHistory_OLD';

		Execute(@SQL_LPR_ImportHistory)

		-- Delete the ID column so no issues copying in the later step
		IF COL_LENGTH('LPR_ImportHistory_OLD', 'ID') IS NOT NULL
			Alter Table LPR_ImportHistory_OLD DROP COLUMN ID;

		-- Add new coluns since original version, in case people are on versions that don't have it, to allow copying
		IF COL_LENGTH('LPR_ImportHistory_OLD', 'Count_Updated') IS NULL
			Alter Table LPR_ImportHistory_OLD
			Add Count_Updated int NULL;
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_ImportHistory](
		[Import_Time] [datetime] NULL,
		[Count_Imported] [int] NULL,
		[Count_Skipped] [int] NULL,
		[Count_Updated] [int] NULL,
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_ImportHistory_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_ImportHistory]
			Select * From [LPR_ImportHistory_OLD]
		
		-- Delete old table
		Drop Table [LPR_ImportHistory_OLD]
	END


---------------------------------------------LPR_KnownPlates-------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_KnownPlates')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_KnownPlates', 'LPR_KnownPlates_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_KnownPlates NVARCHAR(MAX) = N'';

		Select @SQL_LPR_KnownPlates += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_KnownPlates_OLD';

		Execute(@SQL_LPR_KnownPlates)

		-- Add new coluns since original version, in case people are on versions that don't have it, to allow copying
		IF COL_LENGTH('LPR_KnownPlates_OLD', 'Pushover') IS NULL
			Alter Table LPR_KnownPlates_OLD
			Add Pushover bit NULL;

		IF COL_LENGTH('LPR_KnownPlates_OLD', 'Priority') IS NULL
			Alter Table LPR_KnownPlates_OLD
			Add Priority bit NULL;
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_KnownPlates](
		[Plate] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](100) NULL,
		[Status] [nvarchar](100) NULL,
		[Alert_Address] [nvarchar](250) NULL,
		[Pushover] [bit] NULL,
		[Priority] [bit] NULL,
	 CONSTRAINT [PK_KnownPlates_Prod] PRIMARY KEY CLUSTERED 
	(
		[Plate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_KnownPlates_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_KnownPlates]
			Select * From [LPR_KnownPlates_OLD]
		
		-- Delete old table
		Drop Table [LPR_KnownPlates_OLD]
	END


---------------------------------------------LPR_LocalInfo---------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_LocalInfo')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_LocalInfo', 'LPR_LocalInfo_OLD'

		-- Delete duplicate best_uuid from _OLD
		Delete
		From LPR_LocalInfo_OLD
		Where
			UUID in
			(
				Select
					T1.UUID
				From
				(
					Select
						UUID,
						Count(*) as [Cnt]
					From LPR_LocalInfo_OLD
					Group By UUID
				) as T1
				Where
					T1.Cnt > 1
			)

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_LocalInfo NVARCHAR(MAX) = N'';

		Select @SQL_LPR_LocalInfo += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_LocalInfo_OLD';

		Execute(@SQL_LPR_LocalInfo)
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_LocalInfo](
		[UUID] [nvarchar](100) NOT NULL,
		[best_color] [nvarchar](50) NULL,
		[best_make] [nvarchar](50) NULL,
		[best_model] [nvarchar](50) NULL,
	 CONSTRAINT [PK_LPR_LocalInfo_Prod] PRIMARY KEY CLUSTERED 
	(
		[UUID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_LocalInfo_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_LocalInfo]
			Select * From [LPR_LocalInfo_OLD]
		
		-- Delete old table
		Drop Table [LPR_LocalInfo_OLD]
	END


---------------------------------------------LPR_PlateCorrections--------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateCorrections')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_PlateCorrections', 'LPR_PlateCorrections_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_PlateCorrections NVARCHAR(MAX) = N'';

		Select @SQL_LPR_PlateCorrections += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_PlateCorrections_OLD';

		Execute(@SQL_LPR_PlateCorrections)
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_PlateCorrections](
		[wrongPlate] [nvarchar](50) NOT NULL,
		[rightPlate] [nvarchar](50) NULL,
	 CONSTRAINT [PK_PlateCorrections_Prod] PRIMARY KEY CLUSTERED 
	(
		[wrongPlate] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateCorrections_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_PlateCorrections]
			Select * From [LPR_PlateCorrections_OLD]
		
		-- Delete old table
		Drop Table [LPR_PlateCorrections_OLD]
	END


---------------------------------------------LPR_PlateHits---------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateHits')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_PlateHits', 'LPR_PlateHits_OLD'

		-- Delete duplicate best_uuid from _OLD
		Delete
		From LPR_PlateHits_OLD
		Where
			best_uuid in
			(
				Select
					T1.best_uuid
				From
				(
					Select
						best_uuid,
						Count(*) as [Cnt]
					From LPR_PlateHits_OLD
					Group By best_uuid
				) as T1
				Where
					T1.Cnt > 1
			)

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_PlateHits NVARCHAR(MAX) = N'';

		Select @SQL_LPR_PlateHits += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_PlateHits_OLD';

		Execute(@SQL_LPR_PlateHits)
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_PlateHits](
		[pk] [int] NULL,
		[agent_type] [nvarchar](50) NULL,
		[agent_uid] [nvarchar](50) NULL,
		[best_confidence] [float] NULL,
		[best_index] [tinyint] NULL,
		[best_plate] [nvarchar](50) NOT NULL,
		[best_uuid] [nvarchar](100) NOT NULL,
		[camera] [nvarchar](50) NULL,
		[camera_id] [int] NULL,
		[company] [int] NULL,
		[crop_location] [tinyint] NULL,
		[direction_of_travel_degrees] [smallint] NULL,
		[direction_of_travel_id] [smallint] NULL,
		[epoch_time_end] [nvarchar](50) NULL,
		[epoch_time_start] [nvarchar](50) NULL,
		[gps_latitude] [nvarchar](50) NULL,
		[gps_longitude] [nvarchar](50) NULL,
		[hit_count] [smallint] NULL,
		[img_height] [smallint] NULL,
		[img_width] [smallint] NULL,
		[plate_x1] [smallint] NOT NULL,
		[plate_x2] [smallint] NOT NULL,
		[plate_x3] [smallint] NOT NULL,
		[plate_x4] [smallint] NOT NULL,
		[plate_y1] [smallint] NOT NULL,
		[plate_y2] [smallint] NOT NULL,
		[plate_y3] [smallint] NOT NULL,
		[plate_y4] [smallint] NOT NULL,
		[processing_time_ms] [float] NULL,
		[region] [nvarchar](50) NOT NULL,
		[region_confidence] [float] NULL,
		[site] [nvarchar](50) NULL,
		[site_id] [smallint] NULL,
		[vehicle_body_type] [nvarchar](50) NULL,
		[vehicle_body_type_confidence] [nvarchar](50) NULL,
		[vehicle_color] [nvarchar](50) NULL,
		[vehicle_color_confidence] [nvarchar](50) NULL,
		[vehicle_make] [nvarchar](50) NULL,
		[vehicle_make_confidence] [nvarchar](50) NULL,
		[vehicle_make_model] [nvarchar](50) NULL,
		[vehicle_make_model_confidence] [nvarchar](50) NULL,
		[vehicle_region_height] [smallint] NOT NULL,
		[vehicle_region_width] [smallint] NOT NULL,
		[vehicle_region_x] [smallint] NOT NULL,
		[vehicle_region_y] [smallint] NOT NULL,
	 CONSTRAINT [Unique_UUID] UNIQUE NONCLUSTERED 
	(
		[best_uuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateHits_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_PlateHits]
			Select * From [LPR_PlateHits_OLD]
		
		-- Delete old table
		Drop Table [LPR_PlateHits_OLD]
	END




---------------------------------------------LPR_PlateHits_ToHide--------------------------------------------
-------------------------------------------------------------------------------------------------------------
-- Checks if table exists
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateHits_ToHide')
	BEGIN
		-- If it does, rename it to _OLD
		EXEC sp_rename 'LPR_PlateHits_ToHide', 'LPR_PlateHits_ToHide_OLD'

		-- Delete all constraints so we don't run into duplicate issues in the future
		Declare @SQL_LPR_PlateHits_ToHide NVARCHAR(MAX) = N'';

		Select @SQL_LPR_PlateHits_ToHide += N'
		ALTER TABLE ' + OBJECT_NAME(PARENT_OBJECT_ID) + ' DROP CONSTRAINT ' + OBJECT_NAME(OBJECT_ID) + ';' 
		From SYS.OBJECTS
		Where TYPE_DESC LIKE '%CONSTRAINT' AND OBJECT_NAME(PARENT_OBJECT_ID) = 'LPR_PlateHits_ToHide_OLD';

		Execute(@SQL_LPR_PlateHits_ToHide)
	END

-- Creates the table (Both for new, and updates since the old one is renamed)
	CREATE TABLE [dbo].[LPR_PlateHits_ToHide](
		[pk] [int] NOT NULL,
		[reason] [nvarchar](50) NULL,
		[date_added] [datetime] NULL,
	 CONSTRAINT [PK_PlateHits_ToHide] PRIMARY KEY CLUSTERED 
	(
		[pk] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

-- If this is an update, the "OLD" table will exist - need to move data over from it and then clean it up.
IF Exists	(Select 1 From sys.Tables Where	Name = N'LPR_PlateHits_ToHide_OLD')
	BEGIN
		-- Copy all data to new table
		Insert Into [LPR_PlateHits_ToHide]
			Select * From [LPR_PlateHits_ToHide_OLD]
		
		-- Delete old table
		Drop Table [LPR_PlateHits_ToHide_OLD]
	END

-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
---------------------------------------------Stored Procedures-----------------------------------------------
-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_AllPlates]    Script Date: 8/20/2021 1:07:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_AllPlates]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@Plate nvarchar(50),
	@HideNeighbors bit = 0,
	@CurrentOffset varchar(10) = '-07:00',
	@IdentifyDupes int = 0,
	@TopPH int = 999,
	@Status nvarchar(100) = '%',
	@Camera nvarchar(50) = '%',
	@Desc nvarchar(50) = '%',
	@Make nvarchar(50) = '%',
	@Model nvarchar(50) = '%',
	@Color nvarchar(50) = '%',
	@VIN nvarchar(50) = '%'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Top (@TopPH)
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) as [Local Time],
		PH.best_plate as [Plate],
		Case
			When PH.direction_of_travel_degrees <= 22.5 Then N'⬆'
			When PH.direction_of_travel_degrees <= 67.5 Then N'⬈'
			When PH.direction_of_travel_degrees <= 112.5 Then N'→'
			When PH.direction_of_travel_degrees <= 157.5 Then N'⬊'
			When PH.direction_of_travel_degrees <= 202.5 Then N'⬇'
			When PH.direction_of_travel_degrees <= 247.5 Then N'⬋'
			When PH.direction_of_travel_degrees <= 292.5 Then N'←'
			When PH.direction_of_travel_degrees <= 337.5 Then N'⬉'
			Else N'⬆'
		End as [D],
		KP.Description,
		PH.region as [Region],
		PH.vehicle_color as [Color],
		PH.vehicle_make as [Make],
		PH.vehicle_make_model as [Model],
		PH.vehicle_body_type as [Body],
		PH.best_uuid as [Picture],
		(
			Select
				Count(PHinner.best_plate)
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) > dateadd(hour, -24, GetDate()) AND
				PHTHinner.reason is NULL
		) as [Hits Day],
		(
			Select
				Count(PHinner.best_plate)
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) > dateadd(day, -7, GetDate()) AND
				PHTHinner.reason is NULL
		) as [Hits Week],
		(
			Select
				Count(Distinct CONVERT(date, Cast(switchoffset(Cast(PHinner.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime)))
			From LPR_PlateHits as PHinner
			Left Join LPR_PlateHits_ToHide as PHTHinner on PHTHinner.pk = PHinner.pk
			Where
				PHinner.best_plate = PH.best_plate AND
				PHTHinner.reason is NULL		
		) as [Distinct Days],
		KP.Status,
		PH.plate_x1, 
		PH.plate_x2, 
		PH.plate_x3, 
		PH.plate_x4,
		PH.plate_y1,
		PH.plate_y2,
		PH.plate_y3,
		PH.plate_y4,
		PH.vehicle_region_height,
		PH.vehicle_region_width,
		PH.vehicle_region_x,
		PH.vehicle_region_y,
		PH.pk,
		KP.Alert_Address,
		LPRAC.vin as [VIN],
		LPRAC.year as [Yr],
		LPRAC.Color as [Car Color],
		LPRAC.make as [Car Make],
		LPRAC.model as [Car Model],
		LocalInfo.best_color as [ALPR Color],
		LocalInfo.best_model as [ALPR Model]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_AutoCheck as LPRAC on LPRAC.Plate = PH.best_plate
	Left Join LPR_LocalInfo as LocalInfo on LocalInfo.UUID = PH.best_uuid
	Where
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) between @StartDate and @EndDate AND
		(@HideNeighbors = 0 OR IsNull(KP.Status, '') <> 'Neighbor') AND
		(@IdentifyDupes = 0 OR
		(
			Select
				Count(*)
			From LPR_PlateHits as PHDup
			Left Join LPR_PlateHits_ToHide as PHTHDup on PHTHDup.pk = PHDup.pk
			Where
				PHDup.best_plate = PH.best_plate AND
				datediff(second, PH.epoch_time_end, PHDup.epoch_time_end) between -30 and 30 AND
				PHDup.pk <> PH.pk AND
				PHTHDup.reason is NULL
		) >= @IdentifyDupes) AND
		(@Status = '%' OR IsNull(KP.Status, '') like @Status) AND
		(@Camera = '%' OR IsNull(PH.camera, '') like @Camera) AND
		(@Plate = '%' OR PH.best_plate like @Plate) AND
		(@Desc = '%' OR KP.Description like @Desc) AND
		(@Color = '%' OR LPRAC.Color like @Color OR LocalInfo.best_color like @Color) AND
		(@Make = '%' OR LPRAC.Make like @Make OR LocalInfo.best_make like @Make) AND
		(@Model = '%' OR LPRAC.model like @Model OR LocalInfo.best_model like @Model) AND
		(@VIN = '%' OR LPRAC.vin like @VIN) AND
		(@Plate <> '%' OR PH.pk not in (Select PHTH.pk from LPR_PlateHits_ToHide as PHTH))
	Order By
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) Desc
END
GO


----------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_FixPlates]    Script Date: 8/20/2021 1:08:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_FixPlates]
	-- Add the parameters for the stored procedure here


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update
		LPR_PlateHits
	Set
		LPR_PlateHits.best_plate = PC.rightPlate
	From LPR_PlateHits as PH
	Inner Join LPR_PlateCorrections as PC on PC.wrongPlate = PH.best_plate

	Insert Into LPR_PlateHits_ToHide (pk, reason, date_added)
	Select
		PH.pk, 'Auto-Hide', GetDate()
	From LPR_PlateHits as PH
	Where
		PH.best_plate in
		(
			Select
				AHP.Plate
			From LPR_AutoHidePlates as AHP
		) AND
		PH.pk not in
		(
			Select
				PHTH.pk
			From LPR_PlateHits_ToHide as PHTH
		)
END
GO

--------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_GetDBStats]    Script Date: 8/20/2021 1:08:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_GetDBStats]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@Plate nvarchar(50),
	@HideNeighbors bit = 0,
	@CurrentOffset varchar(10) = '-07:00',
	@IdentifyDupes int = 0,
	@TopPH int = 999,
	@Status nvarchar(100) = '%',
	@Camera nvarchar(50) = '%',
	@Desc nvarchar(50) = '%',
	@Make nvarchar(50) = '%',
	@Model nvarchar(50) = '%',
	@Color nvarchar(50) = '%',
	@VIN nvarchar(50) = '%'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
		Count(*) as [Displayed_Total],
		Count(Distinct PH.best_plate) as [Displayed_Distinct],
		(
			Select
				Count(*) as [All_Total]
			From LPR_PlateHits as PHAll
		) as [All_Total],
		(
			Select
				Count(Distinct PHAll.best_plate) as [All_Distinct]
			From LPR_PlateHits as PHAll
		) as [All_Distinct]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_AutoCheck as LPRAC on LPRAC.Plate = PH.best_plate
	Left Join LPR_LocalInfo as LocalInfo on LocalInfo.UUID = PH.best_uuid
	Where
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) >= @StartDate AND
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) <= @EndDate AND
		IsNull(KP.Status, '') <> Case When @HideNeighbors = 0 then 'NeverHide' When @HideNeighbors = 1 then 'Neighbor' end AND
		(@IdentifyDupes = 0 OR
		(
			Select
				Count(*)
			From LPR_PlateHits as PHDup
			Left Join LPR_PlateHits_ToHide as PHTHDup on PHTHDup.pk = PHDup.pk
			Where
				PHDup.best_plate = PH.best_plate AND
				datediff(second, PH.epoch_time_end, PHDup.epoch_time_end) between -30 and 30 AND
				PHDup.pk <> PH.pk AND
				PHTHDup.reason is NULL
		) >= @IdentifyDupes) AND
		(@Status = '%' OR IsNull(KP.Status, '') like @Status) AND
		(@Camera = '%' OR IsNull(PH.camera, '') like @Camera) AND
		(@Plate = '%' OR PH.best_plate like @Plate) AND
		(@Desc = '%' OR KP.Description like @Desc) AND
		(@Color = '%' OR LPRAC.Color like @Color OR LocalInfo.best_color like @Color) AND
		(@Make = '%' OR LPRAC.Make like @Make OR LocalInfo.best_make like @Make) AND
		(@Model = '%' OR LPRAC.model like @Model OR LocalInfo.best_model like @Model) AND
		(@VIN = '%' OR LPRAC.vin like @VIN) AND
		(@Plate <> '%' OR PH.pk not in (Select PHTH.pk from LPR_PlateHits_ToHide as PHTH))
END
GO

------------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateAlerts]    Script Date: 8/20/2021 1:09:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_PlateAlerts]
	-- Add the parameters for the stored procedure here
	@Plate nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
		KP.Alert_Address,
		KP.Description,
		KP.Status,
		AC.year,
		AC.make,
		AC.model,
		KP.Pushover,
		KP.[Priority]
	From LPR_KnownPlates as KP
	Left Join LPR_AutoCheck as AC on AC.Plate = KP.Plate
	Where
		KP.Plate = @Plate
END
GO

---------------------------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateChart]    Script Date: 8/20/2021 1:09:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_PlateChart]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@CurrentOffset varchar(10) = '-07:00'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
		T1.Hour,
		T1.Status,
		Count(*) as [Hits]
	From
	(
		Select
			dateadd(hour, datediff(hour, 0, Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), '-07:00') as datetime)), 0) as [Hour],
			'All' as [Status]
		From LPR_PlateHits as PH
		Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
		Left Join LPR_PlateHits_ToHide as PHTH on PHTH.pk = PH.pk
		Where
			Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) >= @StartDate AND
			Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) <= @EndDate AND
			PHTH.reason is NULL
	) as T1
	Group By
		T1.Hour,
		T1.Status
	Order By
		T1.Status,
		T1.Hour
END
GO

-------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateHistory]    Script Date: 8/20/2021 1:09:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_PlateHistory]
	-- Add the parameters for the stored procedure here
	@Plate nvarchar(50),
	@CurrentOffset varchar(10) = '-07:00',
	@TopPH int = 999

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	Select Top (@TopPH)
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) as [Local Time],
		PH.best_plate as [Plate],
		Case
			When PH.direction_of_travel_degrees <= 22.5 Then N'⬆'
			When PH.direction_of_travel_degrees <= 67.5 Then N'⬈'
			When PH.direction_of_travel_degrees <= 112.5 Then N'→'
			When PH.direction_of_travel_degrees <= 157.5 Then N'⬊'
			When PH.direction_of_travel_degrees <= 202.5 Then N'⬇'
			When PH.direction_of_travel_degrees <= 247.5 Then N'⬋'
			When PH.direction_of_travel_degrees <= 292.5 Then N'←'
			When PH.direction_of_travel_degrees <= 337.5 Then N'⬉'
			Else N'⬆'
		End as [D],
		KP.Description,
		PH.region as [Region],
		PH.vehicle_color as [Color],
		PH.vehicle_make as [Make],
		PH.vehicle_make_model as [Model],
		PH.vehicle_body_type as [Body],
		PH.best_uuid as [Picture],
		KP.Status,
		PH.plate_x1, 
		PH.plate_x2, 
		PH.plate_x3, 
		PH.plate_x4,
		PH.plate_y1,
		PH.plate_y2,
		PH.plate_y3,
		PH.plate_y4,
		PH.vehicle_region_height,
		PH.vehicle_region_width,
		PH.vehicle_region_x,
		PH.vehicle_region_y,
		PH.pk,
		KP.Alert_Address,
		KP.Pushover,
		KP.[Priority],
		LPRAC.vin as [VIN],
		LPRAC.year as [Yr],
		LPRAC.make as [Car Make],
		LPRAC.model as [Car Model],
		LPRAC.Color as [Car Color],
		LocalInfo.best_color as [ALPR Color],
		LocalInfo.best_model as [ALPR Model]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_AutoCheck as LPRAC on LPRAC.Plate = PH.best_plate
	Left Join LPR_LocalInfo as LocalInfo on LocalInfo.UUID = PH.best_uuid
	Where
		PH.best_plate like @Plate
	Order By
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) Desc
END
GO

--------------------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlatePie]    Script Date: 8/20/2021 1:09:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_PlatePie]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@CurrentOffset varchar(10) = '-07:00'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
		T1.Status,
		Count(*) as [Hits]
	From
	(
		Select
			Case
				When KP.Status is NULL then 'Cut-Thru'
				When KP.Status = '' then 'Cut-Thru'
				Else KP.Status
			End as [Status]
		From LPR_PlateHits as PH
		Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
		Left Join LPR_PlateHits_ToHide as PHTH on PHTH.pk = PH.pk
		Where
			Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) >= @StartDate AND
			Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) <= @EndDate AND
			PHTH.reason is NULL
	) as T1
	Group By
		T1.Status
	Order By
		T1.Status
END
GO

-------------------------------
USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateSummary]    Script Date: 8/20/2021 1:10:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_LPR_PlateSummary]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@CurrentOffset varchar(10) = '-07:00'

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Distinct
		PH.best_plate as [Plate],
		KP.Description,
		KP.Status,
		Count(*) as [NumberHits]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_PlateHits_ToHide as PHTH on PHTH.pk = PH.pk
	Where
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) >= @StartDate AND
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) <= @EndDate AND
		PHTH.reason is NULL
	Group By
		PH.best_plate,
		KP.Description,
		KP.Status
	Order By
		NumberHits Desc
END
GO

--------------------------
