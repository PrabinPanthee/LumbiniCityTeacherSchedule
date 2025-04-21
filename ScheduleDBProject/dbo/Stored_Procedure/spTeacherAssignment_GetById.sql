CREATE PROCEDURE [dbo].[spTeacherAssignment_GetById]
	@TeacherAssignmentId INT
	
AS
BEGIN
	SELECT [TeacherAssignmentId],[SubjectId],[TeacherId]
	FROM [dbo].[TeacherAssignment]
	WHERE [TeacherAssignmentId] = @TeacherAssignmentId
END