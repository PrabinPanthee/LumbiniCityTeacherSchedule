CREATE PROCEDURE [dbo].[spClassSchedule_GetAll]
	
AS
BEGIN
	SELECT [ScheduleId], [SemesterInstanceId], [TimeSlotId], [SubjectId], [TeacherId]
	FROM [dbo].[ClassSchedule]
END
