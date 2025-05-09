using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Service.ConfigurationService
{
    public interface ISemesterScheduleConfigService
    {
        Task<ServiceResult<SemesterScheduleConfig>> GetSemesterScheduleConfig(int SemesterId);
        Task<ServiceResult> Update(UpdateSemesterScheduleConfigDto Config);
        Task<ServiceResult> Create(CreateSemesterScheduleConfigDTO configDTO);
        Task<ServiceResult<SemesterScheduleConfig>> Get(int ConfigId);
    }
}
