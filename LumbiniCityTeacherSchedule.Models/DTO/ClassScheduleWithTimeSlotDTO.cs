using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class ClassScheduleWithTimeSlotDTO
    {
        public int SemesterInstanceId { get; set; }
        public int TeacherId { get; set; }
        public int TimeSlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
