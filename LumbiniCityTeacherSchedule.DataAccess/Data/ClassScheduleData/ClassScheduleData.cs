using Dapper;
using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task BulkInsert(List<ClassSchedule> classSchedules) 
        { 
          var dt = new DataTable();
            dt.Columns.Add("SemesterInstanceId",typeof(int));
            dt.Columns.Add("TimeSlotId", typeof(int));
            dt.Columns.Add("SubjectId", typeof(int));
            dt.Columns.Add("TeacherId", typeof(int));

            foreach(var s in classSchedules)
            {
                dt.Rows.Add(s.SemesterInstanceId,s.TimeSlotId,s.SubjectId,s.TeacherId);
            }
            var parameters = new DynamicParameters();
            parameters.Add("@classSchedules", dt.AsTableValuedParameter("dbo.ClassScheduleTVP"));
            await _db.SaveBulkData("spSchedule_BulkInsert", parameters);
        }
            
    }
}
