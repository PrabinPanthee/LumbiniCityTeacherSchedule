CREATE PROCEDURE [dbo].[spTeacherAssignment_Update]
	@TeacherAssignmentId INT,
	@TeacherId INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION
		--Check if TeacherAssignment for given Id exist
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

		IF NOT EXISTS
		(
			SELECT 1
			FROM [dbo].[Teacher]
			WHERE [TeacherId] = @TeacherId
		)
		BEGIN
			RAISERROR('Teacher does not exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
		END

		--GET SUBJECT ID
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
			RAISERROR('Cannot Update assignment linked to active sem', 16, 1);
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

		UPDATE [dbo].[TeacherAssignment]
		SET [TeacherId] = @TeacherId
		WHERE [TeacherAssignmentId] = @TeacherAssignmentId

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		 IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		;THROW;
	END CATCH
		
END
