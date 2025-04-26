using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.SemesterService
{
    public interface ISemesterService
    {
        Task<ServiceResult<IEnumerable<Semester>>> GetSemesterByProgramId(int ProgramId);
        Task<ServiceResult<Semester>> GetSemesterById(int SemesterId);
        Task<ServiceResult> Delete(int  SemesterId);
        Task<ServiceResult> Create(SemesterDTO Semester);
    }
}
