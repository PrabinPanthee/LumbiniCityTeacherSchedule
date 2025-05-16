using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProgramController : Controller
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }
        //Get all Program 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                //gets datas from service 
                var result = await _programService.GetAllPrograms();
                //checks if any record from service 
                if (!result.Success)
                {
                    return NotFound(result.Message);
                }
                //if not null then returns programs 

                return View(result.Data);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while retrieving programs";
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id == 0)
                {
                    ViewBag.Error = "Invalid Program ID provided.";
                    return View();
                }
                var result = await _programService.GetProgramsById(id);
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
                ViewBag.Error = "Internal server error while retrieving programs";
                return View();
            }

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProgramDTO program)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(program);
                }

                var result = await _programService.CreateProgram(program);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(program);
                }
                TempData["Success"] = "Program created successfully!";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while creating programs";
                return View();
            }

        }

        //[HttpPost]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    try
        //    {
        //        if (id == 0)
        //        {
        //            TempData["Error"] = "Invalid Program ID provided.";
        //            return RedirectToAction(nameof(Index));
        //        }

        //        var result = await _programService.DeleteProgram(id);
        //        if (!result.Success)
        //        {
        //            TempData["Error"] = result.Message;
        //            return RedirectToAction(nameof(Index));
        //        }

        //        TempData["Success"] = result.Message;
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine(ex.Message);
        //        TempData["Error"] = "Internal server error while deleting the program.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Json(new { success = false, message = "Invalid Program Id." });

                }
                var result = await _programService.DeleteProgram(id);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }
                return Json(new { success = true, message = result.Message });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Internal server error while deleting" });
            }
        }
    }
}
