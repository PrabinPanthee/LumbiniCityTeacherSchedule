CREATE TABLE [dbo].[TeacherAssignment]
(
	[TeacherAssignmentId] INT NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[TeacherId] INT NOT NULL ,
	[SubjectId] INT NOT NULL UNIQUE,
	   CONSTRAINT [FK_TeacherAssignment_Teacher_Cascade] FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher]([TeacherId]) ON DELETE CASCADE,
	   CONSTRAINT [FK_TeacherAssignment_Subject_Cascade] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject]([SubjectId]) ON DELETE CASCADE
)
