CREATE TABLE [dbo].[TeacherSubject]
(
	[TeacherId] INT NOT NULL ,
	[SubjectId] INT NOT NULL,
	PRIMARY KEY ([TeacherId],[SubjectId]),

	CONSTRAINT [FK_TeacherSubject_Teacher] 
        FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher]([TeacherId]),
    
    CONSTRAINT [FK_TeacherSubject_Subject] 
        FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subject]([SubjectId])

)
