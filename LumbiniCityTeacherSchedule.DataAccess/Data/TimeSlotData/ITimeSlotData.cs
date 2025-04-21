using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TimeSlotData
{
    public interface ITimeSlotData
    {
        Task<IEnumerable<TimeSlot>> GetAllByConfigId(int ConfigId);
    }
}