USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_AllPlates]    Script Date: 11/14/2020 12:44:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_AllPlates]
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
	@SearchBy nvarchar(50) = 'Plate'

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
		LPRAC.make as [Car Make],
		LPRAC.model as [Car Model]
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Left Join LPR_PlateHits_ToHide as PHTH on PHTH.pk = PH.pk
	Left Join LPR_AutoCheck as LPRAC on LPRAC.Plate = PH.best_plate
	Where
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) >= @StartDate AND
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) <= @EndDate AND
		IsNull(KP.Status, '') <> Case When @HideNeighbors = 0 then 'NeverHide' When @HideNeighbors = 1 then 'Neighbor' end AND
		PHTH.reason is NULL AND
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
		(@Plate = '%' OR 
			Case
				When @SearchBy = 'Plate' then PH.best_plate
				When @SearchBy = 'Description' then KP.Description
				When @SearchBy = 'Make' then LPRAC.make
				When @SearchBy = 'Model' then LPRAC.model
				Else PH.best_plate
			End like @Plate
		)
	Order By
		PH.epoch_time_end Desc
END
GO


