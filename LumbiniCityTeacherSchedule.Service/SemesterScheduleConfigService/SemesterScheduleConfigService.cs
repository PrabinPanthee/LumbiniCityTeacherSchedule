using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;
using LumbiniCityTeacherSchedule.Service.ConfigurationService;

namespace LumbiniCityTeacherSchedule.Service.SemesterScheduleConfigService
{
    public class SemesterScheduleConfigService : ISemesterScheduleConfigService
    {
        private readonly ISemesterConfigData _db;
        private readonly ISemesterData _semesterData;
        public SemesterScheduleConfigService(ISemesterConfigData db, ISemesterData semesterData)
        {
            _db = db;
            _semesterData = semesterData;
        }

        public async Task<ServiceResult> Create(CreateSemesterScheduleConfigDTO configDTO)
        {
            var semester = await _semesterData.Get(configDTO.SemesterId);
            if (semester == null)
            {
                return ServiceResult.Fail($"Semester with Id:{configDTO.SemesterId} does not exist");
            }
            var configData = await _db.GetBySemesterId(configDTO.SemesterId);
            if(!(configData == null))
            {
                return ServiceResult.Fail($"Configuration for Semester:{semester.SemesterNumber} already exists");
            }
            await _db.Create(configDTO);

            return ServiceResult.Ok("Successfully created");
        }

        public async Task<ServiceResult<SemesterScheduleConfig>> Get(int ConfigId)
        {
            var config = await _db.Get(ConfigId);
            if(config == null)
            {
                return ServiceResult<SemesterScheduleConfig>.Fail($"Cannot find the configuration for Id :{ConfigId}");
            }
            return ServiceResult<SemesterScheduleConfig>.Ok(config,"Data retrieved successfully");
        }

        public async Task<ServiceResult<SemesterScheduleConfig>> GetSemesterScheduleConfig(int semesterId)
        {
            var semester = await _semesterData.Get(semesterId);
            if (semester == null) {
                return ServiceResult<SemesterScheduleConfig>.Fail($"Semester with Id:{semesterId} does not exist");
            }
            var result = await _db.GetBySemesterId(semesterId);
            if (result == null) 
            {
                return ServiceResult<SemesterScheduleConfig>.Fail($"No config data available for Semester:{semester.SemesterNumber}");
            }
            return ServiceResult<SemesterScheduleConfig>.Ok(result,"Data retrieved successfully");

        }

        public async Task<ServiceResult> Update(UpdateSemesterScheduleConfigDto Config)
        {
            var config = await _db.Get(Config.ConfigId);
            if (config == null) 
            {
                return ServiceResult.Fail($"Configuration with Id:{Config.ConfigId} does not exist");
            }
            var isActive = await _db.ValidateActiveSemesterForConfig(Config.ConfigId);
            if (isActive)
            {
                return ServiceResult.Fail("Cannot update the configuration for active semester");
            }
            await _db.Update(Config.ConfigId,Config.TotalClasses,Config.BreakAfterPeriod);
            return ServiceResult.Ok("Updated Successfully");

        }
    }
}
