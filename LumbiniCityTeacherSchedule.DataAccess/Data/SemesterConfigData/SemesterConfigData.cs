using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData
{
    public class SemesterConfigData : ISemesterConfigData
    {
        private readonly ISqlDataAccess _db;

        public SemesterConfigData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<SemesterScheduleConfig?> Get(int Id)
        {
            var semesterScheduleConfig = await _db.LoadData<SemesterScheduleConfig, dynamic>
                (storedProcedure: "spSemesterScheduleConfig_Get", new { ConfigId = Id });
            return semesterScheduleConfig.FirstOrDefault();
        }

        public async Task<SemesterScheduleConfig?> GetBySemesterId(int Id)
        {
            var semesterScheduleConfig = await _db.LoadData<SemesterScheduleConfig, dynamic>
                (storedProcedure: "spSemesterScheduleConfig_GetBySemesterId", new { SemesterId = Id });
            return semesterScheduleConfig.FirstOrDefault();
        }

        public Task Create(SemesterScheduleConfig semesterScheduleConfig)
        {
            return _db.SaveData(storedProcedure: "spSemesterScheduleConfig_Create", new
            {
                semesterScheduleConfig.SemesterId,
                semesterScheduleConfig.BreakAfterPeriod,
                semesterScheduleConfig.TotalClasses
            });

        }

        public Task Update(int ConfigId,int TotalClasses,int? BreakAfterPeriod )
        {
            return _db.SaveData(storedProcedure: "spSemesterScheduleConfig_Update", new
            {
                ConfigId,
                BreakAfterPeriod,
                TotalClasses
            });
        }

        public async Task<bool> ValidateActiveSemesterForConfig(int ConfigId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsConfigLinkedToActiveSemester", new { ConfigId });
            return result == 1;
        }
    }
    
}
