using System.ComponentModel.DataAnnotations;
namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class UpdateSemesterInstanceDto
    {
        public int SemesterInstanceId { get; set; }
        [Required(ErrorMessage ="End date is Required")]
        public DateOnly EndDate { get; set; }

       
    }
}
