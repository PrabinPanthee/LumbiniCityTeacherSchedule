using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using LumbiniCityTeacherSchedule.Service.TeacherService;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Controllers
{
    public class TeacherWithAvailabilityController : Controller
    {
        private readonly ITeacherWithAvailabilityService _db;

        public TeacherWithAvailabilityController(ITeacherWithAvailabilityService db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _db.GetAll();
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherWithAvailabilityDTO teacher)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(teacher);
                }
                var result = await _db.Create(teacher);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(teacher);
                }
                TempData["Success"] = "Teacher created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while creating teacher";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int TeacherId)
        {
            try
            {
                var result = await _db.GetById(TeacherId);
                if (!result.Success) 
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                var data= result.Data!;
                var ViewData = new UpdateTeacherWithAvailabilityDto
                {
                    TeacherId = data.TeacherId,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    NumberOfClasses = data.NumberOfClasses,
                    StartTime = data.StartTime,
                    EndTime = data.EndTime


                };
                return View(ViewData);
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal Server while updating teacher";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateTeacherWithAvailabilityDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var result = await _db.Update(dto);
                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);
                    return View(dto);
                }
                TempData["Success"] = "Teacher updated successfully!";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = "Internal server error while updating teacher data";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int TeacherId)
        {
            try
            {
                if (TeacherId == 0)
                {
                    return Json(new { success = false, message = "Invalid teacher Id" });
                }

                var result = await _db.Delete(TeacherId);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }
                return Json(new { success = true, message = result.Message });


            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return Json(new { success = false, message = "Internal server error while deleting teacher" });
            }
        }

    }
}
