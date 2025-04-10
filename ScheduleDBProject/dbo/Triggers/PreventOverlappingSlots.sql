CREATE TRIGGER [dbo].[PreventOverlappingSlots]
ON [dbo].[ClassSchedule]
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN [dbo].[TimeSlot] ts ON i.TimeSlotId = ts.TimeSlotId
        JOIN [dbo].[ClassSchedule] cs 
            ON i.TeacherId = cs.TeacherId
            AND i.ScheduleId != cs.ScheduleId
        JOIN [dbo].[TimeSlot] ts2 ON cs.TimeSlotId = ts2.TimeSlotId
        WHERE ts.StartTime < ts2.EndTime AND ts.EndTime > ts2.StartTime
    )
    BEGIN
        RAISERROR('Teacher is already booked during this time.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
