using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData
{
    public class SemesterData : ISemesterData
    {
        private readonly ISqlDataAccess _db;

        public SemesterData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<Semester>> GetAll()
        {
            return _db.LoadData<Semester, dynamic>(storedProcedure: "dbo.spSemester_GetAll", new { });
        }
        public async Task<Semester?> Get(int id)
        {
            var result = await _db.LoadData<Semester, dynamic>(storedProcedure: "dbo.spSemester_Get", new { SemesterId = id });
            return result.FirstOrDefault();
        }

        public Task Create(SemesterDTO semester)
        {
            return _db.SaveData(storedProcedure: "dbo.spSemester_Create", new { semester.ProgramId, semester.SemesterNumber });
        }

      //expression bodied method returns task 
        public Task Delete(int id) =>
            _db.SaveData(storedProcedure: "dbo.spSemester_Delete", new { SemesterId = id });

        public Task<IEnumerable<Semester>> GetAllByProgramId(int ProgramId)
        {
            return _db.LoadData<Semester,dynamic>(storedProcedure: "spSemester_GetAllByProgramId",new {ProgramId});
        }

        public async Task<bool> IsNumberExist(int SemesterNumber, int ProgramId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spSemester_DuplicateSemesterNumber",
                                new {SemesterNumber,ProgramId });
            return result == 1;
        }

        public async Task<bool>IsSemesterActive(int SemesterId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsSemesterActive", new {SemesterId});
            return result == 1;
        }

        public Task<IEnumerable<Semester>> GetAllActiveSemesterByProgramId(int ProgramId)
        {
            return _db.LoadData<Semester,dynamic>(storedProcedure: "spSemester_GetAllActiveSemesterByProgramId", new {ProgramId});
        }
    }
}

