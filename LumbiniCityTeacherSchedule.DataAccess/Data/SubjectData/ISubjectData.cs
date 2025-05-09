using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData
{
    public interface ISubjectData
    {
        Task Create(CreateSubjectDTO subject);
        Task<IEnumerable<Subject>> GetAllSubjectBySemesterId(int SemesterId);
        Task<Subject?> GetById(int SubjectId);
        Task Update(UpdateSubjectDto subjectDto);
        Task<bool> ValidateSubjectCode(string SubjectCode);
        Task<bool> ValidateSubjectCodeForUpdate(int SubjectId, string SubjectCode);
        Task<bool> IsSemesterActive(int SubjectId);
        Task Delete(int SubjectId);
    }
}