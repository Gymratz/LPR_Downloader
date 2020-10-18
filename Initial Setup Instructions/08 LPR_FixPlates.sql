USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_FixPlates]    Script Date: 10/18/2020 1:26:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LPR_FixPlates]
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


