CREATE PROCEDURE [dbo].[spClassScheduleWithTimeSlot]
	
AS
BEGIN
	Select [cs].[TeacherId],cs.SubjectId,[ts].[StartTime], [ts].[EndTime],[cs].[SemesterInstanceId],[cs].[TimeSlotId]
	FROM [dbo].[ClassSchedule] cs
	JOIN [dbo].[TimeSlot] ts ON cs.TimeSlotId = ts.TimeSlotId
END
