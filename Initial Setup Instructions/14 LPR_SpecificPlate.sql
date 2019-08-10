USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_SpecificPlate]    Script Date: 8/9/2019 9:12:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_SpecificPlate]
	-- Add the parameters for the stored procedure here
	@Plate nvarchar(50),
	@CurrentOffset varchar(10) = '-07:00'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select
		Cast(switchoffset(Cast(PH.epoch_time_end as datetimeoffset), @CurrentOffset) as datetime) as [Local Time],
		PH.region as [Region],
		PH.vehicle_color as [Color],
		PH.vehicle_make as [Make],
		PH.vehicle_make_model as [Model],
		PH.vehicle_body_type as [Body],
		PH.pk
	From LPR_PlateHits as PH
	Left Join LPR_KnownPlates as KP on KP.Plate = PH.best_plate
	Where
		PH.best_plate = @Plate
	Order By
		PH.epoch_time_end Desc
END
GO

