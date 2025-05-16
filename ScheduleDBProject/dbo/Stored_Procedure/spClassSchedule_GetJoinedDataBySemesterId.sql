CREATE PROCEDURE [dbo].[spClassSchedule_GetJoinedDataBySemesterId]
    @SemesterInstanceId INT
AS
BEGIN
 SELECT 
    p.ProgramName,
    s.SemesterNumber,
    si.SemesterInstanceId,
    ts.TimeSlotId,
    ts.PeriodNumber,
    ts.StartTime,
    ts.EndTime,
    ts.Type,
    subj.SubjectName,
    t.FirstName + ' ' + t.LastName AS TeacherName
FROM SemesterInstance si
JOIN Semester s ON si.SemesterId = s.SemesterId
JOIN Program p ON s.ProgramId = p.ProgramId
JOIN SemesterScheduleConfig config ON s.SemesterId = config.SemesterId
JOIN TimeSlot ts ON ts.ConfigId = config.ConfigId
LEFT JOIN ClassSchedule cs ON cs.SemesterInstanceId = si.SemesterInstanceId AND cs.TimeSlotId = ts.TimeSlotId
LEFT JOIN Subject subj ON cs.SubjectId = subj.SubjectId
LEFT JOIN Teacher t ON cs.TeacherId = t.TeacherId
WHERE si.SemesterInstanceId = @SemesterInstanceId
ORDER BY 
    ts.TimeSlotId
    
END
