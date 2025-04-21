using LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData;
using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TimeSlotData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using Microsoft.AspNetCore.Mvc;


namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassScheduleController : Controller
    {
        private readonly ISemesterInstanceData _semesterInstanceData;
        private readonly ISemesterConfigData _semesterConfigData;
        private readonly ITeacherAssignmentData _teacherAssignmentData;
        private readonly IJoinedTeacherAndAvailabilityData _joinedTeacherAndAvailabilityData;
        private readonly ISubjectData _subjectData;
        private readonly ISemesterData _semesterData;
        private readonly ITimeSlotData _timeSlotData;
        private readonly IClassScheduleData _classSchedule;

        public ClassScheduleController(ISemesterInstanceData semesterInstanceData, ISemesterConfigData semesterConfigData, ITeacherAssignmentData teacherAssignmentData, IJoinedTeacherAndAvailabilityData joinedTeacherAndAvailabilityData, ISubjectData subjectData, ISemesterData semesterData, ITimeSlotData timeSlotData, IClassScheduleData classSchedule)
        {
            _semesterInstanceData = semesterInstanceData;
            _semesterConfigData = semesterConfigData;
            _teacherAssignmentData = teacherAssignmentData;
            _joinedTeacherAndAvailabilityData = joinedTeacherAndAvailabilityData;
            _subjectData = subjectData;
            _semesterData = semesterData;
            _timeSlotData = timeSlotData;
            _classSchedule = classSchedule;
        }

        [HttpPost("{SemesterInstanceId}")]
        public async Task<IActionResult> GenerateClassSchedule(int SemesterInstanceId)
        {
            try
            {
                var semesterInstanceData = await _semesterInstanceData.Get(SemesterInstanceId);
                if (semesterInstanceData == null)
                {
                    ModelState.AddModelError("EmptyData", $"Cannot find semesterInstance data");
                    return BadRequest(ModelState);
                }
                var semesterData = await _semesterData.Get(semesterInstanceData!.SemesterId);
                if (semesterData == null)
                {
                    ModelState.AddModelError("Empty Data", $"Cannot find the semester data for Semester :{semesterData!.SemesterNumber}");
                    return BadRequest(ModelState);
                }
                var semesterConfigData = await _semesterConfigData.GetBySemesterId(semesterData.SemesterId);
                if (semesterConfigData == null)
                {
                    ModelState.AddModelError("Empty Data", $"Cannot find the semester config data for current semester");
                    return BadRequest(ModelState);
                }

                var timeSlotData = (await _timeSlotData.GetAllByConfigId(semesterConfigData.ConfigId))
                                     .Where(ts => ts.Type == "class")
                                    .OrderBy(ts => ts.TimeSlotId).ToList();
                if (timeSlotData.Count == 0)
                {
                    ModelState.AddModelError("No Slot Data", "Could not find slot data");
                    return BadRequest(ModelState);
                }

                var subjectData = await _subjectData.GetAllSubjectBySemesterId(semesterData.SemesterId);
                if (subjectData == null)
                {
                    ModelState.AddModelError("No Subject Data", $"Could not find Subject data for semester:{semesterData.SemesterNumber}");
                    return BadRequest(ModelState);
                }

                var teacherAssignmentData = await _teacherAssignmentData.GetAllBySemesterId(semesterData.SemesterId);
                if (teacherAssignmentData == null)
                {
                    ModelState.AddModelError("Assignment", $"No teacher are assigned for {semesterData.SemesterNumber}");
                    return BadRequest(ModelState);
                }

                var teacherWithAvailability = await _joinedTeacherAndAvailabilityData.GetAllBySemesterId(semesterData.SemesterId);
                if (teacherWithAvailability == null)
                {
                    ModelState.AddModelError("Teacher", $"Could not find the Teacher Data ");
                    return BadRequest(ModelState);

                }
                var teacherAvailabilityMap = teacherWithAvailability.ToDictionary(t => t.TeacherId);

                var existingSchedules = (await _classSchedule.GetAllWithTimeSlot()).ToList();

                var finalSchedule = new List<ClassSchedule>();
                foreach (var subject in subjectData)
                {
                    var assignment = teacherAssignmentData.FirstOrDefault(a => a.SubjectId == subject.SubjectId);
                    if (assignment == null || !teacherAvailabilityMap.ContainsKey(assignment.TeacherId))
                        continue;

                    var teacherData = teacherAvailabilityMap[assignment.TeacherId];

                    var availableSlot = FindAvailableSlot
                                        (
                                            timeSlotData, 
                                            teacherData, 
                                            assignment.TeacherId, 
                                            existingSchedules,
                                            SemesterInstanceId
                                        );
                    if (availableSlot == null) continue;
                    var schedule = new ClassSchedule
                    {
                        SemesterInstanceId = SemesterInstanceId,
                        TimeSlotId = availableSlot.TimeSlotId,
                        SubjectId = subject.SubjectId,
                        TeacherId = assignment.TeacherId
                    };
                    finalSchedule.Add( schedule );

                    var newExistingSchedule = new ClassScheduleWithTimeSlotDTO
                    {
                        SemesterInstanceId = SemesterInstanceId,
                        TimeSlotId = availableSlot.TimeSlotId,
                        StartTime = availableSlot.StartTime,
                        EndTime = availableSlot.EndTime,
                    };
                    existingSchedules.Add(newExistingSchedule);
                }
                if (!finalSchedule.Any())
                {
                    ModelState.AddModelError("Failed", "CouldnotCreate the schedule");
                    return BadRequest(ModelState);
                }
                return Ok(finalSchedule);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }



        }

        private TimeSlot? FindAvailableSlot(
            List<TimeSlot> timeSlots,
            TeacherWithAvailability teacherData,
            int teacherId,
            List<ClassScheduleWithTimeSlotDTO> existingSchedules,
            int SemesterInstanceId

         )
        {
            foreach (var slot in timeSlots)
            {
                bool isAvailable = teacherData.StartTime <= slot.StartTime && teacherData.EndTime >= slot.EndTime;

                bool isAlreadySchedule = existingSchedules.Any
                                            (
                                                s => s.TeacherId == teacherId 
                                                && !(s.EndTime <= slot.StartTime ||  s.StartTime >= slot.EndTime)
                                              
                                            );
                bool isSlotTakenInSemester = existingSchedules.Any(
                s => s.SemesterInstanceId == SemesterInstanceId &&
                     s.TimeSlotId == slot.TimeSlotId
                );
                if (isAvailable && !isAlreadySchedule && !isSlotTakenInSemester)
                {
                    return slot;
                }
            }
            return null;
        } 
    }
}
