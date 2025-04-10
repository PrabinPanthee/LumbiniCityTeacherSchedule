CREATE PROCEDURE [dbo].[spSemesterInstance_GetActiveInstanceBySemesterId]
	@SemesterId INT
		
AS
BEGIN
	SELECT [SemesterInstanceId],[StartDate],[EndDate],[SemesterId],[SemesterStatus]
	FROM [dbo].[SemesterInstance] WHERE SemesterId = @SemesterId AND SemesterStatus = 'active'
END
