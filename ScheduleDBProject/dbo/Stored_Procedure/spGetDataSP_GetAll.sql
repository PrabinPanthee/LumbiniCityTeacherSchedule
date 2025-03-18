CREATE PROCEDURE [dbo].[spGetDataSP_GetAll]
	
AS
begin
	SELECT [p].[ProgramId], [p].[ProgramName] , [s].[SemesterId], [s].[SemesterNumber]
	from dbo.Semester s
	left join dbo.Program p on p.ProgramId = s.ProgramId 
End


