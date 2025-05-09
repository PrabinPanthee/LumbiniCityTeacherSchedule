using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.ConfigurationService;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly ISemesterScheduleConfigService _db;

        public ConfigurationController(ISemesterScheduleConfigService db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int semesterId,int programId)
        {
            try
            {
                var result = await _db.GetSemesterScheduleConfig(semesterId);
                if (!result.Success) 
                {
                    ViewBag.Error = result.Message;
                    ViewBag.ProgramId = programId;
                    return View();
                }
                ViewBag.ProgramId = programId;
                Console.WriteLine(ViewBag.ProgramId);
                return View(result.Data);

            } 
            catch 
            (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server Error while retrieving semester data by program";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create(int semesterId,int programId)
        {
            var dto = new CreateSemesterScheduleConfigDTO { SemesterId = semesterId,ProgramId = programId};
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSemesterScheduleConfigDTO configDto)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(configDto);
                }

                var result = await _db.Create(configDto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(configDto);
                }
                TempData["Success"] = "Configuration created successfully!";
                return RedirectToAction(nameof(Index), new { semesterId = configDto.SemesterId, programId = configDto.ProgramId });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while creating semester config";
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Update(int configId,int programId)
        {
            try
            {
                var result = await _db.Get(configId);
                if (!result.Success) 
                { 
                 ViewBag.Error = result.Message;
                  return View();
                }
                var data = result.Data;
                var dtoData = new UpdateSemesterScheduleConfigDto
                {
                    ConfigId = data!.ConfigId,
                    ProgramId = programId,
                    SemesterId = data!.SemesterId,
                    TotalClasses = data!.TotalClasses,
                    BreakAfterPeriod = data!.BreakAfterPeriod

                };
                return View(dtoData);
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while retrieving semester config data";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSemesterScheduleConfigDto configDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(configDto);
                }

                var result = await _db.Update(configDto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(configDto);
                }
                TempData["Success"] = "Configuration created successfully!";
                return RedirectToAction(nameof(Index), new { semesterId = configDto.SemesterId, programId=configDto.ProgramId});

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while updating semester config data";
                return View();
            }
        }
    }
}
