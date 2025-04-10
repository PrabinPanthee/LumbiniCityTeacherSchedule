CREATE TABLE [dbo].[TimeSlot]
(
	[TimeSlotId] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[ConfigId] INT NOT NULL ,
	[PeriodNumber] TINYINT  NULL,
	[StartTime] Time NOT NULL,
	[EndTime] Time NOT NULL,
	[Type] NVARCHAR(10) NOT NULL CHECK([Type] IN ('class','break')), 
    CONSTRAINT [FK_TimeSlot_SemesterScheduleConfig] FOREIGN KEY ([ConfigId]) REFERENCES dbo.[SemesterScheduleConfig]([ConfigId]) ON DELETE CASCADE,
	CONSTRAINT[CHK_TimeSlot_ValidDuration]
	CHECK(
		([Type] = 'class' AND DATEDIFF(MINUTE,[StartTime],[EndTime]) = 50)
		OR
		([Type] = 'break' AND DATEDIFF(MINUTE,[StartTime],[EndTime])=30)
	),
	CONSTRAINT [CHK_TimeSlot_PeriodNumber] CHECK (
        ([Type] = 'class' AND [PeriodNumber] IS NOT NULL)
        OR
        ([Type] = 'break' AND [PeriodNumber] IS NULL)
    )

)
