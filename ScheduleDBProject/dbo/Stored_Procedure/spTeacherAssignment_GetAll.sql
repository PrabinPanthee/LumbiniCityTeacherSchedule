CREATE PROCEDURE [dbo].[spTeacherAssignment_GetAll]
	
AS
BEGIN
	SELECT t.FirstName, t.LastName, s.SubjectName, s.SubjectCode
	FROM [dbo].[TeacherAssignment] ta
	JOIN [dbo].[Subject] s ON ta.SubjectId = s.SubjectId
	JOIN [dbo].[Teacher] t ON ta.TeacherId = t.TeacherId
	ORDER BY t.FirstName
END