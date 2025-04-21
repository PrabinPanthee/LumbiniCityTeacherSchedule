using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData
{
    public interface IJoinedTeacherAndAvailabilityData
    {
        Task Create(TeacherWithAvailability withAvailability);
        Task Delete(int TeacherId);
        Task<IEnumerable<TeacherWithAvailability>> GetAll();
        Task<TeacherWithAvailability?> GetById(int TeacherId);
        Task Update(int TeacherId,TeacherWithAvailability withAvailability);
        Task<int> GetNumberOfAssignedTeacherInActiveSchedule(int TeacherId);
        Task<bool> IsLinkedToActiveSemester(int TeacherId);
        Task<IEnumerable<TeacherWithAvailability>> GetAllBySemesterId(int SemesterId);
    }
}