using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData
{
    public class TeacherAssignmentData : ITeacherAssignmentData
    {
        private readonly ISqlDataAccess _db;

        public TeacherAssignmentData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task Create(TeacherAssignment teacherAssignment)
        {
            return _db.SaveData(storedProcedure: "spTeacherAssignment_Create", new
            {
                teacherAssignment.TeacherId,
                teacherAssignment.SubjectId
            });
        }

        public Task Update(int TeacherAssignmentId,UpdateTeacherAssignmentDto updateTeacherAssignment)
        {
            return _db.SaveData(storedProcedure: "spTeacherAssignment_Update", new 
            { 
                TeacherAssignmentId,
                updateTeacherAssignment.TeacherId 
            });
        }

        public async Task<TeacherAssignment?> GetById(int TeacherAssignmentId)
        {
            var result = await _db.LoadData<TeacherAssignment, dynamic>(storedProcedure: "spTeacherAssignment_GetById", new {TeacherAssignmentId});
            return result.FirstOrDefault();
        }

        public Task Delete(int TeacherAssignmentId)
        {
            return _db.SaveData(storedProcedure: "spTeacherAssignment_Delete", new { TeacherAssignmentId });
        }

        public Task<IEnumerable<JoinedTeacherAssignment>> GetAllTeacherAssignmentData()
        {
            return _db.LoadData<JoinedTeacherAssignment,dynamic>(storedProcedure: "spTeacherAssignment_GetAll", new { });
        }

        
        public async Task<IEnumerable<TeacherAssignment>> GetAllBySemesterId(int SemesterId)
        {
            return await _db.LoadData<TeacherAssignment, dynamic>(storedProcedure: "spTeacherAssignment_GetAllBySemesterId", new {SemesterId});
        }

        public async Task<bool> IsLinkedToActiveSemester(int SubjectId)
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spIsSubjectLinkedToActiveSemester", new { SubjectId });
            return result == 1;
        }

        public async Task<bool> IsExistSubject(int SubjectId) 
        {
            var result = await _db.ExecuteScalarQuery<int, dynamic>(storedProcedure: "spTeacherAssignment_CheckExistingSubjectId", new { SubjectId });
            return result == 1;
        }


        public Task<int> GetTotalTeacherAssignmentForTecherId(int TeacherId)
        {
            return  _db.ExecuteScalarQuery<int,dynamic>(storedProcedure: "spTeacherAssignment_CountTeacherAssignment", new { TeacherId });
        }

    }
}
