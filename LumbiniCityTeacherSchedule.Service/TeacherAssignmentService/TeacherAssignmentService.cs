using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.TeacherAssignmentService
{
    public class TeacherAssignmentService : ITeacherAssignmentService
    {
        private readonly ITeacherAssignmentData _db;
        private readonly ISemesterData _semesterData;
        private readonly ISubjectData _subjectData;
        private readonly IJoinedTeacherAndAvailabilityData _joinedTeacherAndAvailabilityData;

        public TeacherAssignmentService(ITeacherAssignmentData db, ISemesterData semesterData, ISubjectData subjectData, IJoinedTeacherAndAvailabilityData joinedTeacherAndAvailabilityData)
        {
            _db = db;
            _semesterData = semesterData;
            _subjectData = subjectData;
            _joinedTeacherAndAvailabilityData = joinedTeacherAndAvailabilityData;
        }

        public async Task<ServiceResult> AssignTeacher(int subjectId,TeacherWithAvailability teacher)
        {

            var subject = await _subjectData.GetById(subjectId);
            if (subject == null)
            {
                return ServiceResult.Fail("Invalid subjectId");
            }

            var teacherData = await _joinedTeacherAndAvailabilityData.GetById(teacher.TeacherId);
            if (subject == null)
            {
                return ServiceResult.Fail("Invalid teacherId");
            }

            var isAlreadyAssigned = await _db.IsExistSubject(subjectId);
            if (isAlreadyAssigned)
            {
                return ServiceResult.Fail("Subject is already assigned to another teacher");
            }

            var totalAssignedClasses = await _db.GetTotalTeacherAssignmentForTecherId(teacher.TeacherId);
            var maxClassesTeacherCanTake = teacher.NumberOfClasses;

            if(totalAssignedClasses>= maxClassesTeacherCanTake)
            {
                return ServiceResult.Fail($"{teacher.FullName} only can teach maximum {maxClassesTeacherCanTake} subjects");
            }
            var dto = new TeacherAssignment
            {
                TeacherId = teacher.TeacherId,
                SubjectId = subjectId
            };
            await _db.Create(dto);
            return ServiceResult.Ok("Created successfully");
        }

        public async Task<ServiceResult<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>>> GetAllTeacherAssignmentBySemesterId(int SemesterId)
        {
           var semester = await _semesterData.Get(SemesterId);
            if(semester == null)
            {
                return ServiceResult<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>>.Fail("Invalid semester Id");
            }

            var result = await _db.GetAllJoinedTeacherAssignmentBySemesterId(SemesterId);
            if (result == null || !result.Any())
            {
                return ServiceResult<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>>.Fail("No teacher assignments found for this semester");
            }
            return ServiceResult<IEnumerable<JoinedTeacherAssignmentBySubjectDTO>>.Ok(result,"Retrieved successfully");

        }
    }
}
