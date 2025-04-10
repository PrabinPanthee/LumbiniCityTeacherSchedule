CREATE PROCEDURE [dbo].[spTimeSlot_GetByConfigId]
	@ConfigId INT
	
AS
BEGIN
	SELECT [ConfigId],[PeriodNumber],[StartTime],[EndTime] 
	FROM [dbo].[TimeSlot] 
	WHERE [ConfigId] = @ConfigId
END
