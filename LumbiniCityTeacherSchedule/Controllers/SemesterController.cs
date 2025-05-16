using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : Controller
    {
        private readonly ISemesterData _semesterData;

        public SemesterController(ISemesterData semesterData)
        {
            _semesterData = semesterData;
        }

        //GetsALl the list of semester
        [HttpGet]
        public async Task<IActionResult> GetSemester()
        {
            try
            {
                //gets datas from service 
                var semester = await _semesterData.GetAll();
                //checks if any record from service 
                if (semester == null || !semester.Any())
                {
                    //if null or empty returns no content 204 ststus 
                    return NoContent();
                }
                //if not null then returns programs 

                return Ok(semester);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs by id");
            }

        }

        //Get By Id controller 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterByID(int id)
        {
            try
            {
                var semester = await _semesterData.Get(id);
                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }
                return Ok(semester);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs by id");
            }

        }
        //Create Semester
        [HttpPost]
        public async Task<IActionResult> CreateSemester(LumbiniCityTeacherSchedule.Models.Models.Semester semester)
        {
            try
            {
                if (semester == null)
                {
                    return BadRequest("Semester Data is Required");
                }
                
                return Ok("SucessFully Created");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(int id)
        {
            try
            {
                var semester = await _semesterData.Get(id);
                if (semester == null)
                {
                    return NotFound($"The id:{id} doesnot exist");
                }
                await _semesterData.Delete(id);
                return Ok(new { Message = "Program deleted succesfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
