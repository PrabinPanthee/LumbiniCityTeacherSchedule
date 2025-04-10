using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class SemesterScheduleConfig
    {
        public int ConfigId { get; set; }
        [Required]
        public int SemesterId { get; set; }
        [Required]
        public int TotalClasses { get; set; }

        public int? BreakAfterPeriod { get; set; }
    }
}
