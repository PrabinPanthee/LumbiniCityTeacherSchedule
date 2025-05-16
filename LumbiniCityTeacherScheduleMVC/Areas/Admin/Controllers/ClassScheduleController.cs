using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.ViewModel;
using LumbiniCityTeacherSchedule.Service.ClassScheduleService;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{   [Area("Admin")]
    
    public class ClassScheduleController : Controller
    {
        
        private readonly IClassScheduleService _data;
        private readonly IProgramService _programService;
        private readonly ISemesterService _semesterService;

        public ClassScheduleController(IClassScheduleService data, IProgramService programService, ISemesterService semesterService)
        {
            _data = data;
            _programService = programService;
            _semesterService = semesterService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = new ClassScheduleVM();

            try
            {
                var programsResult = await _programService.GetAllPrograms();
                if (!programsResult.Success || programsResult.Data == null)
                {
                    viewModel.ErrorMessage = programsResult.Message;
                    return View(viewModel);
                }

                viewModel.Programs = programsResult.Data.ToList();
                var defaultProgram = programsResult.Data.FirstOrDefault();
                if (defaultProgram == null)
                {
                    viewModel.ErrorMessage = "No programs found.";
                    return View(viewModel);
                }

                var semestersResult = await _semesterService.GetAllActiveSemesterByProgramId(defaultProgram.ProgramId);
                if (!semestersResult.Success || semestersResult.Data == null)
                {
                    viewModel.ErrorMessage = semestersResult.Message;
                    return View(viewModel);
                }

                viewModel.Semesters = semestersResult.Data.ToList();
                var defaultSemester = semestersResult.Data.FirstOrDefault();
                if (defaultSemester == null)
                {
                    viewModel.ErrorMessage = "No semesters found for selected program.";
                    return View(viewModel);
                }

                var classScheduleResult = await _data.GetAllBySemesterId(defaultSemester.SemesterId);
                if (classScheduleResult.Success && classScheduleResult.Data != null)
                {
                    viewModel.classSchedule = classScheduleResult.Data.ToList();
                }

                viewModel.SelectedProgramId = defaultProgram.ProgramId;
                viewModel.SelectedSemesterId = defaultSemester.SemesterId;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                viewModel.ErrorMessage = "Error loading teacher assignment data.";
            }

            return View(viewModel);
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
                var semesters = await _semesterService.GetAllActiveSemesterByProgramId(programId);
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

        [HttpGet]
        public async Task<IActionResult> GetClassSchedule(int semesterId)
        {
            try
            {
                var result = await _data.GetAllBySemesterId(semesterId);
                if (!result.Success)
                {
                    return PartialView("_ScheduleTable", Enumerable.Empty<JoinedClassScheduleDataDTO>());
                }

                return PartialView("_ScheduleTable", result.Data);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return PartialView("_ScheduleTable", Enumerable.Empty<JoinedClassScheduleDataDTO>());
            }
        }

        [HttpGet]
        [Authorize(SD.Role_Admin)]
        public async Task<IActionResult> Create(int semesterInstanceId)
        {
            try
            {
                if (semesterInstanceId == 0)
                {
                    TempData["Error"] = "Invalid Semester Instance ID";
                    return RedirectToAction("Index", "SemesterInstance");
                }

                var result = await _data.GenerateClassSchedule(semesterInstanceId); // Your scheduling logic
                if (!result.Success)
                {
                    TempData["Error"] = result.Message;
                }
                else
                {
                    TempData["Success"] = "Schedule created successfully!";
                }

                return RedirectToAction("Index", "SemesterInstance");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                TempData["Error"] = "Internal server error while creating schedule.";
                return RedirectToAction("Index");
            }
        }
    }
}
