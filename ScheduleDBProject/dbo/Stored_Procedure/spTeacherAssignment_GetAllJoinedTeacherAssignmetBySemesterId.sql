CREATE PROCEDURE [dbo].[spTeacherAssignment_GetAllJoinedTeacherAssignmetBySemesterId]
	@SemesterId INT
	
AS
BEGIN
	SELECT [s].[SubjectId], [s].[SubjectName], [s].[SubjectCode], [t].[FirstName], [t].[LastName],[t].[NumberOfClasses]
	FROM [dbo].[Subject] s
	LEFT JOIN [dbo].[TeacherAssignment] ta ON s.SubjectId = ta.SubjectId
	LEFT JOIN [dbo].[Teacher] t ON ta.TeacherId = t.TeacherId
	WHERE s.SemesterId = @SemesterId
END
