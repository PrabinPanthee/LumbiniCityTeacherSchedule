CREATE PROCEDURE [dbo].[spSemesterScheduleConfig_Get]
	@ConfigId int
	
AS
BEGIN
	SELECT [ConfigId],[SemesterId],[TotalClasses],[BreakAfterPeriod] 
	FROM [dbo].[SemesterScheduleConfig]
	WHERE ConfigId = @ConfigId
END
