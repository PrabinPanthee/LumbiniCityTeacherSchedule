CREATE TABLE [dbo].[Subject]
(
	[SubjectId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SemesterId] INT NOT NULL,
	[SubjectName] NVARCHAR(100) NOT NULL,
	[SubjectCode] NVARCHAR(50) NOT NULL UNIQUE,
	CONSTRAINT [FK_Subject_Semester] FOREIGN KEY ([SemesterId]) REFERENCES [dbo].[Semester]([SemesterId])

)
