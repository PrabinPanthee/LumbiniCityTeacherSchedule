using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterInstanceController : Controller
    {
        private readonly ISemesterInstanceData _db;
        private readonly ISemesterData _semData;
        private readonly ISemesterConfigData _semConfigData;


        public SemesterInstanceController(ISemesterInstanceData db, ISemesterData semData, ISemesterConfigData semConfigData)
        {
            _db = db;
            _semData = semData;
            _semConfigData = semConfigData;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSemesterInstance()
        {
            try {
                var Result = await _db.GetAll();
                if (Result == null)
                {
                    return NoContent();
                }

                return Ok(Result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{SemesterId}")]

        public async Task<IActionResult>GetActiveBySemesterById(int SemesterId) 
        {
            try {
                var semester = await _semData.Get(SemesterId);

                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }

                var result = await _db.GetActiveInstanceBySemesterId(SemesterId);
                if (result == null)
                {
                    ModelState.AddModelError("No Active Semester", "There is no active semester currently");
                    return BadRequest(ModelState);

                }
                return Ok(result);
            } 
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateSemesterInstanceDto dto)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var semester = await _semData.Get(dto.SemesterId);

                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }

                var config = await _semConfigData.Get(dto.SemesterId);
                if(config == null) 
                {
                    ModelState.AddModelError("SemesterId", "Need Configuration to Create Active semester");
                    return BadRequest(ModelState);
                }

                var SemActive = await _db.GetActiveInstanceBySemesterId(dto.SemesterId);
                if(SemActive != null)
                {
                    ModelState.AddModelError("SemesterId", "Cannot activate semester: Semester is Active Already");
                    return BadRequest(ModelState);
                }

                await _db.Create(dto);
                return Ok("Created Succesfully");
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        
        }

        [HttpPut("{SemesterInstanceId}")]
        public async Task<IActionResult> UpdateInstance(int SemesterInstanceId, [FromBody] UpdateSemesterInstanceDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var instance = await _db.Get(SemesterInstanceId);
                if (instance?.SemesterStatus != "active")
                {
                    ModelState.AddModelError("Already Completed", "Cannot Update Already semester: Semester is Active");
                    return BadRequest(ModelState);
                }

                
                if (updateDto.EndDate.Date <= instance.StartDate.Date) {

                    ModelState.AddModelError("Date", "Invalid date must be greater than start date");
                    return BadRequest(ModelState);

                }
                await _db.Update(SemesterInstanceId, updateDto);

                return Ok("Updated succesfully");


            }

            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }


    }
}
