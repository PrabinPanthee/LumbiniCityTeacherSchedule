using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class SemesterController : Controller
    {
        private readonly ISemesterService _semesterService;


        public SemesterController(ISemesterService semesterService, ISemesterData semesterData)
        {
            _semesterService = semesterService;

        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                //gets datas from service 
                var result = await _semesterService.GetSemesterByProgramId(id);
                //checks if any record from service 
                if (!result.Success)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                //if not null then returns programs 

                return View(result.Data);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server Error while retrieving semester data by program";
                return View();
            }

        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var dto = new SemesterDTO { ProgramId = id };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SemesterDTO semester)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(semester);
                }

                var result = await _semesterService.Create(semester);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(semester);
                }
                TempData["Success"] = "Semester created successfully!";
                return RedirectToAction(nameof(Index), new { id = semester.ProgramId });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while creating semester";
                return View();
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int semesterId)
        {
            try
            {
                if (semesterId == 0)
                {
                    return Json(new { success = false, message = "Invalid semester Id" });
                }

                var result = await _semesterService.Delete(semesterId);
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



        //Get By Id controller




    }
}

