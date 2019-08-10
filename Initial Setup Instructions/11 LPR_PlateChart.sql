USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateChart]    Script Date: 8/9/2019 9:12:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_PlateChart]
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

