CREATE PROCEDURE [dbo].[spSemester_GetAllByProgramId]
	@ProgramId INT
	
AS
BEGIN
	SELECT [SemesterId], [ProgramId], [SemesterNumber] 
	FROM [dbo].[Semester]
	WHERE [ProgramId] = @ProgramId
	ORDER BY [SemesterNumber]
END