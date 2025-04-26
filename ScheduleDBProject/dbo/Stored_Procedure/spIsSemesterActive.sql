CREATE PROCEDURE [dbo].[spIsSemesterActive]
	@SemesterId INT
	
AS
BEGIN
	IF EXISTS (
			SELECT 1 FROM [dbo].[SemesterInstance]
			WHERE [SemesterId] = @SemesterId
			AND [SemesterStatus] = 'active'
			)
			SELECT 1
			ELSE
			SELECT 0
END
