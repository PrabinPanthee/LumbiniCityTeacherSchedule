CREATE PROCEDURE [dbo].[spTimeSlot_GetByConfigId]
	@ConfigId INT
	
AS
BEGIN
	SELECT [TimeSlotId], [ConfigId],[PeriodNumber],[StartTime],[EndTime],[Type]
	FROM [dbo].[TimeSlot] 
	WHERE [ConfigId] = @ConfigId
END
