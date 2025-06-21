
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData
{
    public interface IClassScheduleData
    {
        Task<IEnumerable<ClassScheduleData>> GetAll();
        Task<IEnumerable<ClassScheduleWithTimeSlotDTO>> GetAllWithTimeSlot();
        Task BulkInsert(List<ClassSchedule> classSchedules);
        Task <IEnumerable<JoinedClassScheduleDataDTO>> GetAllJoinedClassesBySemesterInstanceId(int SemesterInstanceId);
        Task<IEnumerable<ClassSchedulePDFDto>> GetAllJoinedDataForPDF();
        
    }
}