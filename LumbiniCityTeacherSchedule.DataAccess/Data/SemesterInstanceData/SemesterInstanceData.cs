using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData
{
    public class SemesterInstanceData : ISemesterInstanceData
    {
        private readonly ISqlDataAccess _dataAccess;

        public SemesterInstanceData(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Task<IEnumerable<SemesterInstance>> GetAll()
        {
            return _dataAccess.LoadData<SemesterInstance, dynamic>(storedProcedure: "spSemesterInstance_GetAllActiveSemester", new { });

        }

        public async Task<SemesterInstance?> GetActiveInstanceBySemesterId(int SemesterId)
        {
            var activeInstance = await _dataAccess.LoadData<SemesterInstance, dynamic>
                (storedProcedure: "spSemesterInstance_GetActiveInstanceBySemesterId", new { SemesterId });
            return activeInstance.FirstOrDefault();

        }

        public async Task<SemesterInstance?> Get(int SemesterInstanceId) 
        {
            var result = await _dataAccess.LoadData<SemesterInstance, dynamic>(storedProcedure: "spSemesterInstance_GetById", new {SemesterInstanceId});
            return result.FirstOrDefault();
        
        }

        public Task Create(CreateSemesterInstanceDto instanceDto) =>
            _dataAccess.SaveData(storedProcedure: "spSemesterInstance_Create", new
            {
                instanceDto.SemesterId,
                instanceDto.StartDate
            });

        public Task Update(int SemesterInstanceId, UpdateSemesterInstanceDto updateDto)
        {
            return _dataAccess.SaveData(storedProcedure: "spSemesterInstance_Update", new { SemesterInstanceId, updateDto.EndDate });
        }

    }
}
