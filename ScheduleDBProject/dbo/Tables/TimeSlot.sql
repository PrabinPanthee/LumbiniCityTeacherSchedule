CREATE TABLE [dbo].[TimeSlot]
(
	[TimeSlotId] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[SemesterInstanceId] INT NOT NULL,
	[StartTime] Time NOT NULL,
	[EndTime] Time NOT NULL,
	[Type] NVARCHAR(10) NOT NULL CHECK([Type] IN ('class','break')), 
    CONSTRAINT [FK_TimeSlot_SemesterInstance] FOREIGN KEY ([SemesterInstanceId]) REFERENCES dbo.[SemesterInstance]([SemesterInstanceId]),
	CONSTRAINT[CHK_TimeSlot_ValidDuration]
	CHECK(
		([Type] = 'class' AND DATEDIFF(MINUTE,[StartTime],[EndTime]) = 50)
		OR
		([Type] = 'break' AND DATEDIFF(MINUTE,[StartTime],[EndTime])=30)
	)

)
