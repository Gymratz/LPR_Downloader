USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateHistory]    Script Date: 7/31/2021 9:38:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_PlateHistory]
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
		PH.epoch_time_end Desc
END
GO


