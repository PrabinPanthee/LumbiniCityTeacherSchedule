CREATE PROCEDURE [dbo].[spTeacher_Update]
    @TeacherId INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @NumberOfClasses TINYINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate teacher exists
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[Teacher] 
            WHERE TeacherId = @TeacherId
        )
        BEGIN
            RAISERROR('Teacher does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Validate class limits
        IF @NumberOfClasses NOT BETWEEN 1 AND 6
        BEGIN
            RAISERROR('NumberOfClasses must be between 1 and 6.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if reducing available classes below current assignments
        DECLARE @CurrentAssignedClasses INT;
        SELECT @CurrentAssignedClasses = COUNT(DISTINCT cs.ScheduleId)
        FROM [dbo].[ClassSchedule] cs
        WHERE cs.TeacherId = @TeacherId;

        IF @NumberOfClasses < @CurrentAssignedClasses
        BEGIN
            RAISERROR('Cannot reduce classes: Teacher has %d active assignments.', 16, 1, @CurrentAssignedClasses);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update teacher
        UPDATE [dbo].[Teacher]
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            NumberOfClasses = @NumberOfClasses
        WHERE TeacherId = @TeacherId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;