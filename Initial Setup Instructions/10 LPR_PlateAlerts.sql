USE [LPR]
GO

/****** Object:  StoredProcedure [dbo].[sp_LPR_PlateAlerts]    Script Date: 8/9/2019 9:11:57 PM ******/
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
		KP.Alert_Address
	From LPR_KnownPlates as KP
	Where
		KP.Plate = @Plate
END
GO

