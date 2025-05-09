using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData
{
    public interface ITeacherAssignmentData
    {
        Task Create(TeacherAssignment teacherAssignment);
        Task<bool> IsExistSubject(int SubjectId);
        Task<int> GetTotalTeacherAssignmentForTecherId(int TeacherId);
        Task Update(int TeacherAssignmentId, UpdateTeacherAssignmentDto updateTeacherAssignment);
        Task<bool> IsLinkedToActiveSemester(int SubjectId);
        Task<TeacherAssignment?> GetById(int TeacherAssignmentId);
        Task Delete(int TeacherAssignmentId);
        Task<IEnumerable<JoinedTeacherAssignment>> GetAllTeacherAssignmentData();
        Task<IEnumerable<TeacherAssignment>> GetAllBySemesterId(int SemesterId);
        Task<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>> GetAllJoinedTeacherAssignmentBySemesterId(int SemesterId);
    }
}
