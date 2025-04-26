CREATE PROCEDURE [dbo].[spProgram_GetByProgramName]
	@ProgramName NVARCHAR(100)
As 
BEGIN
   if exists
	(SELECT 1
	FROM [dbo].[Program]
	WHERE [ProgramName] = @ProgramName)
	SELECT 1
	ELSE
	SELECT 0
END
