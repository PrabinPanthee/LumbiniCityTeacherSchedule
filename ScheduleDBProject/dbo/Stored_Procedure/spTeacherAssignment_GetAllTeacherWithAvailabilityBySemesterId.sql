CREATE PROCEDURE [dbo].[spTeacherAssignment_GetAllTeacherWithAvailabilityBySemesterId]
	@SemesterId INT
	
AS
BEGIN
	SELECT DISTINCT
		t.TeacherId,
        t.FirstName,
		t.NumberOfClasses,
        t.LastName,
        ta.StartTime,
        ta.EndTime
	FROM Teacher t
	JOIN TeacherAssignment ta2 ON ta2.TeacherId = t.TeacherId
	JOIN [dbo].[Subject] s ON s.SubjectId = ta2.SubjectId
	JOIN [dbo].[TeacherAvailability] ta ON ta.TeacherId = t.TeacherId
	WHERE s.SemesterId =@SemesterId
	
END