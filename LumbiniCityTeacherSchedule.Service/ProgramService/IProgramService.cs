using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.ProgramService
{
    public interface IProgramService
    {
        Task<ServiceResult<IEnumerable<Program>>> GetAllPrograms();
        Task<ServiceResult<Program>> GetProgramsById(int id);
        Task<ServiceResult> CreateProgram(CreateProgramDTO program);
        Task<ServiceResult> DeleteProgram(int id);
    }
}
