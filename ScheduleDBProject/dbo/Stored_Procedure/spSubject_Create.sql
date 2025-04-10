CREATE PROCEDURE [dbo].[spSubject_Create]
	@SemesterId INT,
	@SubjectName NVARCHAR(100),
	@SubjectCode NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
			BEGIN TRANSACTION
				--Validate semester id
				IF NOT EXISTS (
						SELECT 1 
						FROM [dbo].[Semester]
						WHERE SemesterId = @SemesterId 
				)
				BEGIN
					RAISERROR('Invalid Semester Id',16,1)
					ROLLBACK TRANSACTION
					RETURN;
				END

				--Check for active sem 
				IF EXISTS (
					SELECT 1
					FROM [dbo].[SemesterInstance]
					WHERE SemesterId = @SemesterId
					  AND SemesterStatus = 'active'
				)
				BEGIN
					RAISERROR('Cannot add subjects to active semesters.', 16, 1);
					ROLLBACK TRANSACTION;
					RETURN;
				END

				--Validate duplicate Subject Code 
				IF EXISTS (
						SELECT 1
						FROM [dbo].[Subject]
						WHERE [SubjectCode] = @SubjectCode
				)
				BEGIN
					RAISERROR('Subject Code must be unique across all semesters.',16,1)
					ROLLBACK TRANSACTION
					RETURN
				END

				 -- Insert subject
				INSERT INTO [dbo].[Subject] (
					SemesterId,
					SubjectName,
					SubjectCode
				)
				VALUES (
					@SemesterId,
					@SubjectName,
					@SubjectCode
				);
			COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		;THROW;
	END CATCH
END
