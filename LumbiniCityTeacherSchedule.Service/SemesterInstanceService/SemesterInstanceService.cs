using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Results;
using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Service.SemesterInstanceService
{
    public class SemesterInstanceService : ISemesterInstanceService
    {
        private readonly ISemesterInstanceData _data;
        private readonly ISemesterData _semesterData;
        private readonly ISemesterConfigData _semesterConfigData;
        private readonly ISubjectData _subjectData;
        private readonly ITeacherAssignmentData _teacherAssignment;

        public SemesterInstanceService(ISemesterInstanceData data, ISemesterData semesterData, ISemesterConfigData semesterConfigData, ISubjectData subjectData, ITeacherAssignmentData teacherAssignment)
        {
            _data = data;
            _semesterData = semesterData;
            _semesterConfigData = semesterConfigData;
            _subjectData = subjectData;
            _teacherAssignment = teacherAssignment;
        }

        public async Task<ServiceResult> Create(CreateSemesterInstanceDto dto)
        {
            var semester = await _semesterData.Get(dto.SemesterId);
            if (semester == null)
            {
                return ServiceResult.Fail("Semester does not exist");
            }
            var isActiveAlready = await _data.GetActiveInstanceBySemesterId(dto.SemesterId);
            if (isActiveAlready != null)
            {
                return ServiceResult.Fail("Semester is already active");
            }
             var configData = await _semesterConfigData.GetBySemesterId(dto.SemesterId);
            if(configData == null)
            {
                return ServiceResult.Fail($"Cannot activate semester when it has no config data");
            }

            var subjects = await _subjectData.GetAllSubjectBySemesterId(dto.SemesterId);
            if (subjects == null || !subjects.Any()) 
            {
                return ServiceResult.Fail("Cannot activate semester when it has no subject data");
            }
            var subjectCount = subjects.Count();
            if(configData.TotalClasses != subjectCount) 
            {
                return ServiceResult.Fail($"Add all {configData.TotalClasses} before activating");
            }

            var teacherAssignment = await _teacherAssignment.GetAllBySemesterId(dto.SemesterId);
            var assignmentsCount = teacherAssignment.Count();
            if (configData.TotalClasses != assignmentsCount)
            {
                return ServiceResult.Fail($"Assign all {configData.TotalClasses} subjects with teacher to activate semester" );
            }

            await _data.Create(dto);
            return ServiceResult.Ok("Created successfully");
  
        }

        public async Task<ServiceResult<IEnumerable<JoinedSemesterInstanceDto>>> GetAllActiveSemesterInstance()
        {
            var result = await _data.GetAll();
            if (result == null || !result.Any())
            {
                return ServiceResult<IEnumerable<JoinedSemesterInstanceDto>>.Fail("No any data available");
            }
            return ServiceResult<IEnumerable<JoinedSemesterInstanceDto>>.Ok(result,"Retrieved successfully");
        }

        public async Task<ServiceResult> Update(UpdateSemesterInstanceDto dto)
        {
            var instance = await _data.Get(dto.SemesterInstanceId);
            if(instance == null)
            {
                return ServiceResult.Fail("Invalid SemesterInstanceId");
            }
            if(instance.SemesterStatus != "active")
            {
                return ServiceResult.Fail("Semester is already completed");
            }
            if(dto.EndDate < instance.StartDate)
            {
                return ServiceResult.Fail("Invalid End date cant be less than start date");
            }
            await _data.Update(dto);
            return ServiceResult.Ok("Updated successfully");
        }
    }
}
