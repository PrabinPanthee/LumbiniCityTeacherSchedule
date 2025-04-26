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
                //map the teacherWithAvailability in map for easy access
                var teacherAvailabilityMap = teacherWithAvailability.ToDictionary(t => t.TeacherId);

                //get the existing schedule data
                var existingSchedules = (await _classSchedule.GetAllWithTimeSlot()).ToList();

                //store available slot for each tracher 
                var teacherAvailableSlots = new List<(int subjectId , int teacherId, List<int> slotIds)>();

                var finalSchedule = new List<ClassSchedule>();
                foreach (var subject in subjectData)
                {
                    var assignment = teacherAssignmentData.FirstOrDefault(a => a.SubjectId == subject.SubjectId);
                    if (assignment == null || !teacherAvailabilityMap.ContainsKey(assignment.TeacherId))
                        continue;

                    var teacherData = teacherAvailabilityMap[assignment.TeacherId];

                    var availableSlotIds = FindAvailableSlotIds
                                        (
                                            timeSlotData, 
                                            teacherData, 
                                            assignment.TeacherId, 
                                            existingSchedules,
                                            SemesterInstanceId
                                        );
                    if (availableSlotIds.Any()) 
                    { 
                     teacherAvailableSlots.Add((subject.SubjectId,assignment.TeacherId, availableSlotIds));
                    }
                }
                
                var sortedTeacher = teacherAvailableSlots.OrderBy(t=> t.slotIds.Count).ToList();
                foreach (var (subjectId, teacherId, slotIds) in sortedTeacher)
                {
                    foreach (var slotId in slotIds)
                    {
                        bool alreadyTaken = existingSchedules.Any(s => s.TimeSlotId == slotId &&
                            (s.TeacherId == teacherId || s.SemesterInstanceId == SemesterInstanceId));
                        if (!alreadyTaken)
                        {
                            finalSchedule.Add(new ClassSchedule
                            {
                                SemesterInstanceId = SemesterInstanceId,
                                SubjectId = subjectId,
                                TeacherId = teacherId,
                                TimeSlotId = slotId
                            });

                            var slot = timeSlotData.First(s => s.TimeSlotId == slotId);
                            existingSchedules.Add(new ClassScheduleWithTimeSlotDTO
                            {
                                SemesterInstanceId = SemesterInstanceId,
                                TimeSlotId = slotId,
                                TeacherId = teacherId,
                                StartTime = slot.StartTime,
                                EndTime = slot.EndTime
                            });
                            break;
                        }

                    }

                }
                if (!finalSchedule.Any())
                    return BadRequest("Failed to generate schedule.");
                await _classSchedule.BulkInsert(finalSchedule);

                return Ok(finalSchedule);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }



        }

        private List<int> FindAvailableSlotIds(
            List<TimeSlot> timeSlots,
            TeacherWithAvailability teacherData,
            int teacherId,
            List<ClassScheduleWithTimeSlotDTO> existingSchedules,
            int SemesterInstanceId

         )
        {
            var availableSlotsIds = new List<int>();
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
                    availableSlotsIds.Add( slot.TimeSlotId );
                }
            }
            if (!availableSlotsIds.Any())
            {
                Console.WriteLine($"Cannot find suitable time for {teacherId}");
            }
            return availableSlotsIds;
        } 
    }
}
