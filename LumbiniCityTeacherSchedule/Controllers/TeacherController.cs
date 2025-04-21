//using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherData;
//using LumbiniCityTeacherSchedule.Models.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace LumbiniCityTeacherSchedule.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TeacherController : Controller
//    {
//        private readonly ITeacherData _db;

//        public TeacherController(ITeacherData db)
//        {
//            _db = db;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllTeacherData() 
//        {
//            try
//            {
//                var result = await _db.GetAll();
//                if (result == null) 
//                {
//                    ModelState.AddModelError("Null Data", "No data available");
//                    return BadRequest(ModelState);
//                }

//                return Ok(result);
//            }
//            catch (Exception ex) 
//            { 
//             Console.Error.WriteLine(ex.Message);
//             return StatusCode(500,"Internal Server Error");
//            }

//        }

//        [HttpGet("{TeacherId}")]
//        public async Task<IActionResult> GetById(int TeacherId) 
//        {
//            try 
//            {
//                var result = await _db.GetById(TeacherId);
//                if (result == null) 
//                {
//                    ModelState.AddModelError("Invalid Id", "Invalid Teacher Id");
//                    return BadRequest(ModelState);
//                }
//                return Ok(result);
//            } 
//            catch (Exception ex) 
//            {
//                Console.Error.WriteLine(ex.Message);
//                return StatusCode(500, "Internal Server Error");
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(Teacher teacher)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                if (teacher.NumberOfClasses < 1 || teacher.NumberOfClasses > 6)
//                {
//                    ModelState.AddModelError("RangeError", "Must be greater than 1 and less tha 6");
//                    return BadRequest(ModelState);
//                }
//                await _db.Create(teacher);
//                return Ok("Created Successfully");
            
//            } 
//            catch (Exception ex) 
//            {
//                Console.Error.WriteLine(ex.Message);
//                return StatusCode(500, "Internal Server Error");
//            }
            
//        }

//        [HttpPost("{TeacherId}")]
//        public async Task<IActionResult> Update(int TeacherId, Teacher teacher)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);

//                }

//                var isIdValid = await _db.GetById(TeacherId);
//                if (isIdValid == null)
//                {
//                    ModelState.AddModelError("InvalidId", $"Teacher with Id:{TeacherId} doesn't exist");
//                    return BadRequest(ModelState);
//                }

//                if (teacher.NumberOfClasses < 1 || teacher.NumberOfClasses > 6)
//                {
//                    ModelState.AddModelError("RangeError", "Must be greater than 1 and less tha 6");
//                    return BadRequest(ModelState);
//                }

//                var activeNoOfClasses = await _db.GetNumberOfAssignedTeacherInActiveSchedule(TeacherId);

//                if (teacher.NumberOfClasses < activeNoOfClasses)
//                {
//                    ModelState.AddModelError("Numberofclasses", $"Cant be less then active no of classes {activeNoOfClasses}");
//                    return BadRequest(ModelState);
//                }
//                await _db.Update(TeacherId, teacher);
//                return Ok("Updated succesfully");
//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine(ex.Message);
//                return StatusCode(500, "Internal Server Error");
//            }
//        }

//        [HttpDelete("{TeacherId}")]
//        public async Task<IActionResult> Delete(int TeacherId)
//        {
//            try
//            {
//                var isIdValid = await _db.GetById(TeacherId);
//                if (isIdValid == null)
//                {
//                    ModelState.AddModelError("InvalidId", $"Teacher with Id:{TeacherId} doesn't exist");
//                    return BadRequest(ModelState);
//                }

//                var isLinkedToActiveSemester = await _db.IsLinkedToActiveSemester(TeacherId);
//                if (isLinkedToActiveSemester)
//                {
//                    ModelState.AddModelError("Active Semester", "Cannot Update linked to Active Semester");
//                    return BadRequest(ModelState);
//                }
//                await _db.Delete(TeacherId);
//                return Ok("Deleted Successfully");

//            }
//            catch (Exception ex)
//            {
//                Console.Error.WriteLine(ex.Message);
//                return StatusCode(500, "Internal Server Error");
//            }
//        }
//    }
//}
