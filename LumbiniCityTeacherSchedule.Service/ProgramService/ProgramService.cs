using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.ProgramService
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramData _db;

        public ProgramService(IProgramData db)
        {
            _db = db;
        }

        public async  Task<ServiceResult> CreateProgram(CreateProgramDTO program) 
        {
            program.ProgramName = program.ProgramName!.Trim().ToUpper();
            Console.WriteLine(program.ProgramName);
            var isExist = await _db.IsNameExist(program.ProgramName!);
            if (isExist) {
                return ServiceResult.Fail("Program name already exists");
            }
            await _db.Create(program);
            return ServiceResult.Ok("Program Created Successfully");

        }

        public async Task<ServiceResult> DeleteProgram(int id )
        {
            var programs = await _db.Get(id);
            if (programs == null) return ServiceResult.Fail($"Program with ID {id} not found");

            var isActive = await _db.ValidateActiveSemesterForProgram(id);
            if (isActive)
            {
                return ServiceResult.Fail("Cannot delete the program with active semester");
            }
            await _db.Delete(id);
            return ServiceResult.Ok("Deleted Successfully");
            
        }

        public async Task<ServiceResult<IEnumerable<Program>>> GetAllPrograms()
        {
            //gets data from db 
            var programs = await _db.GetAll();
            if (programs == null || !programs.Any()) {
                return ServiceResult<IEnumerable<Program>>.Fail("No programs found");
            }
            return ServiceResult<IEnumerable<Program>>.Ok(programs,"Programs retrieved successfully");
        }

        public async  Task<ServiceResult<Program>> GetProgramsById(int id)
        {
            var program =  await _db.Get(id);
            if (program == null) return ServiceResult<Program>.Fail($"Program with ID {id} not found");

            return ServiceResult<Program>.Ok(program, "Program found");  
        }

    }
}
