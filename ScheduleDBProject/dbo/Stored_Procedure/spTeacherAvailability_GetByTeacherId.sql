CREATE PROCEDURE [dbo].[spTeacherAvailability_GetByTeacherId]
	@TeacherId INT
AS
BEGIN
	SELECT * 
	FROM [dbo].[TeacherAvailability]
	WHERE [TeacherId] = @TeacherId
END
