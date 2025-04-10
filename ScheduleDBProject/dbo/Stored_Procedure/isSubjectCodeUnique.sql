CREATE PROCEDURE [dbo].[isSubjectCodeUnique]
	@SubjectCode NVARCHAR(50)
AS
BEGIN
	IF EXISTS (
				SELECT 1
				FROM [dbo].[Subject]
				WHERE [SubjectCode] = @SubjectCode
				)
		SELECT 1;
		ELSE
		SELECT 0;
END
