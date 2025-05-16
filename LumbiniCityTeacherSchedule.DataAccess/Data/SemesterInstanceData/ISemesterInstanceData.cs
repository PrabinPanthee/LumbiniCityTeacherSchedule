using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData
{
    public interface ISemesterInstanceData
    {
        Task Create(CreateSemesterInstanceDto instanceDto);
        Task<SemesterInstance?> GetActiveInstanceBySemesterId(int SemesterId);
        Task<IEnumerable<JoinedSemesterInstanceDto>> GetAll();
        Task Update(UpdateSemesterInstanceDto updateDto);
        Task<SemesterInstance?> Get(int SemesterInstanceId);
    }
}