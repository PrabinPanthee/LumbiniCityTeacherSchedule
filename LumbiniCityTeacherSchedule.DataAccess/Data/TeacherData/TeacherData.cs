using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TeacherData
{
    public class TeacherData : ITeacherData
    {
        private readonly ISqlDataAccess _db;

        public TeacherData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<Teacher>> GetAll()
        {
            return _db.LoadData<Teacher, dynamic>(storedProcedure: "spTeacher_GetAll", new { });
        }

        public async Task<Teacher?> GetById(int TeacherId)
        {
            var result = await _db.LoadData<Teacher, dynamic>(storedProcedure: "spTeacher_Get", new { TeacherId });
            return result.FirstOrDefault();
        }

        public Task Create(Teacher teacher)
        {
            return _db.SaveData(storedProcedure: "spTeacher_Create", new { teacher.FirstName, teacher.LastName, teacher.NumberOfClasses });
        }

        public Task Update(int TeacherId, Teacher teacher)
        {

            return _db.SaveData(storedProcedure: "spTeacher_Update",
                new
                {
                    TeacherId,
                    teacher.FirstName,
                    teacher.LastName,
                    teacher.NumberOfClasses
                });
        }

        public Task Delete(int TeacherId)
        {

            return _db.SaveData(storedProcedure: "spTeacher_Delete", new { TeacherId });
        }

        public  Task<int> GetNumberOfAssignedTeacherInActiveSchedule(int TeacherId)
        {
            return _db.ExecuteScalarQuery<int,dynamic>(storedProcedure: "spCountAssignedClasses",new { TeacherId });
        }

        public async Task<bool>IsLinkedToActiveSemester(int TeacherId)
        {
            var result = await _db.ExecuteScalarQuery<int,dynamic>(storedProcedure: "spIsteacherLinkedToActiveSemester",new{TeacherId});
            return result == 1;
        }
    }
}
