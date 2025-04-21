    CREATE PROCEDURE [dbo].[spTeacherWithAvailability_Create]
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@NumberOfClasses TINYINT,
	@StartTime Time,
	@EndTime TIME
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRANSACTION;

		--Validate Number of Classes 
		IF (@NumberOfClasses  NOT BETWEEN 1 AND 6)
		BEGIN
			RAISERROR('Invalid class Number: Must be between 1 and 6',16,1)
			RETURN
		END 
		 -- Insert teacher
        INSERT INTO [dbo].[Teacher] (
            FirstName,
            LastName,
            NumberOfClasses
        )
        VALUES (
            @FirstName,
            @LastName,
            @NumberOfClasses
        );
		
        DECLARE @TeacherId INT = SCOPE_IDENTITY();

		-- Validate teacher exists
        IF NOT EXISTS (
            SELECT 1 
            FROM [dbo].[Teacher] 
            WHERE TeacherId = @TeacherId
        )
        BEGIN
            RAISERROR('Invalid TeacherId.', 16, 1);
            
            RETURN;
        END

		IF @EndTime <= @StartTime
        BEGIN
            RAISERROR('EndTime must be after StartTime.', 16, 1);
            RETURN;
        END
         INSERT INTO TeacherAvailability (TeacherId, StartTime, EndTime)
        VALUES (@TeacherId, @StartTime, @EndTime);

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT >0 ROLLBACK TRANSACTION;
	END CATCH
END


