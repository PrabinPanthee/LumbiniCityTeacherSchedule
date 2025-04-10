CREATE PROCEDURE [dbo].[spProgram_Delete]
	@ProgramId INT
	
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY
		BEGIN TRANSACTION;

		--Validate ProgramId
		IF NOT EXISTS(
				SELECT 1
				FROM [dbo].[Program]
				WHERE [ProgramId] = @ProgramId
		)
		BEGIN 
			RAISERROR('Invalid ProgramId : Doesnot exists', 16,1)
			ROLLBACK TRANSACTION
			RETURN
		END

		  -- Check for ACTIVE semesters
        IF EXISTS (
            SELECT 1
            FROM [dbo].[Semester] s
            JOIN [dbo].[SemesterInstance] si 
                ON s.SemesterId = si.SemesterId
            WHERE s.ProgramId = @ProgramId
                AND si.SemesterStatus = 'active'
        )
        BEGIN
            RAISERROR('Cannot delete program: Active semesters exist.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
		--CHECK IF SEMESTER EXISTS 
		IF EXISTS(
		   SELECT 1 FROM [dbo].[Semester]
		   WHERE [ProgramId] = @ProgramId
		)
	    BEGIN
		    RAISERROR('Cannot delete program. Delete all semester first!',16,1);
			RETURN
	    END

		
		--ELSE DELETE PROGRAM IF NO DEPENDENCIES
		
		DELETE FROM [dbo].[Program] WHERE [ProgramId] = @ProgramId;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
	      ROLLBACK TRANSACTION;
		  ;THROW;
    END CATCH
END;

---SUMMARY---
--HERE IT DELETS OR REMOVES THE PROGRAM RECORD 
--IN TRY BLOCK  TRANSACTION IS CREATED THEN IT CHECKS FOR ANY DEPENDENCIES OF THE PROGRAM IF EXIST THEN ERROR IS THOWN AND TRANSACTION IS ROLLED BACK
--ELSE PROGRAM IS REMOVED AND TRANSCTION I COMIITED 