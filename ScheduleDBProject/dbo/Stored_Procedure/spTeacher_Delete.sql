CREATE PROCEDURE [dbo].[spTeacher_Delete]
	@TeacherId INT
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION
		--VALIDATE Teacher id--
		IF NOT EXISTS(
				SELECT 1 
				FROM dbo.Teacher 
				WHERE TeacherId =@TeacherId
		)
		BEGIN
		RAISERROR('Invalid Teacher Id',16,1)
		ROLLBACK TRANSACTION
		RETURN
		END 

		--Check if teacher is linked to active semesters 
		IF EXISTS (
				SELECT 1 
				FROM [dbo].[TeacherAssignment] ta
				JOIN [dbo].[Subject] s ON ta.SubjectId = s.SubjectId
				JOIN [dbo].[SemesterInstance] st ON s.SemesterId = st.SemesterId
				WHERE ta.TeacherId =@TeacherId
				AND st.SemesterStatus = 'active'

				UNION

				SELECT 1
				FROM [dbo].[ClassSchedule] cs
				JOIN [dbo].[SemesterInstance] st ON cs.SemesterInstanceId = st.SemesterInstanceId
				WHERE cs.TeacherId = @TeacherId
				AND st.SemesterStatus = 'active'

		)
		BEGIN
			RAISERROR('Cannot delete teacher: Assigned to active se',16,1);
			ROLLBACK TRANSACTION
			RETURN
		END

		 -- Delete teacher (cascades to assignments, availability)
		DELETE FROM [dbo].[Teacher] 
        WHERE TeacherId = @TeacherId;

		COMMIT TRANSACTION
	END TRY 
	BEGIN CATCH 
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	END CATCH
END