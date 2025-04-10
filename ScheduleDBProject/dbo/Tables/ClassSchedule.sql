CREATE TABLE [dbo].[ClassSchedule](
    [ScheduleId] INT PRIMARY KEY IDENTITY(1,1),
    [SemesterInstanceId] INT NOT NULL,
    [TimeSlotId] INT NOT NULL,
    [SubjectId] INT NOT NULL,
    [TeacherId] INT NOT NULL ,
    
    CONSTRAINT [FK_ClassSchedule_SemesterInstance] 
        FOREIGN KEY ([SemesterInstanceId]) 
        REFERENCES [dbo].[SemesterInstance]([SemesterInstanceId]),
    
    CONSTRAINT [FK_ClassSchedule_TimeSlot] 
        FOREIGN KEY ([TimeSlotId]) 
        REFERENCES [dbo].[TimeSlot]([TimeSlotId]) 
        ,
    
    CONSTRAINT [FK_ClassSchedule_Subject] 
        FOREIGN KEY ([SubjectId]) 
        REFERENCES [dbo].[Subject]([SubjectId]),
    
    CONSTRAINT [FK_ClassSchedule_Teacher] 
        FOREIGN KEY ([TeacherId]) 
        REFERENCES [dbo].[Teacher]([TeacherId]),
    
    CONSTRAINT [UC_ClassSchedule_UniqueSlot] 
        UNIQUE ([SemesterInstanceId], [TimeSlotId], [TeacherId])
);