using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.SemesterInstanceService
{
    public interface ISemesterInstanceService
    {
        Task<ServiceResult> Create(CreateSemesterInstanceDto dto);
        Task<ServiceResult> Update(UpdateSemesterInstanceDto dto);
        Task<ServiceResult<IEnumerable<JoinedSemesterInstanceDto>>> GetAllActiveSemesterInstance();

    }
}
