CREATE PROCEDURE [dbo].[spSubject_GetAllBySemesterId]
	@SemesterId INT
	
AS
BEGIN
	SELECT [SubjectId],[SemesterId],[SubjectName],[SubjectCode]
	FROM [dbo].[Subject]
	WHERE [SemesterId] = @SemesterId
END
