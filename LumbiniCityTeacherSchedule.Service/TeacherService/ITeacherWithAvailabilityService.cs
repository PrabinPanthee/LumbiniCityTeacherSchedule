using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;
namespace LumbiniCityTeacherSchedule.Service.TeacherService
{
    public interface ITeacherWithAvailabilityService
    {
        Task<ServiceResult<IEnumerable<TeacherWithAvailability>>> GetAll();
        Task<ServiceResult<TeacherWithAvailability>> GetById(int TeacherId);
        Task<ServiceResult> Create(CreateTeacherWithAvailabilityDTO TeacherDTO);
        Task<ServiceResult> Update(UpdateTeacherWithAvailabilityDto TeacherDTO);
        Task<ServiceResult> Delete(int TeacherId);
    }
}
