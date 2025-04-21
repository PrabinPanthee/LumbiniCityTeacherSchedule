using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class TeacherAssignment
    {
        public int TeacherAssignmentId { get; set; }
        [Required(ErrorMessage ="Teacher is required")]
        public int TeacherId { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public int SubjectId { get; set; }
    }
}
