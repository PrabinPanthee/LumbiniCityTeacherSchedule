
using LumbiniCityTeacherSchedule.Models.DTO;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData
{
    public interface IClassScheduleData
    {
        Task<IEnumerable<ClassScheduleData>> GetAll();
        Task<IEnumerable<ClassScheduleWithTimeSlotDTO>> GetAllWithTimeSlot();
    }
}