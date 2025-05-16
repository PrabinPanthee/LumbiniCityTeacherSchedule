using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.ViewModel
{
    public class ClassScheduleVM
    {
        public List<Program> Programs { get; set; } = new();
        public List<Semester> Semesters { get; set; } = new();
        public int SelectedProgramId { get; set; }
        public int SelectedSemesterId { get; set; }
        public List<JoinedClassScheduleDataDTO> classSchedule { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
