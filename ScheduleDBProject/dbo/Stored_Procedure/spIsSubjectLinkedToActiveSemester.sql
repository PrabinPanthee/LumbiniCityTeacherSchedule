CREATE PROCEDURE [dbo].[spIsSubjectLinkedToActiveSemester]
	@SubjectId INT
	
AS
BEGIN
	IF EXISTS (
            SELECT 1
            FROM [dbo].[Subject] s
            JOIN [dbo].[SemesterInstance] si ON s.SemesterId = si.SemesterId
            WHERE s.SubjectId = @SubjectId
              AND si.SemesterStatus = 'active'
        )
        SELECT 1
        ELSE
        SELECT 0
END
