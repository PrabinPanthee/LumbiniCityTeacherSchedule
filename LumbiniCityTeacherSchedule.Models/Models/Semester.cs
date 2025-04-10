using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
