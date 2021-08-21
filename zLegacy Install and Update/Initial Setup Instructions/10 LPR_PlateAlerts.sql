USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateAlerts]    Script Date: 10/18/2020 1:27:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_PlateAlerts]
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
		AC.model
	From LPR_KnownPlates as KP
	Left Join LPR_AutoCheck as AC on AC.Plate = KP.Plate
	Where
		KP.Plate = @Plate
END
GO


