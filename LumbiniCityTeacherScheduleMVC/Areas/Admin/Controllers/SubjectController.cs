using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Service.SubjectService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _db;
        public SubjectController(ISubjectService db, ISemesterService semesterService)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int semesterId, int programId)
        {
            try
            {
                var result = await _db.GetAllBySemesterId(semesterId);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    ViewBag.ProgramId = programId;
                    ViewBag.SemesterId = semesterId;
                    return View();
                }
                ViewBag.ProgramId = programId;
                ViewBag.SemesterId = semesterId;
                return View(result.Data);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server Error while retrieving subject data by program";
                return View();
            }

        }
        [HttpGet]
        public IActionResult Create(int semesterId, int programId)
        {

            var dto = new CreateSubjectDTO { SemesterId = semesterId, ProgramId = programId };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubjectDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var result = await _db.Create(dto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(dto);
                }
                TempData["Success"] = " Subject created successfully!";
                return RedirectToAction(nameof(Index), new { semesterId = dto.SemesterId, programId = dto.ProgramId });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server while creating subject";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int subjectId, int programId)
        {
            try
            {
                var result = await _db.GetById(subjectId);
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                var data = result.Data;

                var dto = new UpdateSubjectDto
                {
                    ProgramId = programId,
                    SubjectId = data!.SubjectId,
                    SemesterId = data.SemesterId,
                    SubjectName = data!.SubjectName,
                    SubjectCode = data!.SubjectCode,
                };

                return View(dto);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server while updating subject";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateSubjectDto subjectDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(subjectDto);
                }

                var result = await _db.Update(subjectDto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(subjectDto);
                }
                TempData["Success"] = "Configuration created successfully!";
                return RedirectToAction(nameof(Index), new { semesterId = subjectDto.SemesterId, programId = subjectDto.ProgramId });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while updating semester config data";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int subjectId)
        {
            try
            {
                if (subjectId == 0)
                {
                    return Json(new { success = false, message = "Invalid semester Id" });
                }

                var result = await _db.Delete(subjectId);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }
                return Json(new { success = true, message = result.Message });


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Internal server error while deleting semester" });
            }
        }

    }
}
