using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData
{
    public interface ISemesterData
    {
        Task Create(Semester semester);
        Task Delete(int id);
        Task<Semester?> Get(int id);
        Task<IEnumerable<Semester>> GetAll();
    }
}