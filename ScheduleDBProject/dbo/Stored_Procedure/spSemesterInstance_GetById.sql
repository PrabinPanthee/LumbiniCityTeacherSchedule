CREATE PROCEDURE [dbo].[spSemesterInstance_GetById]
	@SemesterInstanceId INT
	
AS
BEGIN
	SELECT [SemesterInstanceId],[SemesterId],[StartDate],[SemesterStatus],[EndDate] FROM [dbo].[SemesterInstance] 
	WHERE [SemesterInstanceId] = @SemesterInstanceId
END
