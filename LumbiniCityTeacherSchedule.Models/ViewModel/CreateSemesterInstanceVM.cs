using LumbiniCityTeacherSchedule.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Models.ViewModel
{
    public class CreateSemesterInstanceVM
    {
        [Required(ErrorMessage ="SemesterId is required")]
        public int SemesterId {  get; set; }
        [Required(ErrorMessage ="Start Date is required")]
        public List<Program> ProgramList { get; set; } = new();
    }
}
