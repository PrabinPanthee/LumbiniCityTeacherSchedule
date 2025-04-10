CREATE PROCEDURE [dbo].[spSemesterScheduleConfig_Update]
	@ConfigId INT,
	@TotalClasses INT,
	@BreakAfterPeriod INT

	
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check 1: Verify config exists
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[SemesterScheduleConfig]
            WHERE ConfigId = @ConfigId
        )
        BEGIN
            RAISERROR('Invalid ConfigId: Configuration does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check 2: Prevent updates for active semesters
        IF EXISTS (
            SELECT 1
            FROM [dbo].[SemesterScheduleConfig] ssc
            JOIN [dbo].[Semester] s ON ssc.SemesterId = s.SemesterId
            JOIN [dbo].[SemesterInstance] si ON s.SemesterId = si.SemesterId
            WHERE ssc.ConfigId = @ConfigId
              AND si.SemesterStatus = 'active'
        )
        BEGIN
            RAISERROR('Cannot update config: Linked to active semester.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check 3: Validate new TotalClasses
        IF @TotalClasses NOT BETWEEN 1 AND 6
        BEGIN
            RAISERROR('TotalClasses must be between 1 and 6.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check 4: Validate BreakAfterPeriod
        IF @BreakAfterPeriod IS NOT NULL 
            AND (@BreakAfterPeriod < 1 OR @BreakAfterPeriod >= @TotalClasses)
        BEGIN
            RAISERROR('BreakAfterPeriod must be >=1 and < TotalClasses.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update config
        UPDATE [dbo].[SemesterScheduleConfig]
        SET 
            TotalClasses = @TotalClasses,
            BreakAfterPeriod = @BreakAfterPeriod
        WHERE ConfigId = @ConfigId;

        -- Regenerate timeslots
        EXEC [dbo].[GenerateTimeSlotsForConfig] @ConfigId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
