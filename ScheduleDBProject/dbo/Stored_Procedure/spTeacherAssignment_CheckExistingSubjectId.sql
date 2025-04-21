CREATE PROCEDURE [dbo].[spTeacherAssignment_CheckExistingSubjectId]
	@SubjectId INT
AS
BEGIN
	 IF EXISTS (
            SELECT 1 
            FROM [dbo].[TeacherAssignment] 
            WHERE SubjectId = @SubjectId
        )
        SELECT 1;
        ELSE
        SELECT 0;
END
