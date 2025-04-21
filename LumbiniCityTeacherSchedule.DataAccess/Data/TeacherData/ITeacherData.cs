using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TeacherData
{
    public interface ITeacherData
    {
        Task Create(Teacher teacher);
        Task Delete(int TeacherId);
        Task<IEnumerable<Teacher>> GetAll();
        Task<Teacher?> GetById(int TeacherId);
        Task Update(int TeacherId, Teacher teacher);
        Task<int> GetNumberOfAssignedTeacherInActiveSchedule(int TeacherId);
        Task<bool> IsLinkedToActiveSemester(int TeacherId);
    }
}