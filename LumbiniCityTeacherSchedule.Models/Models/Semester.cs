using System.ComponentModel.DataAnnotations;
namespace LumbiniCityTeacherSchedule.Models.Models
{
    public  class Semester
    {
        public int SemesterId { get; set; }
        [Required]
        public int ProgramId  { get; set; }
        [Required]
        public int SemesterNumber { get; set; }
    }
}
