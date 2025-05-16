using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherWithAvailabilityController : Controller
    {
        
        private readonly IJoinedTeacherAndAvailabilityData _db;

        public TeacherWithAvailabilityController(IJoinedTeacherAndAvailabilityData db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeacherWithAvailability()
        {
            try
            {
                var result = await _db.GetAll();
                if (result == null)
                {
                    ModelState.AddModelError("Empty Result", "No data available");
                    return NotFound(ModelState);


                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Internal server error");

            }
        }
        [HttpGet("{TeacherId}")]

        public async Task<IActionResult> GetById(int TeacherId)
        {
            try
            {
                if (TeacherId == 0)
                {
                    ModelState.AddModelError("Invalid Input", "Invalid Id");
                    return BadRequest(ModelState);

                }

                var result = await _db.GetById(TeacherId);
                if(result == null)
                {
                    ModelState.AddModelError("Empty Result", "No data available");
                    return NotFound(ModelState);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]TeacherWithAvailability teacherWithAvailability)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (teacherWithAvailability.NumberOfClasses < 1 || teacherWithAvailability.NumberOfClasses > 6)
                {
                    ModelState.AddModelError("RangeError", "Must be greater than 1 and less tha 6");
                    return BadRequest(ModelState);
                }

                var minStartTime = new TimeOnly(6, 30);
                var maxEndTime = new TimeOnly(12, 00);
                if(teacherWithAvailability.StartTime < minStartTime)
                {
                    ModelState.AddModelError("StartTime", "Start time cannot be earlier than 6:30 AM.");
                    return BadRequest(ModelState);
                }

                if(teacherWithAvailability.EndTime > maxEndTime)
                {
                    ModelState.AddModelError("EndTime", "End time cannot be late than 12:00 AM.");
                    return BadRequest(ModelState);
                }

                if(teacherWithAvailability.EndTime <= teacherWithAvailability.StartTime)
                {
                    ModelState.AddModelError("EndTime", "End time Must be late than StartTime.");
                    return BadRequest(ModelState);
                }

                int requiredMinutes = teacherWithAvailability.NumberOfClasses * 50;
                var requiredEndTime = teacherWithAvailability.StartTime.AddMinutes(requiredMinutes);
                if (teacherWithAvailability.EndTime < requiredEndTime)
                {
                    ModelState.AddModelError("EndTime",
                        $"The available time is too short for {teacherWithAvailability.NumberOfClasses} classes. " +
                        $"Starting at {teacherWithAvailability.StartTime}, the end time must be at least {requiredEndTime}.");
                    return BadRequest(ModelState);
                }

              
                return Ok("Created SuccessFully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{TeacherId}")]

        public async Task<IActionResult> Update(int TeacherId, [FromBody] TeacherWithAvailability teacherWithAvailability)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var isLinkedToActiveSemester = await _db.IsLinkedToActiveSemester(TeacherId);
                if (isLinkedToActiveSemester)
                {
                    ModelState.AddModelError("Active Semester", "Cannot Update linked to Active Semester");
                    return BadRequest(ModelState);
                }

                if (TeacherId == 0)
                {
                    ModelState.AddModelError("Invalid Input", "Invalid Id");
                    return BadRequest(ModelState);
                }

                var isExist = await _db.GetById(TeacherId);
                if (isExist == null)
                {
                    ModelState.AddModelError("TeacherId", "Invalid Teacher Id");
                    return NotFound(ModelState);
                }

                if (teacherWithAvailability.NumberOfClasses < 1 || teacherWithAvailability.NumberOfClasses > 6)
                {
                    ModelState.AddModelError("RangeError", "Must be greater than 1 and less tha 6");
                    return BadRequest(ModelState);
                }

                int activeAssignedTeacherClasses = await _db.GetNumberOfAssignedTeacherInActiveSchedule(TeacherId);
                if(teacherWithAvailability.NumberOfClasses <  activeAssignedTeacherClasses)
                {
                    ModelState.AddModelError("NumberOfClasses", $"NumberOfClasses Cant be less than {activeAssignedTeacherClasses} active classes");
                    return BadRequest(ModelState);
                }


                var minStartTime = new TimeOnly(6, 30);
                var maxEndTime = new TimeOnly(12, 00);
                if (teacherWithAvailability.StartTime < minStartTime)
                {
                    ModelState.AddModelError("StartTime", "Start time cannot be earlier than 6:30 AM.");
                    return BadRequest(ModelState);
                }

                if (teacherWithAvailability.EndTime > maxEndTime)
                {
                    ModelState.AddModelError("EndTime", "End time cannot be late than 12:00 AM.");
                    return BadRequest(ModelState);
                }

                if (teacherWithAvailability.EndTime <= teacherWithAvailability.StartTime)
                {
                    ModelState.AddModelError("EndTime", "End time Must be late than StartTime.");
                    return BadRequest(ModelState);
                }

                int requiredMinutes = teacherWithAvailability.NumberOfClasses * 50;
                var requiredEndTime = teacherWithAvailability.StartTime.AddMinutes(requiredMinutes);
                if (teacherWithAvailability.EndTime < requiredEndTime)
                {
                    ModelState.AddModelError("EndTime",
                        $"The available time is too short for {teacherWithAvailability.NumberOfClasses} classes. " +
                        $"Starting at {teacherWithAvailability.StartTime}, the end time must be at least {requiredEndTime}.");
                    return BadRequest(ModelState);
                }

               
                return Ok("Updated Successfully");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{TeacherId}")]
        public async Task<IActionResult> Delete(int TeacherId)
        {
            try
            {
                var isIdValid = await _db.GetById(TeacherId);
                if (isIdValid == null)
                {
                    ModelState.AddModelError("InvalidId", $"Teacher with Id:{TeacherId} doesn't exist");
                    return BadRequest(ModelState);
                }

                var isLinkedToActiveSemester = await _db.IsLinkedToActiveSemester(TeacherId);
                if (isLinkedToActiveSemester)
                {
                    ModelState.AddModelError("Active Semester", "Cannot Update linked to Active Semester");
                    return BadRequest(ModelState);
                }
                await _db.Delete(TeacherId);
                return Ok("Deleted Successfully");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
