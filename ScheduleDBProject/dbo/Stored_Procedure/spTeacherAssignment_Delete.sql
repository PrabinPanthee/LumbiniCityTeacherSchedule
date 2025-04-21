CREATE PROCEDURE [dbo].[spTeacherAssignment_Delete]
	@TeacherAssignmentId INT
	
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRANSACTION
		--Check if Id is valid
		
		IF NOT EXISTS
		(
			SELECT 1 
			FROM [dbo].[TeacherAssignment] 
			WHERE [TeacherAssignmentId] = @TeacherAssignmentId
		)
		BEGIN
			RAISERROR('Assignment does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
		END

		--CHECK IF LINKED TO ACTIVE SEM
		DECLARE @SubjectId INT
		SELECT @SubjectId = [SubjectId]
		FROM [dbo].[TeacherAssignment]
		WHERE [TeacherAssignmentId] = @TeacherAssignmentId

		IF EXISTS
		(
			SELECT 1
			FROM [dbo].[Subject] s
			JOIN [dbo].[SemesterInstance] si ON s.SemesterId = si.SemesterId
			WHERE s.SubjectId = @SubjectId AND si.SemesterStatus = 'active'
		)
		BEGIN
			RAISERROR('Cannot Delete assignment linked to active sem', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
		END

		--At last delete
		DELETE FROM [dbo].[TeacherAssignment]
		WHERE [TeacherAssignmentId] = @TeacherAssignmentId

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
		;THROW;
	END CATCH
END
