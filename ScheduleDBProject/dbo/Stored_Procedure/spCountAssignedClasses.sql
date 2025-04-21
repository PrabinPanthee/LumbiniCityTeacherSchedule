CREATE PROCEDURE [dbo].[spCountAssignedClasses]
	@TeacherId INT
AS
BEGIN
	    DECLARE @CurrentAssignedClasses INT;
        SELECT @CurrentAssignedClasses = COUNT(DISTINCT cs.ScheduleId)
        FROM [dbo].[ClassSchedule] cs
        WHERE cs.TeacherId = @TeacherId;
        
        SELECT @CurrentAssignedClasses;
END
