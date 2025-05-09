using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class UpdateSubjectDto
    {
        public int ProgramId {  get; set; }
        [Required(ErrorMessage ="Subject id is required")]
        public int SemesterId { get; set; }
        public int SubjectId {  get; set; }
        [Required(ErrorMessage = "Subject name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string? SubjectName { get; set;}
        [Required(ErrorMessage = "Subject Code is required)")]
        [RegularExpression("^(?=.*[A-Z])(?=.*\\d)[A-Z\\d]+$", ErrorMessage =
            "Subject code must contain at least one uppercase letter, one number, and only consist of uppercase letters and digits.")]
        public string? SubjectCode { get; set; }
    }
}
