CREATE PROCEDURE [dbo].[spSemester_DuplicateSemesterNumber]
	@SemesterNumber int,
	@ProgramId INT
AS
BEGIN
	IF EXISTS(
			SELECT 1 FROM [dbo].[Semester]
			WHERE [ProgramId]=@ProgramId
			AND [SemesterNumber] = @SemesterNumber
			)
			SELECT 1;
			ELSE
			SELECT 0;
END

