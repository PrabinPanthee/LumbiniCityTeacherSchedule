
namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class JoinedSemesterInstanceDto
    {
        public int SemesterInstanceId { get; set; }
        public string? ProgramName { get; set; }
        public int SemesterNumber { get; set; }
        public DateOnly StartDate { get; set; }
        public string? SemesterStatus { get; set; }

    }
}
