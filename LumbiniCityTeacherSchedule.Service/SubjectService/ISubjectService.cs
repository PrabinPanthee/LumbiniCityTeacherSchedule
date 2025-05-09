using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.SubjectService
{
    public interface ISubjectService
    {
        Task<ServiceResult<IEnumerable<Subject>>> GetAllBySemesterId(int semesterId);
        Task<ServiceResult> Delete(int subjectId);
        Task<ServiceResult> Create(CreateSubjectDTO subject);
        Task<ServiceResult> Update(UpdateSubjectDto subject);
        Task<ServiceResult<Subject>> GetById(int id);

    }
}
