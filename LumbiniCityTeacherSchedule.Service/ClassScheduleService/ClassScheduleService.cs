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
using LumbiniCityTeacherSchedule.Models.Results;

namespace LumbiniCityTeacherSchedule.Service.ClassScheduleService
{
    public class ClassScheduleService : IClassScheduleService
    {
        private readonly IClassScheduleData _classSchedule;
        private readonly ISemesterInstanceData _semesterInstanceData;
        private readonly ISemesterConfigData _semesterConfigData;
        private readonly ITeacherAssignmentData _teacherAssignmentData;
        private readonly IJoinedTeacherAndAvailabilityData _joinedTeacherAndAvailabilityData;
        private readonly ISubjectData _subjectData;
        private readonly ISemesterData _semesterData;
        private readonly ITimeSlotData _timeSlotData;

        public ClassScheduleService(IClassScheduleData classSchedule, ISemesterInstanceData semesterInstanceData, ISemesterConfigData semesterConfigData, ITeacherAssignmentData teacherAssignmentData, IJoinedTeacherAndAvailabilityData joinedTeacherAndAvailabilityData, ISubjectData subjectData, ISemesterData semesterData, ITimeSlotData timeSlotData)
        {
            _classSchedule = classSchedule;
            _semesterInstanceData = semesterInstanceData;
            _semesterConfigData = semesterConfigData;
            _teacherAssignmentData = teacherAssignmentData;
            _joinedTeacherAndAvailabilityData = joinedTeacherAndAvailabilityData;
            _subjectData = subjectData;
            _semesterData = semesterData;
            _timeSlotData = timeSlotData;
        }

        public async Task<ServiceResult> GenerateClassSchedule(int SemesterInstanceId)
        {
            var semesterInstanceData = await _semesterInstanceData.Get(SemesterInstanceId);
            if (semesterInstanceData == null)
            {
                return ServiceResult.Fail("Cannot find semesterInstance data");
            }

            var semesterData = await _semesterData.Get(semesterInstanceData!.SemesterId);
            if (semesterData == null)
            {
                return ServiceResult.Fail("Cannot find the semester data for Semester :{semesterData!.SemesterNumber}");
            }

            var semesterConfigData = await _semesterConfigData.GetBySemesterId(semesterData.SemesterId);
            if (semesterConfigData == null)
            {
                return ServiceResult.Fail("Cannot find the semester config data for current semester") ;
            }

            var timeSlotData = (await _timeSlotData.GetAllByConfigId(semesterConfigData.ConfigId))
                                    .Where(ts => ts.Type == "class")
                                   .OrderBy(ts => ts.TimeSlotId).ToList();
            if (timeSlotData.Count == 0)
            {
                return ServiceResult.Fail("Could not find slot data");
            }

            var subjectData = await _subjectData.GetAllSubjectBySemesterId(semesterData.SemesterId);
            if (subjectData == null)
            {
                return ServiceResult.Fail($"Could not find Subject data for semester:{semesterData.SemesterNumber}"); ;
            }

            var teacherAssignmentData = await _teacherAssignmentData.GetAllBySemesterId(semesterData.SemesterId);
            if (teacherAssignmentData == null)
            {
                return ServiceResult.Fail($"No teacher are assigned for {semesterData.SemesterNumber}"); ;
            }

            var teacherWithAvailability = await _joinedTeacherAndAvailabilityData.GetAllBySemesterId(semesterData.SemesterId);
            if (teacherWithAvailability == null)
            {
                return ServiceResult.Fail($"Could not find the Teacher Data ");
            }

            //map the teacherWithAvailability in map for easy access
            var teacherAvailabilityMap = teacherWithAvailability.ToDictionary(t => t.TeacherId);

            //get the existing schedule data
            var existingSchedules = (await _classSchedule.GetAllWithTimeSlot()).ToList();

            //store available slot for each teacher 
            var teacherAvailableSlots = new List<(int subjectId, int teacherId, List<int> slotIds)>();

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
                    teacherAvailableSlots.Add((subject.SubjectId, assignment.TeacherId, availableSlotIds));
                }
            }
            //least flexible first + greedy algorithm 
            var sortedTeacher = teacherAvailableSlots.OrderBy(t => t.slotIds.Count).ToList();
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
            {
                return ServiceResult.Fail("Failed to create schedule");
            }
               
            await _classSchedule.BulkInsert(finalSchedule);
            return ServiceResult.Ok("Schedule Created Successfully");
        }

        // (GreedyAlgorithm)
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
                                                && !(s.EndTime <= slot.StartTime || s.StartTime >= slot.EndTime)

                                            );
                bool isSlotTakenInSemester = existingSchedules.Any(
                s => s.SemesterInstanceId == SemesterInstanceId &&
                     s.TimeSlotId == slot.TimeSlotId
                );
                if (isAvailable && !isAlreadySchedule && !isSlotTakenInSemester)
                {
                    availableSlotsIds.Add(slot.TimeSlotId);
                }
            }
            if (!availableSlotsIds.Any())
            {
                Console.WriteLine($"Cannot find suitable time for {teacherId}");
            }
            return availableSlotsIds;
        }
        
        public async Task<ServiceResult<IEnumerable<JoinedClassScheduleDataDTO>>> GetAllBySemesterId(int semesterId)
        {
           var semester = await _semesterData.Get(semesterId);
            if(semester == null)
            {
                return ServiceResult<IEnumerable<JoinedClassScheduleDataDTO>>.Fail("Invalid semester");
            }

            var semesterInstance = await _semesterInstanceData.GetActiveInstanceBySemesterId(semesterId);
            if(semesterInstance == null)
            {
                return ServiceResult<IEnumerable<JoinedClassScheduleDataDTO>>.Fail("No active sem or schedule available for this semester");
            }
            var scheduleData = await _classSchedule.GetAllJoinedClassesBySemesterInstanceId(semesterInstance.SemesterInstanceId);

            return ServiceResult<IEnumerable<JoinedClassScheduleDataDTO>>.Ok(scheduleData,"Fetched successfully");
        }

    }
}

