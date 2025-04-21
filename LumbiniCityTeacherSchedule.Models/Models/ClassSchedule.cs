using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.Models
{
    public class ClassSchedule
    {
        public int ScheduleId {  get; set; }
        public int SemesterInstanceId { get; set; }
        public int TimeSlotId {  get; set; }
        public int SubjectId {  get; set; }
        public int TeacherId {  get; set; }
    }
}
