using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterScheduleConfigController : Controller
    {
        private readonly ISemesterConfigData _db;
        private readonly ISemesterData _semData;
        



        public SemesterScheduleConfigController(ISemesterConfigData db, ISemesterData semData)
        {
            _db = db;
            _semData = semData;
            
        }

        [HttpGet("{SemesterId}")]
        public async Task<IActionResult> GetBySemesterId(int SemesterId)
        {
            try
            {
                var semester = await _semData.Get(SemesterId);

                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }

                var configData = await _db.GetBySemesterId(SemesterId);
                if (configData == null)
                {
                    return NotFound("Cannot find configuration for semester");
                }

                return Ok(configData);
            }
            catch
            (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while retrieving programs by id");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateConfig(SemesterScheduleConfig config)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var semester = await _semData.Get(config.SemesterId);

                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }

                var ExistingConfig = await _db.GetBySemesterId(config.SemesterId);
                if (ExistingConfig != null)
                {
                    ModelState.AddModelError("SemesterId", "Config for this sem already exist");
                    return BadRequest(ModelState);
                }


                if (config.TotalClasses < 1 || config.TotalClasses > 6)
                {
                    ModelState.AddModelError("TotalClasses", "TotalClasses Must be between 1 and 6");
                    return BadRequest(ModelState);
                }

                if (config.BreakAfterPeriod.HasValue)
                {
                    if (config.BreakAfterPeriod <= 1 || config.BreakAfterPeriod >= config.TotalClasses)
                    {
                        ModelState.AddModelError("BreakAfterPeriod", "BreakAfterPeriod must be >= 1 and < TotalClasses.");
                        return BadRequest(ModelState);
                    }
                }

                
                return Ok("SucessFully Created");
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error ");
            }
        }

        [HttpPut("{ConfigId}")]
        public async Task<IActionResult> UpdateConfig(int ConfigId, [FromBody] UpdateSemesterScheduleConfigDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ConfigExists = await _db.Get(ConfigId);
                if (ConfigExists == null) 
                {
                  return NotFound("Cannot find Existing Configuration");
                }
                bool isSemesterActive = await _db.ValidateActiveSemesterForConfig(ConfigId);
                if (isSemesterActive) 
                {
                    ModelState.AddModelError(nameof(ConfigId), "Cannot Update while semester is active");
                    return BadRequest(ModelState);
                }

                if (dto.TotalClasses < 1 || dto.TotalClasses > 6)
                {
                    ModelState.AddModelError(nameof(dto.TotalClasses), "TotalClasses must be between 1 and 6.");
                    return BadRequest(ModelState);
                }

                if (dto.BreakAfterPeriod.HasValue)
                {
                    if (dto.BreakAfterPeriod <= 1 || dto.BreakAfterPeriod >= dto.TotalClasses)
                    {
                        ModelState.AddModelError("BreakAfterPeriod", "BreakAfterPeriod must be >= 1 and < TotalClasses.");
                        return BadRequest(ModelState);
                    }
                }

                await _db.Update(ConfigId, dto.TotalClasses,dto.BreakAfterPeriod);
                return Ok("Configuration successfully updated.");

            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error ");
            }
        }


            
    }
}
