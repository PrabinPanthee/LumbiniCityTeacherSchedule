CREATE PROCEDURE [dbo].[spProgram_GetAll]
	
AS
BEGIN
	SELECT [ProgramId],[ProgramName]
	FROM dbo.Program;
END

