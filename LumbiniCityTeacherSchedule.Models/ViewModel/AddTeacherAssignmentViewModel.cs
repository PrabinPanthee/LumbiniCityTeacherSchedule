using LumbiniCityTeacherSchedule.Models.Models;
using System.ComponentModel.DataAnnotations;
namespace LumbiniCityTeacherSchedule.Models.ViewModel
{
    public class AddTeacherAssignmentViewModel
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        [Required(ErrorMessage ="Please select teacher")]
        public int? SelectedTeacherId { get; set; }
        public List<TeacherWithAvailability> Assignment { get; set; } = new();
    }
}
