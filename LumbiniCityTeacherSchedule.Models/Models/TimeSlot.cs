using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public int ConfigId { get; set; }
        public int PeriodNumber {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? Type {  get; set; }
    }
}
