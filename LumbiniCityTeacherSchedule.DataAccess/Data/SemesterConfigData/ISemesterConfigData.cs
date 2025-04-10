using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData
{
    public interface ISemesterConfigData
    {
        Task Create(SemesterScheduleConfig semesterScheduleConfig);
        Task<SemesterScheduleConfig?> Get(int Id);
        Task<SemesterScheduleConfig?> GetBySemesterId(int Id);
        Task Update(int ConfigId, int TotalClasses, int? BreakAfterPeriod);

        Task<bool> ValidateActiveSemesterForConfig(int ConfigId);
    }
}