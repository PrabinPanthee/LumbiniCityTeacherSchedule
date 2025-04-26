using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class SemesterScheduleConfig
    {
        public int ConfigId { get; set; }
        [Required]
        public int SemesterId { get; set; }
        [Required]
        public int TotalClasses { get; set; }

        public int? BreakAfterPeriod { get; set; }
    }
}
