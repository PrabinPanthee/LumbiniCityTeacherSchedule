using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.TeacherAssignmentService
{
    public interface ITeacherAssignmentService
    {
        Task<ServiceResult<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>>> GetAllTeacherAssignmentBySemesterId(int SemesterId);
        Task<ServiceResult> AssignTeacher(int subjectId,TeacherWithAvailability teacher);
    }
}
