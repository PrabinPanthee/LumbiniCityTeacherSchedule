CREATE PROCEDURE [dbo].[spSchedule_BulkInsert]
	@classSchedules dbo.ClassScheduleTVP READONLY
AS
BEGIN
    INSERT INTO ClassSchedule (SemesterInstanceId, TimeSlotId, SubjectId, TeacherId)
    SELECT SemesterInstanceId, TimeSlotId, SubjectId, TeacherId
    FROM @ClassSchedules;
END
