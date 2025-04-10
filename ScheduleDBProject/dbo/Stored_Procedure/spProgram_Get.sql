CREATE PROCEDURE [dbo].[spProgram_Get]
	@ProgramId INT
AS
BEGIN
	SELECT [ProgramId],[ProgramName]
	FROM dbo.Program
	WHERE ProgramId = @ProgramId;
END
