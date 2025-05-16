using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.ViewModel;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Service.SubjectService;
using LumbiniCityTeacherSchedule.Service.TeacherAssignmentService;
using LumbiniCityTeacherSchedule.Service.TeacherService;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LumbiniCityTeacherScheduleMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherAssignmentController : Controller
    {
        private readonly ITeacherAssignmentService _db;
        private readonly IProgramService _programService;
        private readonly ISemesterService _semesterService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherWithAvailabilityService _teacherWithAvailabilityService;


        public TeacherAssignmentController(ITeacherAssignmentService db, IProgramService programService, ISemesterService semesterService, ISubjectService subjectService, ITeacherWithAvailabilityService teacherWithAvailabilityService)
        {
            _db = db;
            _programService = programService;
            _semesterService = semesterService;
            _subjectService = subjectService;
            _teacherWithAvailabilityService = teacherWithAvailabilityService;
        }

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        var programsResult = await _programService.GetAllPrograms();
        //        if (!programsResult.Success || programsResult.Data == null)
        //        {
        //            ViewBag.Error = programsResult.Message;
        //            return View();
        //        }

        //        var defaultProgram = programsResult.Data.First();
        //        var semestersResult = await _semesterService.GetSemesterByProgramId(defaultProgram.ProgramId);

        //        if (!semestersResult.Success || semestersResult.Data == null)
        //        {
        //            ViewBag.Error = semestersResult.Message;
        //            return View();
        //        }

        //        var defaultSemester = semestersResult.Data.First();
        //        var assignmentsResult = await _db.GetAllTeacherAssignmentBySemesterId(defaultSemester.SemesterId);

        //        ViewBag.Programs = programsResult.Data;
        //        ViewBag.Semesters = semestersResult.Data;
        //        ViewBag.SelectedProgramId = defaultProgram.ProgramId;
        //        ViewBag.SelectedSemesterId = defaultSemester.SemesterId;

        //        return View(assignmentsResult.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine(ex.Message);
        //        ViewBag.Error = "Error loading teacher assignment data.";
        //        return View();
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new TeacherAssignmentViewModel();

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

                var semestersResult = await _semesterService.GetSemesterByProgramId(defaultProgram.ProgramId);
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

                var assignmentsResult = await _db.GetAllTeacherAssignmentBySemesterId(defaultSemester.SemesterId);
                if (assignmentsResult.Success && assignmentsResult.Data != null)
                {
                    viewModel.Assignments = assignmentsResult.Data.ToList();
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

        [HttpGet]
        public async Task<IActionResult> GetAssignments(int semesterId)
        {
            try
            {
                var result = await _db.GetAllTeacherAssignmentBySemesterId(semesterId);
                if (!result.Success)
                {
                    return PartialView("_AssignmentTable", Enumerable.Empty<JoinedTeacherAssignmentBySubjectDTO>());
                }

                return PartialView("_AssignmentTable", result.Data);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return PartialView("_AssignmentTable", Enumerable.Empty<JoinedTeacherAssignmentBySubjectDTO>());
            }
        }
        
        [HttpGet]
        [Authorize(Roles =SD.Role_Admin)]
        public async Task<IActionResult> Assign(int subjectId)
        {
            try
            {
                // Fetch the subject details, including name
                var subject = await _subjectService.GetById(subjectId)!;
                if (!subject.Success)
                {
                    ViewBag.Error = subject.Message;
                    return View();
                }
                var data = subject.Data;

                // Fetch available teachers for this subject
                var teachersResult = await _teacherWithAvailabilityService.GetAll();
                if (!teachersResult.Success || teachersResult.Data == null)
                {
                    ViewBag.Error = teachersResult.Message;
                    return View();
                }

                // Prepare the ViewModel for the Assign view
                var assignViewModel = new AddTeacherAssignmentViewModel
                {
                    SubjectId = subjectId,
                    SubjectName = data!.SubjectName,// Add the subject name
                    Assignment = teachersResult.Data.ToList()
                };

                return View(assignViewModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
        public async Task<IActionResult> Assign(AddTeacherAssignmentViewModel assign)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Reload teacher list if validation fails
                    var teacherResult = await _teacherWithAvailabilityService.GetAll();
                    assign.Assignment = teacherResult.Data?.ToList() ?? new();
                    return View(assign);
                }

                var teacher = await _teacherWithAvailabilityService.GetById(assign.SelectedTeacherId!.Value);
                if (!teacher.Success)
                {
                    ModelState.AddModelError(string.Empty, teacher.Message!);
                    var teacherResult = await _teacherWithAvailabilityService.GetAll();
                    assign.Assignment = teacherResult.Data?.ToList() ?? new();
                    return View(assign);
                }

                // Validation (e.g., check if teacher is available, etc.)
                var result = await _db.AssignTeacher(assign.SubjectId, teacher.Data!);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message!);

                    var teacherResult = await _teacherWithAvailabilityService.GetAll();
                    assign.Assignment = teacherResult.Data?.ToList() ?? new();
                    return View(assign);
                }

                // Redirect to the index or another view as needed
                TempData["Success"] = "Teacher assigned successfully";
                return RedirectToAction("Index"); // Redirect to the main index or another appropriate action
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                TempData["Error"] = "An error occurred while assigning the teacher.";
                return RedirectToAction("Assign", new { assign.SubjectId });
            }
        }



    }
}
