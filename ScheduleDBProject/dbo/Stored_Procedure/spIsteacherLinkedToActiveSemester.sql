CREATE PROCEDURE [dbo].[spIsteacherLinkedToActiveSemester]
	@TeacherId int
	
AS
BEGIN
	SET NOCOUNT ON;
		IF EXISTS (
				SELECT 1 
				FROM [dbo].[TeacherAssignment] ta
				JOIN [dbo].[Subject] s ON ta.SubjectId = s.SubjectId
				JOIN [dbo].[SemesterInstance] st ON s.SemesterId = st.SemesterId
				WHERE ta.TeacherId =@TeacherId
				AND st.SemesterStatus = 'active'

				UNION

				SELECT 1
				FROM [dbo].[ClassSchedule] cs
				JOIN [dbo].[SemesterInstance] st ON cs.SemesterInstanceId = st.SemesterInstanceId
				WHERE cs.TeacherId = @TeacherId
				AND st.SemesterStatus = 'active'

		)
		SELECT 1;
		ELSE
		SELECT 0;
END
