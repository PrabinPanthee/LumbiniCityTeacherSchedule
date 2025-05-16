using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData
{
    public interface ISemesterData
    {
        Task Create(SemesterDTO semester);
        Task Delete(int id);
        Task<Semester?> Get(int id);
        Task<IEnumerable<Semester>> GetAll();
        Task<IEnumerable<Semester>> GetAllByProgramId(int ProgramId);
        Task<bool> IsNumberExist(int SemesterNumber, int ProgramId);
        Task<bool> IsSemesterActive(int SemesterId);
        Task<IEnumerable<Semester>> GetAllActiveSemesterByProgramId(int ProgramId);
    }
}