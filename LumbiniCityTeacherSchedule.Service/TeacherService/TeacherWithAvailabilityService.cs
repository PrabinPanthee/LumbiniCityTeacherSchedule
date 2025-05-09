using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.TeacherService
{
    public class TeacherWithAvailabilityService : ITeacherWithAvailabilityService
    {
        private readonly IJoinedTeacherAndAvailabilityData _db;

        public TeacherWithAvailabilityService(IJoinedTeacherAndAvailabilityData db)
        {
            _db = db;
        }

        public async Task<ServiceResult> Create(CreateTeacherWithAvailabilityDTO TeacherDTO)
        {
            int requiredMinutes = TeacherDTO.NumberOfClasses * 50;
            var requiredEndTime = TeacherDTO.StartTime.AddMinutes(requiredMinutes);
            if (TeacherDTO.EndTime < requiredEndTime)
            {
                return ServiceResult.Fail($"The available time is too short for {TeacherDTO.NumberOfClasses} classes. " +
                    $"Starting at {TeacherDTO.StartTime}, the end time must be at least {requiredEndTime}.");
            }
            await _db.Create(TeacherDTO);
            return ServiceResult.Ok("Created Successfully");
        }

        public async Task<ServiceResult> Delete(int TeacherId)
        {
            var isExist = await _db.GetById(TeacherId);
            if (isExist == null)
            {
                return ServiceResult.Fail($"Teacher with ID:{TeacherId} does not exist");
            }
            var isLinkedToActiveSemester = await _db.IsLinkedToActiveSemester(TeacherId);
            if (isLinkedToActiveSemester)
            {
                return ServiceResult.Fail("Cannot Delete Teacher linked to Active Semester");
            }
            await _db.Delete(TeacherId);
            return ServiceResult.Ok("Deleted Successfully");

        }

        public async Task<ServiceResult<IEnumerable<TeacherWithAvailability>>> GetAll()
        {
           var result = await _db.GetAll();
            if (result == null || !result.Any()) 
            {
                return ServiceResult<IEnumerable<TeacherWithAvailability>>.Fail("No data available");
            }
            return ServiceResult<IEnumerable<TeacherWithAvailability>>.Ok(result,"Data retrieved successfully");
        }

        public async Task<ServiceResult<TeacherWithAvailability>> GetById(int TeacherId)
        {
            var result = await _db.GetById(TeacherId);
            if(result == null)
            {
                return ServiceResult<TeacherWithAvailability>.Fail($"Data for ID:{TeacherId} does not exist");
            }
            return ServiceResult<TeacherWithAvailability>.Ok(result, "Data retrieved successfully");
        }

        public async Task<ServiceResult> Update(UpdateTeacherWithAvailabilityDto TeacherDTO)
        {
            var isExist = await _db.GetById(TeacherDTO.TeacherId);
            if (isExist == null)
            {
                return ServiceResult.Fail($"Teacher with ID:{TeacherDTO.TeacherId} does not exist");
            }
            var isLinkedToActiveSemester = await _db.IsLinkedToActiveSemester(TeacherDTO.TeacherId);
            if (isLinkedToActiveSemester)
            {
                return ServiceResult.Fail("Cannot Update Teacher linked to Active Semester");
            }
            int requiredMinutes = TeacherDTO.NumberOfClasses * 50;
            var requiredEndTime = TeacherDTO.StartTime.AddMinutes(requiredMinutes);
            if (TeacherDTO.EndTime < requiredEndTime)
            {
                return ServiceResult.Fail($"The available time is too short for {TeacherDTO.NumberOfClasses} classes. " +
                    $"Starting at {TeacherDTO.StartTime}, the end time must be at least {requiredEndTime}.");
            }
            await _db.Update(TeacherDTO);

            return ServiceResult.Ok("Updated successfully");
        }
    }
}
