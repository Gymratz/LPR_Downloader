USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_GetDBStats]    Script Date: 11/14/2020 12:44:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_GetDBStats]
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
END
GO


