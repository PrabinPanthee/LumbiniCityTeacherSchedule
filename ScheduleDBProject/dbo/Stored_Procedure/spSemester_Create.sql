CREATE PROCEDURE [dbo].[spSemester_Create]
	@ProgramId INT,
	@SemesterNumber INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
			
	        --validate the program exist or not 
	        IF NOT EXISTS(
			SELECT
			1 FROM [dbo].[Program]
			WHERE [ProgramId] = @ProgramId
			)
			BEGIN
			    RAISERROR('Invalid Program Id!',16,1);
				RETURN;
			END

			--Check duplicate semester number 
			IF EXISTS(
			SELECT 1 FROM [dbo].[Semester]
			WHERE [ProgramId]=@ProgramId
			AND [SemesterNumber] = @SemesterNumber
			)
			BEGIN
				RAISERROR('Semester  Already Exists for this Program!',16,1);
				RETURN;
		    END

			--validate semester number range
			IF @SemesterNumber NOT BETWEEN 1 AND 8
			BEGIN
			RAISERROR('Semester Number must be between 1 - 8',16,1);
			END

			--AFTER VALIDATION ARE PASSED THE CREATE SEMESTER
			INSERT INTO [dbo].[Semester] ([ProgramId],[SemesterNumber]) 
			VALUES (@ProgramId,@SemesterNumber);

			--RETURN NEW CREATED SEMESTER ID FOR FOR SERVER IF THEY NEED IT 

			 
			DECLARE @NewSemesterId INT;
			
			--GETS LATEST SEMESTER ID
			SET @NewSemesterId = SCOPE_IDENTITY();

			--Ensure a valid ID was generated
			IF @NewSemesterId IS NULL
			BEGIN
				RAISERROR('Failed to create Semester. Please try again!', 16, 1);
				RETURN;
			END

			-- Return the new Semester ID
			SELECT @NewSemesterId AS NewSemesterId;

     END TRY
	 BEGIN CATCH
		;THROW;
	 END CATCH
END;

---sumarry-----
--FOR CREATING THE SEMESTER 
--TRY BLOCK CHECKS VALIDATION FOR PROGRAM IF IT EXISTS OR NOT
--CHECKS FOR DUPLICATE SEMESTER NUMBER (LIKE BCA CANNOT HAVE 2 FIRST SEM
--CHECKS THE RANGE OF THE SEMESTER (LIKE SEMESTER NUMBER CANNOT BE 0 AND GREATER THAN 8)
--CREATES NEW SEM IF VALIDATION ARE PASSED 
--RETURNS NEWLY CREATED SMESTER ID IF THEY NEED TO BE USED IN SERVER OR FRONT END IF not NUll then 

			

