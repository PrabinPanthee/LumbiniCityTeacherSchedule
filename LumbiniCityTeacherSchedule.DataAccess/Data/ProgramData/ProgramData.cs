using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData
{
    public class ProgramData : IProgramData
    {
        private readonly ISqlDataAccess _db;

        public ProgramData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<Program>> GetAll()
        {
            return _db.LoadData<Program, dynamic>(storedProcedure: "dbo.spProgram_GetAll", new { });
        }
        public async Task<Program?> Get(int id)
        {
            var result = await _db.LoadData<Program, dynamic>(storedProcedure: "dbo.spProgram_Get", new { ProgramId = id });
            return result.FirstOrDefault();
        }

        public Task Create(Program program)
        {
            return _db.SaveData(storedProcedure: "dbo.spProgram_Create", new { program.ProgramName });
        }

        public Task Delete(int id) =>
            _db.SaveData(storedProcedure: "dbo.spProgram_Delete", new { ProgramId = id });

        public async Task<bool> ValidateActiveSemesterForProgram(int programId) 
        {
            var result = await _db.ExecuteScalarQuery<int,dynamic>(storedProcedure:"spIsProgramLinkedToActiveSemester",new { programId });
            return result == 1;
        } 
    }
}
