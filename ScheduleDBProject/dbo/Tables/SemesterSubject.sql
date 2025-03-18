CREATE TABLE [dbo].[SemesterSubject]
(
	[SemesterId] INT NOT NULL,
	[SubjectId] INT NOT NULL, 
	PRIMARY KEY (SemesterId,SubjectId),
    CONSTRAINT [FK_SemesterSubject_Semester] FOREIGN KEY ([SemesterId]) REFERENCES dbo.[Semester]([SemesterId]), 
    CONSTRAINT [FK_SemesterSubject_Subject] FOREIGN KEY ([SubjectId]) REFERENCES dbo.[Subject]([SubjectId])

)
