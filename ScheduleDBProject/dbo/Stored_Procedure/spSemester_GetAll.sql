CREATE PROCEDURE [dbo].[spSemester_GetAll]
	
AS
BEGIN
	SELECT [SemesterId],[ProgramId],[SemesterNumber]
	FROM dbo.Semester
END