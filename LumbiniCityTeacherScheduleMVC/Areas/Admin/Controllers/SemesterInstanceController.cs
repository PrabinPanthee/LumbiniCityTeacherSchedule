using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.ViewModel;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Service.SemesterInstanceService;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SemesterInstanceController : Controller
    {
        private readonly ISemesterInstanceService _db;
        private readonly ISemesterService _semesterService;
        private readonly IProgramService _programService;

        public SemesterInstanceController(ISemesterInstanceService db, ISemesterService semesterService, IProgramService programService)
        {
            _db = db;
            _semesterService = semesterService;
            _programService = programService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _db.GetAllActiveSemesterInstance();
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(result.Data);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server Error while retrieving";
                return View();
            }
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {  
            try
            {
                var programData = await _programService.GetAllPrograms();
                if (!programData.Success)
                {
                    ViewBag.Error = programData.Message;
                    return View();
                }
                var programList = programData.Data!.ToList();
                var VM = new CreateSemesterInstanceVM
                {
                    ProgramList = programList
                };
                return View(VM);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server Error while creating";
                return View();
            }
       
        }
        [HttpGet]
        public async Task<IActionResult> GetSemesters(int programId)
        {
            try
            {
                if (programId == 0)
                {
                    return Json(new { success = false, message = "Invalid program Id" });
                }
                var semesters = await _semesterService.GetSemesterByProgramId(programId);
                if (!semesters.Success)
                {
                    return Json(new { success = false, message = semesters.Message });
                }

                return Json(new { success = true, data = semesters.Data, message = semesters.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Internal server error while fetching semester data" });
            }
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Create(CreateSemesterInstanceVM VM)
        {
            try
            {

                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                var createSemDto = new CreateSemesterInstanceDto 
                {
                 SemesterId = VM.SemesterId,
                 StartDate = dateOnly
                };
                var result = await _db.Create(createSemDto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    var programList = await _programService.GetAllPrograms();
                    VM.ProgramList = programList.Data!.ToList();
                    return View(VM);
                }
                TempData["Success"] = "Semester Activated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                TempData["Error"] = "An error occurred while assigning the teacher.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Complete(int SemesterInstanceId)
        {
            try
            {
                DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);
                var dto = new UpdateSemesterInstanceDto
                {
                    SemesterInstanceId = SemesterInstanceId,
                    EndDate = dateOnly
                };
                Console.WriteLine(dto.EndDate);
                var result = await _db.Update(dto);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, message = "Semester marked as completed." });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Internal server error." });
            }
        }
    }
}
