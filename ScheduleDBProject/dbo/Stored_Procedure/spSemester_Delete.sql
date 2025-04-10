CREATE PROCEDURE [dbo].[spSemester_Delete]
	@SemesterId INT 
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
			BEGIN TRANSACTION;
			--CEHCK IF ANY ACTIVE SEMESTER INSTANCES 
			IF EXISTS (
			SELECT 1 FROM [dbo].[SemesterInstance]
			WHERE [SemesterId] = @SemesterId
			AND [SemesterStatus] = 'active'
			)
			BEGIN
				RAISERROR('Cannot delete semester with active instances',16,1);
				Return;
			END

			--Deleting downstream dependencies

			--DELETE FROM CLASSSCHEDULE 
			DELETE FROM [dbo].[ClassSchedule] 
			WHERE [SemesterInstanceId] IN (
			SELECT SemesterInstanceId 
			FROM [dbo].[SemesterInstance] 
			WHERE [SemesterId] = @SemesterId
			);

			--DELETE FROM SEMESTER INSTANCE 
			DELETE FROM [dbo].[SemesterInstance]
			WHERE [SemesterId] = @SemesterId;

			--DELETE RECORDS FROM TEACHER ASSIGNMENT TABLE 
			DELETE FROM [dbo].[TeacherAssignment] 
			WHERE [SubjectId] IN(
			SELECT SubjectId 
			FROM [dbo].[Subject]
			WHERE [SemesterId] =@SemesterId
			);

			--DELETING SUBJECT RECORDS RELATED TO SEMESTER
			DELETE FROM [dbo].[Subject] 
			WHERE [SemesterId] =@SemesterId;

			--DELTEING CONFIG RELATED TO SEMESTER (CASCADES TO TIME SLOT)
			DELETE FROM [dbo].[SemesterScheduleConfig]
            WHERE [SemesterId] = @SemesterId;
			
			--DELETING SEMESTER FINALLY 
			DELETE FROM [dbo].[Semester]
			WHERE [SemesterId] = @SemesterId	
			
			COMMIT TRANSACTION;
			
	END TRY
	BEGIN CATCH
			ROLLBACK TRANSACTION
			;THROW;
	END CATCH
END 
     