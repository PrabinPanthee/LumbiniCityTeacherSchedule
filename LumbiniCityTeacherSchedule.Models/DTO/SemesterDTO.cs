using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class SemesterDTO
    {
        [Required(ErrorMessage ="Program is Required")]
        public int ProgramId { get; set; }
        [Required(ErrorMessage ="Semester Number is Required")]
        [Range(1,8,ErrorMessage ="Must be between 1 and 8")]
        public int SemesterNumber { get; set; }
    }
}
