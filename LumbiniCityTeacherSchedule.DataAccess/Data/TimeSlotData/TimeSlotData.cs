using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TimeSlotData
{
    public class TimeSlotData : ITimeSlotData
    {
        private readonly ISqlDataAccess _db;

        public TimeSlotData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<TimeSlot>> GetAllByConfigId(int ConfigId)
        {
            return _db.LoadData<TimeSlot, dynamic>(storedProcedure: "spTimeSlot_GetByConfigId", new { ConfigId });
        }
    }
}
