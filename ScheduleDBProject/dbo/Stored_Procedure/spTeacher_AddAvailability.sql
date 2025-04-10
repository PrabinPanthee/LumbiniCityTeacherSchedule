CREATE PROCEDURE [dbo].[spTeacher_AddAvailability]
    @TeacherId INT,
    @StartTime TIME,
    @EndTime TIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate teacher exists
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[Teacher] 
            WHERE TeacherId = @TeacherId
        )
        BEGIN
            RAISERROR('Invalid TeacherId.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Validate time range
        IF @EndTime <= @StartTime
        BEGIN
            RAISERROR('EndTime must be after StartTime.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Insert availability
        INSERT INTO [dbo].[TeacherAvailability] (
            TeacherId,
            StartTime,
            EndTime
        )
        VALUES (
            @TeacherId,
            @StartTime,
            @EndTime
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
