CREATE TABLE [dbo].[SemesterInstance]
(
	[SemesterInstanceId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[SemesterId] INT NOT NULL,
	[StartDate] DATE NOT NULL,
	[SemesterStatus] NVARCHAR(20) NOT NULL DEFAULT 'active' CHECK(SemesterStatus IN('active','completed')) ,
	[EndDate] DATE NULL DEFAULT NULL, 
	CONSTRAINT[CK_SemesterInstance_Dates] Check(([SemesterStatus] = 'completed' AND [EndDate]>[StartDate])
	OR
	([SemesterStatus] = 'active' AND [EndDate] IS NULL )
	),
    CONSTRAINT [FK_SemesterInstance_Semester] FOREIGN KEY ([SemesterId]) REFERENCES dbo.[Semester]([SemesterId]),

	
)
