CREATE PROCEDURE [dbo].[spTeacherAssignment_GetAllBySemesterId]
	@SemesterId INT
	
AS
BEGIN
	SELECT ta.TeacherAssignmentId,ta.SubjectId,ta.TeacherId
	FROM [dbo].[TeacherAssignment] ta
	JOIN [dbo].[Subject] s ON ta.SubjectId = s.SubjectId
	JOIN [dbo].[Teacher] t ON ta.TeacherId = t.TeacherId
	WHERE s.SemesterId = @SemesterId
	
END