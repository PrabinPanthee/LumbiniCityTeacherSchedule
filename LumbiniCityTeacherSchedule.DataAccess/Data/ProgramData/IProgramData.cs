using LumbiniCityTeacherSchedule.Models.Models;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData
{
    public interface IProgramData
    {
        Task Create(Program program);
        Task Delete(int id);
        Task<Program?> Get(int id);
        Task<IEnumerable<Program>> GetAll();
        Task<bool> ValidateActiveSemesterForProgram(int programId);
    }
}