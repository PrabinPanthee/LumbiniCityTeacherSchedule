CREATE PROCEDURE [dbo].[spSemester_Get]
	@SemesterId INT 
AS
BEGIN
	SELECT [SemesterId],[ProgramId],[SemesterNumber]
	FROM [dbo].[Semester]
	WHERE SemesterId = @SemesterId;
END
