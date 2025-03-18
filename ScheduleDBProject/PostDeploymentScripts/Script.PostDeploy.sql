/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists (select 1 from dbo.[Program])
Begin
   Insert Into dbo.[Program] (ProgramName) Values ('BCA'),('CSIT'),('BIM');
End


IF NOT EXISTS (Select 1 from dbo.[Semester]) 
begin
Insert into dbo.[Semester] (ProgramId,SemesterNumber) Values ('1','2'),('1','1'),('1','3'),('1','4')
end

IF NOT EXISTS(Select 1 from dbo.[SemesterScheduleConfig])
begin
Insert into dbo.[SemesterScheduleConfig] (SemesterId,TotalClasses,BreakAfterPeriod) Values ('5','5','2'),('4','5','3')
end
