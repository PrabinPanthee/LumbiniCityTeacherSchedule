
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.ClassScheduleService
{
    public interface IClassScheduleService
    {
        Task<ServiceResult> GenerateClassSchedule(int SemesterInstanceId);
        Task<ServiceResult<IEnumerable<JoinedClassScheduleDataDTO>>> GetAllBySemesterId(int semesterId);
        Task<ServiceResult<IEnumerable<ClassSchedulePDFDto>>> GetAllDataForPDF();
    }

    
}
