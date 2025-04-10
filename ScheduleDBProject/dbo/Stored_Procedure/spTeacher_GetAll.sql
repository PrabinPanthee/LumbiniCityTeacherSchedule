CREATE PROCEDURE [dbo].[spTeacher_GetAll]
	
AS
BEGIN
	SELECT [TeacherId],[FirstName],[LastName],[NumberOfClasses]
	FROM [dbo].[Teacher]
END
