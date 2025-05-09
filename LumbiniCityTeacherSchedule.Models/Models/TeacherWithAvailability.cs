using System.ComponentModel.DataAnnotations;


namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class TeacherWithAvailability
    {
        public int TeacherId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public int NumberOfClasses { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
