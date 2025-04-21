CREATE PROCEDURE [dbo].[spTeacherAssignment_CountTeacherAssignment]
	@TeacherId INT
	
AS
BEGIN
	
	DECLARE @CurrentAssignments INT;
    SELECT @CurrentAssignments = COUNT(*) 
    FROM [dbo].[TeacherAssignment] 
    WHERE TeacherId = @TeacherId;

	SELECT @CurrentAssignments;

END
