CREATE PROCEDURE [dbo].[spSubject_Update]
    @SubjectId INT,
    @SubjectName NVARCHAR(100),
    @SubjectCode NVARCHAR(50)
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
            RAISERROR('Cannot update subject in active semester.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Validate unique SubjectCode (excluding current subject)
        IF EXISTS (
            SELECT 1 
            FROM [dbo].[Subject] 
            WHERE SubjectCode = @SubjectCode
              AND SubjectId <> @SubjectId
        )
        BEGIN
            RAISERROR('Subject Code must be unique across all semesters.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Update subject
        UPDATE [dbo].[Subject]
        SET 
            SubjectName = @SubjectName,
            SubjectCode = @SubjectCode
        WHERE SubjectId = @SubjectId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;