CREATE TRIGGER [dbo].[ValidateTeacherAssignment]
ON [dbo].[ClassSchedule]
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE NOT EXISTS (
            SELECT 1
            FROM [dbo].[TeacherAssignment] ta
            WHERE ta.TeacherId = i.TeacherId
                AND ta.SubjectId = i.SubjectId
        )
    )
    BEGIN
        RAISERROR('Teacher is not assigned to this subject.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;