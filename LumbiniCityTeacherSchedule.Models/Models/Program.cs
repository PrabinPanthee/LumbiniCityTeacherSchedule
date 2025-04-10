using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class Program
    {
        public int ProgramId { get; set; }
        [Required]
        public  string? ProgramName {  get; set; }
    }
}
