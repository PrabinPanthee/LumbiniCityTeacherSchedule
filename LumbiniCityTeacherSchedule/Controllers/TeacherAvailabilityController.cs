//using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAvailabilityData;
//using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherData;
//using Microsoft.AspNetCore.Mvc;

//namespace LumbiniCityTeacherSchedule.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TeacherAvailabilityController : Controller
//    {
//        private readonly ITeacherAvailabilityData _db;
//        private readonly ITeacherData _tdata;

//        public TeacherAvailabilityController(ITeacherAvailabilityData db, ITeacherData tdata)
//        {
//            _db = db;
//            _tdata = tdata;
//        }

//        [HttpGet("{TeacherId}")]
//        public async Task<IActionResult> GetByTeacherId(int TeacherId)
//        {
//            try
//            {
//                var isExist = await _tdata.GetById(TeacherId);
//                if (isExist == null)
//                {
//                    ModelState.AddModelError("Invalid Id",$"{TeacherId} doesn't exist");
//                    return BadRequest(ModelState);
//                }

//                var teacherAvailability = await _db.GetByTeacherId(TeacherId);
//                return Ok(teacherAvailability);
//            }
//            catch (Exception ex) 
//            { 
//             Console.Error.WriteLine(ex.Message);
//                return StatusCode(500,"Internal server error");
            
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create()
//        {

//        }
//    }
//}
