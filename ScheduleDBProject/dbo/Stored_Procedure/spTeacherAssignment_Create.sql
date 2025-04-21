CREATE PROCEDURE [dbo].[spTeacherAssignment_Create]
    @TeacherId INT,
    @SubjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if teacher exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Teacher] WHERE TeacherId = @TeacherId)
        BEGIN
            RAISERROR('Teacher does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if subject exists
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Subject] WHERE SubjectId = @SubjectId)
        BEGIN
            RAISERROR('Subject does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check if subject is already assigned to another teacher
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[TeacherAssignment] 
            WHERE SubjectId = @SubjectId
        )
        BEGIN
            RAISERROR('Subject is already assigned to another teacher.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check teacher's current assignment count
        DECLARE @CurrentAssignments INT;
        SELECT @CurrentAssignments = COUNT(*) 
        FROM [dbo].[TeacherAssignment] 
        WHERE TeacherId = @TeacherId;

        DECLARE @MaxClasses INT;
        SELECT @MaxClasses = NumberOfClasses 
        FROM [dbo].[Teacher] 
        WHERE TeacherId = @TeacherId;

        IF @CurrentAssignments >= @MaxClasses
        BEGIN
            RAISERROR('Teacher has reached maximum assigned classes (%d).', 16, 1, @MaxClasses);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Assign teacher to subject
        INSERT INTO [dbo].[TeacherAssignment] (TeacherId, SubjectId)
        VALUES (@TeacherId, @SubjectId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;