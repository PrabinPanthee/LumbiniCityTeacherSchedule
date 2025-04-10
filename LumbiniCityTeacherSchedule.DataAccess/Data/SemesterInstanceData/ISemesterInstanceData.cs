using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData
{
    public interface ISemesterInstanceData
    {
        Task Create(CreateSemesterInstanceDto instanceDto);
        Task<SemesterInstance?> GetActiveInstanceBySemesterId(int SemesterId);
        Task<IEnumerable<SemesterInstance>> GetAll();
        Task Update(int InstanceId, UpdateSemesterInstanceDto updateDto);
        Task<SemesterInstance?> Get(int SemesterInstanceId);
    }
}