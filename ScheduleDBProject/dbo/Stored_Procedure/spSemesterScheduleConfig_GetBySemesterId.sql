CREATE PROCEDURE [dbo].[spSemesterScheduleConfig_GetBySemesterId]
	@SemesterId int
	
AS
BEGIN
	SELECT [ConfigId],[SemesterId],[TotalClasses],[BreakAfterPeriod] 
	FROM [dbo].[SemesterScheduleConfig]
	WHERE SemesterId = @SemesterId
END
