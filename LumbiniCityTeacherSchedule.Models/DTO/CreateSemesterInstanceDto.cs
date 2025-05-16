using System.ComponentModel.DataAnnotations;
namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class CreateSemesterInstanceDto
    {
        [Required(ErrorMessage ="Semester is Required")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage ="StartDate is required")]
        public DateOnly StartDate { get; set; }
    }
}
