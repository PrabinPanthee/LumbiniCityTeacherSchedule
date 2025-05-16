

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class JoinedClassScheduleDataDTO
    {
        public string? ProgramName { get; set; }
        public int SemesterNumber { get; set; }
        public int SemesterInstanceId { get; set; }
        public int TimeSlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Type { get; set; }
        public string? SubjectName { get; set; }
        public string? TeacherName { get; set; }

    }
}
