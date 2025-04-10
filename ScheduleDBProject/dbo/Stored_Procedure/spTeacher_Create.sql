CREATE PROCEDURE [dbo].[spTeacher_Create]
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@NumberOfClasses TINYINT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY
		BEGIN TRANSACTION;

		--Validate Number of Classes 
		IF (@NumberOfClasses  NOT BETWEEN 1 AND 6)
		BEGIN
			RAISERROR('Invalid class Number: Must be between 1 and 6',16,1)
			ROLLBACK TRANSACTION
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

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	IF @@TRANCOUNT >0 ROLLBACK TRANSACTION;
	END CATCH
END


