CREATE PROCEDURE [dbo].[spSemester_GetAllActiveSemesterByProgramId]
	@ProgramId INT
AS
BEGIN
	SELECT [s].[SemesterId], [s].[ProgramId],[s].[SemesterNumber]
	FROM [dbo].Semester s
	JOIN [dbo].SemesterInstance si on si.SemesterId = s.SemesterId
	where si.SemesterStatus = 'active' and s.ProgramId = @ProgramId
END
