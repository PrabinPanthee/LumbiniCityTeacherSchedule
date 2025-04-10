using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class UpdateSubjectDto
    {
        [Required]
        public string? SubjectName { get; set; }
        [Required]
        public string? SubjectCode { get; set; }
    }
}
