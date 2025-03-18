CREATE TABLE [dbo].[SemesterScheduleConfig]
(
	[ConfigId] INT NOT NULL PRIMARY KEY Identity(1,1),
	[SemesterId] INT NOT NULL UNIQUE,
	[TotalClasses] INT NOT NULL,
	[BreakAfterPeriod] INT NULL, 
    CONSTRAINT [FK_SemesterScheduleConfig_ToSemester] FOREIGN KEY ([SemesterId]) REFERENCES dbo.[Semester]([SemesterId]), 
    CONSTRAINT [CK_SemesterScheduleConfig_TotalClasses] CHECK ([TotalClasses] BETWEEN 1 AND 6),
	CONSTRAINT [CK_SemesterScheduleConfig_BreakPeriod] 
        CHECK (
            [BreakAfterPeriod] IS NULL 
            OR (
                [BreakAfterPeriod] >= 1 
                AND [BreakAfterPeriod] < [TotalClasses] 
            )
        )
	


)
