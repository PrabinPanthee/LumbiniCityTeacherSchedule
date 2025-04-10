CREATE PROCEDURE [dbo].[spSemesterInstance_GetAllActiveSemester]
	
AS
BEGIN
	SELECT [SemesterInstanceId],[SemesterId],[StartDate],[EndDate],[SemesterStatus]
	FROM [dbo].[SemesterInstance]
	
END
