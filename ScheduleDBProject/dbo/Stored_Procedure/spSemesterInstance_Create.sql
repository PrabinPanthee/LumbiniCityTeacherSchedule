CREATE PROCEDURE [dbo].[spSemesterInstance_Create]
	@SemesterId INT,
	@StartDate DATE
	

AS
BEGIN
	SET NOCOUNT ON;
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
	
				--Validate if Config exists for this semester
				IF NOT EXISTS(
						SELECT 1
						FROM [dbo].[SemesterScheduleConfig]
						WHERE SemesterId = @SemesterId
						)
				BEGIN
					RAISERROR('Cannot activate semester: Configuration doesnot exist.',16,1);
					ROLLBACK TRANSACTION
					RETURN;
				END

				--Validate duplicate active semester 
				IF EXISTS(
						SELECT 1
						FROM [dbo].[SemesterInstance]
						WHERE [SemesterId] = @SemesterId
						AND [SemesterStatus] = 'active' 
						)
				BEGIN
					RAISERROR('Semester is already Active',16,1)
					ROLLBACK TRANSACTION
					RETURN;
				END

				--create semester instance 
				INSERT INTO [dbo].[SemesterInstance]
					([SemesterId],[StartDate],[SemesterStatus],[EndDate]) 
					VALUES(@SemesterId,@StartDate,'active',NULL)

			COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		;THROW;
	END CATCH

END
