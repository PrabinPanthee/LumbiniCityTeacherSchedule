CREATE PROCEDURE [dbo].[spSubject_Get]
	@SubjectId INT
	
AS
BEGIN
	SELECT [SubjectId],[SemesterId],[SubjectName],[SubjectCode]
	FROM [dbo].[Subject]
	WHERE [SubjectId] = @SubjectId
END
