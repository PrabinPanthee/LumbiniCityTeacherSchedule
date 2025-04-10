CREATE PROCEDURE [dbo].[spIsConfigLinkedToActiveSemester]
    @ConfigId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM [dbo].[SemesterScheduleConfig] ssc
        JOIN [dbo].[Semester] s ON ssc.SemesterId = s.SemesterId
        JOIN [dbo].[SemesterInstance] si ON s.SemesterId = si.SemesterId
        WHERE ssc.ConfigId = @ConfigId
          AND si.SemesterStatus = 'active'
    )
        SELECT 1;
    ELSE
        SELECT 0;
END