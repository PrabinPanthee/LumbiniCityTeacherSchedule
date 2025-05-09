using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData
{
    public class SubjectData : ISubjectData
    {
        private readonly ISqlDataAccess _db;

        public SubjectData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<Subject>> GetAllSubjectBySemesterId(int SemesterId)
        {
            return _db.LoadData<Subject, dynamic>(storedProcedure: "spSubject_GetAllBySemesterId", new { SemesterId });

        }
        public Task Create(CreateSubjectDTO subject) =>
        _db.SaveData(storedProcedure: "spSubject_Create", new { subject.SemesterId, subject.SubjectName, subject.SubjectCode });


        public Task Update(UpdateSubjectDto dto) =>
            _db.SaveData(storedProcedure: "spSubject_Update", new {dto.SubjectId, dto.SubjectName, dto.SubjectCode });

        public async Task<Subject?> GetById(int SubjectId)
        {
            var result = await _db.LoadData<Subject, dynamic>(storedProcedure: "spSubject_Get", new { SubjectId });
            return result.FirstOrDefault();
        }

        public async Task<bool> ValidateSubjectCode(string SubjectCode)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "isSubjectCodeUnique", new { SubjectCode });
            return result == 1;

        }

        public async Task<bool> ValidateSubjectCodeForUpdate(int SubjectId,string SubjectCode) 
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsSubjectUniqueForUpdate", 
                new {SubjectId,SubjectCode});
            return result == 1;
        }

        public async Task<bool> IsSemesterActive(int SubjectId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsSubjectLinkedToActiveSemester", new { SubjectId });
            return result == 1;
        }

        public  Task Delete(int SubjectId) 
        {
            return  _db.SaveData(storedProcedure: "spSubject_Delete", new { SubjectId });
        }

        
        
    }
}
