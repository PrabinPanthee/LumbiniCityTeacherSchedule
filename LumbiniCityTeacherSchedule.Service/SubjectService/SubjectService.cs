using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.SubjectService
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectData _db;
        private readonly ISemesterData _semesterData;
        private readonly ISemesterConfigData _configData;
        public SubjectService(ISubjectData db, ISemesterData semesterData, ISemesterConfigData configData)
        {
            _db = db;
            _semesterData = semesterData;
            _configData = configData;
        }

        public async Task<ServiceResult> Create(CreateSubjectDTO subject)
        {
            var semester = await _semesterData.Get(subject.SemesterId);
            if (semester == null) {
                return ServiceResult.Fail($"Semester with id: {subject.SemesterId} does not exists");
            }
            var isActive = await _semesterData.IsSemesterActive(subject.SemesterId);
            if (isActive) 
            {
                return ServiceResult.Fail("Cannot add subjects for subject linked to active semester");
            }

            var config = await _configData.GetBySemesterId(subject.SemesterId);
            if (config == null)
            {
                return ServiceResult.Fail("Add configuration for the semester at first");
            }
            var maxClasses = config.TotalClasses;
            var existingSubjects = await _db.GetAllSubjectBySemesterId(subject.SemesterId);
            var totalExistingSubjects = existingSubjects.ToList().Count();
            if(totalExistingSubjects >= maxClasses)
            {
                return ServiceResult.Fail("Subjects limit reached");
            }
            subject.SubjectCode = subject.SubjectCode?.Trim().ToUpperInvariant();
            var isExistingCode = await _db.ValidateSubjectCode(subject.SubjectCode!);
            if (isExistingCode)
            {
                return ServiceResult.Fail("Subject code must be Unique");
            }
           
             await _db.Create(subject);
            return ServiceResult.Ok("Created Successfully");
        }

        public async Task<ServiceResult> Delete(int subjectId)
        {
            var subject = await _db.GetById(subjectId);
            if (subject == null)
            {
                return ServiceResult.Fail($"Subject for Id: {subjectId} does not exist");
            }
            var isActive = await _db.IsSemesterActive(subject.SemesterId);
            if (isActive)
            {
                return ServiceResult.Fail("Cannot delete subjects for subject linked to active semester");
            }
            await _db.Delete(subjectId);
            return ServiceResult.Ok("Deleted Successfully");
        }

        public async Task<ServiceResult<IEnumerable<Subject>>> GetAllBySemesterId(int semesterId)
        {
            var semester = await _semesterData.Get(semesterId);
            if (semester == null) 
            {
                return ServiceResult<IEnumerable<Subject>>.Fail($"Semester with Id:{semesterId} does not exist");
            }
            var result = await _db.GetAllSubjectBySemesterId(semesterId);
            if(result == null || !result.Any())
            {
                return ServiceResult<IEnumerable<Subject>>.Fail($"No Subject data available for Semester: {semester.SemesterNumber}");
            }
            return ServiceResult<IEnumerable<Subject>>.Ok(result,"Retrieved successfully");
        }

        public async Task<ServiceResult<Subject>> GetById(int id)
        {
            var result = await _db.GetById(id);
            if(result == null)
            {
                return ServiceResult<Subject>.Fail($"Subject with Id:{id} does not exists");

            }
            return ServiceResult<Subject>.Ok(result, "Retrieved successfully");
        }

        public async Task<ServiceResult> Update(UpdateSubjectDto dto)
        {
            var subject = await _db.GetById(dto.SubjectId);
            if(subject == null)
            {
                return ServiceResult.Fail($"Subject for Id: {dto.SubjectId} does not exist");
            }
            var isActive = await _db.IsSemesterActive(subject.SemesterId);
            if (isActive)
            {
                return ServiceResult.Fail("Cannot update subjects for subject linked to active semester");
            }
            subject.SubjectCode = subject.SubjectCode?.Trim().ToUpperInvariant();
            var isExistingCode = await _db.ValidateSubjectCodeForUpdate(dto.SubjectId,dto.SubjectCode!);
            if (isExistingCode)
            {
                return ServiceResult.Fail("Subject code must be Unique");
            }
            
            await _db.Update(dto);
            return ServiceResult.Ok("Updated successfully");

        }
    }
}
