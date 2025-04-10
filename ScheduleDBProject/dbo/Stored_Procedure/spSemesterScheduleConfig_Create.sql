CREATE PROCEDURE [dbo].[spSemesterScheduleConfig_Create]
	@SemesterId INT,
	@TotalClasses INT,
	@BreakAfterPeriod INT =NULL
AS
BEGIN
     SET NOCOUNT ON;
	 BEGIN TRY 
			BEGIN TRANSACTION

			--CHECK IF SEMESTER EXIST
			IF NOT EXISTS (
				SELECT 1
				FROM [dbo].[Semester]
				WHERE SemesterId = @SemesterId
			)
			BEGIN
				RAISERROR('Invalid SemesterId: Semester doesnot exist',16,1);
				ROLLBACK TRANSACTION;
				RETURN;
			END

			--CHECK IF CONFIG ALRADY EXIST
			IF EXISTS (
				SELECT 1
				FROM [dbo].[SemesterScheduleConfig]
				WHERE SemesterId =@SemesterId
			)
			BEGIN
				RAISERROR('Semester already has a configuration',16,1)
				ROLLBACK TRANSACTION;
				RETURN
			END

			--Prevent config creation for active semesters
			IF EXISTS (
            SELECT 1
            FROM [dbo].[SemesterInstance]
            WHERE SemesterId = @SemesterId
              AND SemesterStatus = 'active'
			)
			BEGIN
				RAISERROR('Cannot create config: Semester is currently active.', 16, 1);
				ROLLBACK TRANSACTION;
				RETURN;
			END

			--Validate TotalClasses constraint
			IF @TotalClasses NOT BETWEEN 1 AND 6
			BEGIN
				RAISERROR('TotalClasses must be between 1 and 6.', 16, 1);
				ROLLBACK TRANSACTION;
				RETURN;
			END

			--Validate BreakAfterPeriod if provided
			IF @BreakAfterPeriod IS NOT NULL 
            AND (@BreakAfterPeriod < 1 OR @BreakAfterPeriod >= @TotalClasses)
			BEGIN
				RAISERROR('BreakAfterPeriod must be >=1 and < TotalClasses.', 16, 1);
				ROLLBACK TRANSACTION;
				RETURN;
			END


			--INSERT NEW CONFIG
			INSERT INTO [dbo].[SemesterScheduleConfig] (
            SemesterId,
            TotalClasses,
            BreakAfterPeriod
			)
			VALUES (
				@SemesterId,
				@TotalClasses,
				@BreakAfterPeriod
			);

			--GENERATE TIME SLOTS 
			DECLARE @NewConfigId INT = SCOPE_IDENTITY();--gets the latest created ConfigId
			EXEC [dbo].[GenerateTimeSlotsForConfig] @ConfigId = @NewConfigId;
			COMMIT TRANSACTION;
	 END TRY
	 BEGIN CATCH 
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
		;THROW;
	END CATCH
END
