CREATE PROCEDURE [dbo].[spTeacher_UpdateAvailability]
    @AvailabilityId INT,
    @NewStartTime TIME,
    @NewEndTime TIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate time range
        IF @NewEndTime <= @NewStartTime
        BEGIN
            RAISERROR('EndTime must be after StartTime.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update availability
        UPDATE [dbo].[TeacherAvailability]
        SET 
            StartTime = @NewStartTime,
            EndTime = @NewEndTime
        WHERE TeacherAvailabilityId = @AvailabilityId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;