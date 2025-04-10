CREATE TABLE [dbo].[Semester]
(
	[SemesterId] INT NOT NULL PRIMARY KEY Identity(1,1),
	[ProgramId] INT NOT NULL,
	[SemesterNumber] INT NOT NULL, 
    CONSTRAINT [FK_Semester_ToProgram] FOREIGN KEY ([ProgramId]) REFERENCES[dbo].[Program]([ProgramId]),
	CONSTRAINT [UC_Semester_ProgramSemester] UNIQUE ([ProgramId],[SemesterNumber]),
	CONSTRAINT [CK_Semester_Number] CHECK ([SemesterNumber] BETWEEN 1 AND 8)
	
)
