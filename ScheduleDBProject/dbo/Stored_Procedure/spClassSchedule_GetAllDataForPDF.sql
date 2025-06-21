CREATE PROCEDURE [dbo].[spClassSchedule_GetAllDataForPDF]

AS
BEGIN
 SELECT 
    p.ProgramName,
    s.SemesterNumber,
    si.SemesterInstanceId,
    si.StartDate,

    ts.TimeSlotId,
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

WHERE si.SemesterStatus = 'active'
ORDER BY 
    p.ProgramName,
    s.SemesterNumber,
    ts.TimeSlotId
    
END
