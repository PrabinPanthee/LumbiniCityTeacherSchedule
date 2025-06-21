
namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class ClassSchedulePDFDto
    {
        public string ProgramName { get; set; } = string.Empty;
        public int SemesterNumber { get; set; }
        public int SemesterInstanceId { get; set; }
        public DateTime StartDate { get; set; }

        // Time Slot Info
        public int TimeSlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Type { get; set; } = string.Empty; // "class" or "break"

        // Subject Info
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }

        // Teacher Info
        public string? TeacherName { get; set; }
    }
}
