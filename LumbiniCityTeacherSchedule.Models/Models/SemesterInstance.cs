using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class SemesterInstance
    {
        public int SemesterInstanceId { get; set; }
        
        public int SemesterId {  get; set; }
        
        public DateTime StartDate { get; set; }

        public string? SemesterStatus { get; set; } 
        public DateTime? EndDate { get; set; }

    }
}
