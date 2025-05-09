using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;


namespace LumbiniCityTeacherSchedule.Models.ViewModel
{
     public class TeacherAssignmentViewModel
    {
        public List<Program> Programs { get; set; } = new();
        public List<Semester> Semesters { get; set; } = new();
        public int SelectedProgramId { get; set; }
        public int SelectedSemesterId { get; set; }
        public List<JoinedTeacherAssignmentBySubjectDTO> Assignments { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
