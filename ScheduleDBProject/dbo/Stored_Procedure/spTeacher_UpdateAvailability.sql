CREATE PROCEDURE [dbo].[spTeacher_UpdateAvailability]
    @TeacherAvailabilityId INT,
    @StartTime TIME,
    @EndTime TIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate time range
        IF @EndTime <= @StartTime
        BEGIN
            RAISERROR('EndTime must be after StartTime.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update availability
        UPDATE [dbo].[TeacherAvailability]
        SET 
            StartTime = @StartTime,
            EndTime = @EndTime
        WHERE TeacherAvailabilityId = @TeacherAvailabilityId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;