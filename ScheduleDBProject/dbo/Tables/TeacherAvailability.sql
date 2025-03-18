CREATE TABLE [dbo].[TeacherAvailability]
(
	[TeacherAvailabilityId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TeacherId] INT NOT NULL,
	[StartTime] Time NOT NULL,
	[EndTime] Time NOT NULL,
	CONSTRAINT [FK_TeacherAvailability_Teacher] 
        FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher]([TeacherId]),
    CONSTRAINT [CHK_Availability_Times] 
        CHECK ([EndTime] > [StartTime])
)
