using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgramController : Controller
    {  //injecting di services 
        private readonly IProgramData _programData;

        public ProgramController(IProgramData programData)
        {
            _programData = programData;
        }
        //Get all Program 
        [HttpGet]
        public async Task<IActionResult> GetPrograms() 
        {
            try {
                //gets datas from service 
               var programs = await _programData.GetAll();
                //checks if any record from service 
                if (programs == null || !programs.Any()) 
                { 
                    //if null or empty returns no content 204 ststus 
                    return NoContent(); 
                }
                //if not null then returns programs 

                return Ok(programs);
                 
            } catch (Exception ex) 
            { 
             Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs.");
            }
           
        }

        //Get By Id controller 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramByID(int id) 
        {
            try
            {
               var program = await _programData.Get(id);
                if(program == null)
                {
                    return NotFound("Cannot find program");
                }
                return Ok(program);
            
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs by id"); 
            }
        
        }
        [HttpPost]
        public async Task<IActionResult> CreateProgram(LumbiniCityTeacherSchedule.Models.Models.Program program)
        {
            try 
            { 
             if(program == null) 
                {
                    return BadRequest("Program Data is Required");
                }
                await _programData.Create(program);
                return Ok("SucessFully Created");
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
                var program = await _programData.Get(id);
                if (program == null) 
                {
                    return NotFound($"The id:{id} doesnot exist");
                }

                var isActive = await _programData.ValidateActiveSemesterForProgram(id);
                if (isActive) 
                {
                    ModelState.AddModelError("ProgramId", "Cannot Delete while semester is active");
                    return BadRequest(ModelState);
                }
                await _programData.Delete(id);
                return Ok(new {Message = "Program deleted succesfully"});
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


    }
}
