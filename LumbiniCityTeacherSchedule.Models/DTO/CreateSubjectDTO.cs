
using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class CreateSubjectDTO
    {
        [Required(ErrorMessage ="Program Id is required")]
        public int ProgramId { get; set; }  
        [Required(ErrorMessage ="SemesterId is Required")]
        public int SemesterId { get; set; }
        [Required(ErrorMessage ="SubjectName is Required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string? SubjectName { get; set; }
        [Required(ErrorMessage ="Subject Code is required)")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)[A-Z\\d]+$", ErrorMessage = 
            "Subject code must contain at least one uppercase letter, one number, and only consist of uppercase letters and digits.")]
        public string? SubjectCode { get; set; }
    }
}
