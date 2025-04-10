CREATE PROCEDURE [dbo].[spTeacher_Get]
	@TeacherId INT
	
AS
BEGIN
	SELECT [TeacherId],[FirstName],[LastName],[NumberOfClasses]
	FROM [dbo].[Teacher]
	WHERE [TeacherId] = @TeacherId
END
