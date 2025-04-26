using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : Controller
    {  //injecting di services 
       
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }
        //Get all Program 
        [HttpGet]
        public async Task<IActionResult> GetPrograms() 
        {
            try {
                //gets datas from service 
               var result  = await _programService.GetAllPrograms();
                //checks if any record from service 
                if (!result.Success) 
                {  
                    return NotFound(result.Message);
                }
                //if not null then returns programs 

                return Ok(result.Data);
                 
            } catch (Exception ex) 
            { 
             Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs.");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramByID(int id) 
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid Id");
                }
               var result = await _programService.GetProgramsById(id);
                if(!result.Success)
                {
                    return NotFound(result.Message);
                }
                return Ok(result.Data);
            
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs by id"); 
            }
        
        }
        [HttpPost]
        public async Task<IActionResult> CreateProgram(CreateProgramDTO program)
        {
          
            try 
            { 
                if(!ModelState.IsValid )
                {
                  return BadRequest(ModelState);
                }

                if(program == null)
                {
                    return BadRequest("Program is required");
                }

                if (string.IsNullOrWhiteSpace(program.ProgramName))
                {
                    return BadRequest("Program name is required");
                }

                var result = await _programService.CreateProgram(program);
                if (!result.Success) 
                { 
                 return BadRequest(result.Message);
                }
                return Ok(result.Message);
            
               
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500,"Internal server error");
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                if(id == 0)
                {
                    return BadRequest("Invalid Id");
                }
               var result = await _programService.DeleteProgram(id);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


    }
}
