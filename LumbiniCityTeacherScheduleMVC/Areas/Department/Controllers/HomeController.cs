using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Service.ClassScheduleService;
using LumbiniCityTeacherScheduleMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LumbiniCityTeacherScheduleMVC.Areas.Department.Controllers
{
    [Area("Department")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClassScheduleService _classScheduleService;

        public HomeController(ILogger<HomeController> logger, IClassScheduleService classScheduleService)
        {
            _logger = logger;
            _classScheduleService = classScheduleService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _classScheduleService.GetAllDataForPDF();

                if (!result.Success || result.Data == null || !result.Data.Any())
                {
                    ViewBag.Error = result.Message ?? "No schedule data available.";
                    return View(new List<ClassSchedulePDFDto>());
                }

                return View(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching schedule data.");
                ViewBag.Error = "An internal server error occurred.";
                return View(new List<ClassSchedulePDFDto>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
