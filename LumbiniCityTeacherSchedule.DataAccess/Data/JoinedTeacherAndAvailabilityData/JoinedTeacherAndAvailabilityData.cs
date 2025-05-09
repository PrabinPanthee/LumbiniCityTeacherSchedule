using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData
{
    public class JoinedTeacherAndAvailabilityData : IJoinedTeacherAndAvailabilityData
    {
        private readonly ISqlDataAccess _db;

        public JoinedTeacherAndAvailabilityData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<TeacherWithAvailability>> GetAll()
        {
            return _db.LoadData<TeacherWithAvailability, dynamic>(storedProcedure: "spTeacherWithAvailability_GetAll", new { });
        }

        public async Task<IEnumerable<TeacherWithAvailability>> GetAllBySemesterId(int SemesterId)
        {
            return await _db.LoadData<TeacherWithAvailability, dynamic>
                        (storedProcedure: "spTeacherAssignment_GetAllTeacherWithAvailabilityBySemesterId", new {SemesterId});
        }

        public async Task<TeacherWithAvailability?> GetById(int TeacherId)
        {
            var result = await _db.LoadData<TeacherWithAvailability,
                        dynamic>(storedProcedure: "spTeacherWithAvailability_GetById", new { TeacherId });
            return result.FirstOrDefault();
        }

        public Task Create(CreateTeacherWithAvailabilityDTO withAvailability)
        {
            return _db.SaveData(storedProcedure: "spTeacherWithAvailability_Create",
                new
                {
                    withAvailability.FirstName,
                    withAvailability.LastName,
                    withAvailability.NumberOfClasses,
                    withAvailability.StartTime,
                    withAvailability.EndTime
                });
        }

        public Task Update(UpdateTeacherWithAvailabilityDto withAvailability)
        {
            return _db.SaveData(storedProcedure: "spTeacherWithAvailability_Update", new
            {
                withAvailability.TeacherId,
                withAvailability.FirstName,
                withAvailability.LastName,
                withAvailability.NumberOfClasses,
                withAvailability.StartTime,
                withAvailability.EndTime
            });
        }

        public Task Delete(int TeacherId)
        {
            return _db.SaveData(storedProcedure: "spTeacher_Delete", new { TeacherId });
        }

        public Task<int> GetNumberOfAssignedTeacherInActiveSchedule(int TeacherId)
        {
            return _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spCountAssignedClasses", new { TeacherId });
        }
        public async Task<bool> IsLinkedToActiveSemester(int TeacherId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsteacherLinkedToActiveSemester", new { TeacherId });
            return result == 1;
            
        }

    }
}
