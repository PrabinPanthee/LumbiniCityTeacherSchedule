//using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
//using LumbiniCityTeacherSchedule.Models.DTO;
//using LumbiniCityTeacherSchedule.Models.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAvailabilityData
//{
//    public class TeacherAvailabilityData : ITeacherAvailabilityData
//    {
//        private readonly ISqlDataAccess _db;

//        public TeacherAvailabilityData(ISqlDataAccess db)
//        {
//            _db = db;
//        }

//        public async Task<TeacherAvailability?> GetByTeacherId(int TeacherId)
//        {
//            var result = await _db.LoadData<TeacherAvailability, dynamic>(storedProcedure: "spTeacherAvailability_GetByTeacherId", new { TeacherId });
//            return result.FirstOrDefault();
//        }

//        public Task Create(TeacherAvailability teacherAvailability)
//        {
//            return _db.SaveData(storedProcedure: "spTeacher_AddAvailability",
//                new
//                {
//                    teacherAvailability.TeacherId,
//                    teacherAvailability.StartTime,
//                    teacherAvailability.EndTime
//                });
//        }

//        public Task Update(int TeacherAvailabilityId, UpdateTeacherAvailabilityDto teacherAvailability)
//        {
//            return _db.SaveData(storedProcedure: "spTeacher_UpdateAvailability",
//                new
//                {
//                    TeacherAvailabilityId,
//                    teacherAvailability.StartTime,
//                    teacherAvailability.EndTime
//                });
//        }
//    }
//}
