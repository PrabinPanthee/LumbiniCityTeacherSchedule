CREATE PROCEDURE [dbo].[spSemesterInstance_Update]
	@SemesterInstanceId INT,
	@EndDate DATE
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate active instance
        IF NOT EXISTS (
            SELECT 1
            FROM [dbo].[SemesterInstance]
            WHERE SemesterInstanceId = @SemesterInstanceId
              AND SemesterStatus = 'active'
        )
        BEGIN
            RAISERROR('Cannot complete: Instance not found or already completed.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Get StartDate for comparison
        DECLARE @StartDate DATE;
        SELECT @StartDate = StartDate
        FROM [dbo].[SemesterInstance]
        WHERE SemesterInstanceId = @SemesterInstanceId;

        -- Validate EndDate
        IF @EndDate <= @StartDate
        BEGIN
            RAISERROR('EndDate must be after StartDate.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        --clean up class schedule 
        DELETE FROM [dbo].[ClassSchedule]
        WHERE SemesterInstanceId = @SemesterInstanceId;

        -- Update to completed
        UPDATE [dbo].[SemesterInstance]
        SET 
            SemesterStatus = 'completed',
            EndDate = @EndDate
        WHERE SemesterInstanceId = @SemesterInstanceId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END