CREATE PROCEDURE [dbo].[spIsSubjectUniqueForUpdate]
	@SubjectId INT,
	@SubjectCode NVARCHAR(50)
	
AS
BEGIN
	  IF EXISTS (
            SELECT 1 
            FROM [dbo].[Subject] 
            WHERE SubjectCode = @SubjectCode
              AND SubjectId <> @SubjectId
        )
        SELECT 1;
        ELSE
        SELECT 0;
END