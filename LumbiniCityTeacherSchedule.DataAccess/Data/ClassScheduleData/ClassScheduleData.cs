using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData
{
    public class ClassScheduleData : IClassScheduleData
    {
        private readonly ISqlDataAccess _db;
        public ClassScheduleData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<ClassScheduleData>> GetAll()
        {
            return _db.LoadData<ClassScheduleData, dynamic>(storedProcedure: "spClassSchedule_GetAll", new { });
        }

        public Task<IEnumerable<ClassScheduleWithTimeSlotDTO>> GetAllWithTimeSlot() 
        {

            return _db.LoadData<ClassScheduleWithTimeSlotDTO, dynamic>(storedProcedure: "spClassScheduleWithTimeSlot",new {});
        
        }

            
            


            
    }
}
