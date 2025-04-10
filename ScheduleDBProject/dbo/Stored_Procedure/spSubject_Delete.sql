CREATE PROCEDURE [dbo].[spSubject_Delete]
    @SubjectId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if subject exists
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[Subject] 
            WHERE SubjectId = @SubjectId
        )
        BEGIN
            RAISERROR('Subject does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Check for active semester instance
        IF EXISTS (
            SELECT 1
            FROM [dbo].[Subject] s
            JOIN [dbo].[SemesterInstance] si ON s.SemesterId = si.SemesterId
            WHERE s.SubjectId = @SubjectId
              AND si.SemesterStatus = 'active'
        )
        BEGIN
            RAISERROR('Cannot delete subject from active semester.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Delete subject (CASCADE will handle TeacherAssignment)
        DELETE FROM [dbo].[Subject] 
        WHERE SubjectId = @SubjectId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
