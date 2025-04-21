CREATE PROCEDURE [dbo].[spTeacherWithAvailability_GetById]
	@TeacherId INT
	
AS
BEGIN
	SELECT [t].[TeacherId], [t].[FirstName], [t].[LastName], [t].[NumberOfClasses], [a].[StartTime], [a].[EndTime]
	FROM [dbo].[Teacher] t
	LEFT JOIN [dbo].[TeacherAvailability] a
	ON t.TeacherId = a.TeacherId
	WHERE t.[TeacherId] = @TeacherId
END
