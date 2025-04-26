using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterData _db;
        private readonly IProgramData _programData;

        public SemesterService(ISemesterData db, IProgramData programData)
        {
            _db = db;
            _programData = programData;
        }


        public async Task<ServiceResult> Create(SemesterDTO Semester)
        {
            var program = await _programData.Get(Semester.ProgramId);
            if (program == null)
            {
                ServiceResult.Fail("The program does not exist");
            }
            var isNumberExist = await _db.IsNumberExist(Semester.SemesterNumber, Semester.ProgramId);
            if (isNumberExist)
            {
                return ServiceResult.Fail($"The SemesterNumber {Semester.SemesterNumber} already exists");
            }
            await _db.Create(Semester);
            return ServiceResult.Ok($"Created Successfully");
        }

        public async Task<ServiceResult> Delete(int SemesterId)
        {
            var semester = await _db.Get(SemesterId);
            if(semester == null)
            {
                return ServiceResult.Fail($"Semester with Id:{SemesterId} does not exist");
            }
            var isActive = await _db.IsSemesterActive(SemesterId);
            if (isActive) 
            {
                return ServiceResult.Fail("Cannot delete the semester which is active");
            }
            await _db.Delete(SemesterId);
            return ServiceResult.Ok("Deleted Successfully");
        }

        public Task<ServiceResult<Semester>> GetSemesterById(int SemesterId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<IEnumerable<Semester>>> GetSemesterByProgramId(int ProgramId)
        {
            var program = await _programData.Get(ProgramId);
            if (program == null)
            {
                ServiceResult<IEnumerable<Semester>>.Fail("The program does not exist");
            }

            var result = await _db.GetAllByProgramId(ProgramId);
            if (result == null || !result.Any())
            {
                return ServiceResult<IEnumerable<Semester>>.Fail($"The data for {program!.ProgramName} not found.");
            }
            return ServiceResult<IEnumerable<Semester>>.Ok(result, "Retrieved all data");
        }
    }
}
