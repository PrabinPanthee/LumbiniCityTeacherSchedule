CREATE PROCEDURE [dbo].[spSemesterInstance_GetAllJoinedActiveData]
AS
BEGIN
	SELECT [p].[ProgramName],[s].[SemesterNumber], [si].[SemesterInstanceId],[si].[StartDate], [si].[SemesterStatus]
	FROM [dbo].[Program] p 
	JOIN [dbo].[Semester] s ON p.ProgramId = s.ProgramId
	JOIN [dbo].[SemesterInstance] si ON si.SemesterId = s.SemesterId
	WHERE si.SemesterStatus = 'active'
END
