CREATE PROCEDURE [dbo].[spIsProgramLinkedToActiveSemester]
	@ProgramId int
	
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(
	SELECT 1
	FROM [dbo].[Semester] s
	JOIN [dbo].[SemesterInstance] si
	ON s.SemesterId = si.SemesterId
	WHERE s.ProgramId = @ProgramId 
	AND si.SemesterStatus = 'active'
	)
		SELECT 1;
	ELSE
		SELECT 0;
END