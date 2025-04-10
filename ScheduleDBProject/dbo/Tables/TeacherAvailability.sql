CREATE TABLE [dbo].[TeacherAvailability]
(
	[TeacherAvailabilityId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TeacherId] INT NOT NULL UNIQUE,
	[StartTime] Time NOT NULL,
	[EndTime] Time NOT NULL,
	CONSTRAINT [FK_TeacherAvailability_Teacher] 
        FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher]([TeacherId]) ON DELETE CASCADE,
    CONSTRAINT [CHK_Availability_Times] 
        CHECK ([EndTime] > [StartTime])
)
