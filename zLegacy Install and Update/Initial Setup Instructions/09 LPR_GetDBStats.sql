USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_GetDBStats]    Script Date: 7/31/2021 10:33:52 PM ******/
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


