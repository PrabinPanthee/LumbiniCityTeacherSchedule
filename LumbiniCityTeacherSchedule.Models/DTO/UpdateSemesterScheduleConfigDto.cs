using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class UpdateSemesterScheduleConfigDto
    {
        [Required]
        public int TotalClasses { get; set; }


        public int? BreakAfterPeriod { get; set; }
    }
}
