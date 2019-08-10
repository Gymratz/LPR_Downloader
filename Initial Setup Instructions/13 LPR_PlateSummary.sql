USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateSummary]    Script Date: 8/9/2019 9:12:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_PlateSummary]
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

