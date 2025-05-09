using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using Microsoft.AspNetCore.Mvc;


namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherAssignmentController : Controller
    {
        private readonly ITeacherAssignmentData _db;
        private readonly ISubjectData _subjectData;
        private readonly IJoinedTeacherAndAvailabilityData _teacherData;

        public TeacherAssignmentController(ITeacherAssignmentData db, ISubjectData subjectData, IJoinedTeacherAndAvailabilityData teacherData)
        {
            _db = db;
            _subjectData = subjectData;
            _teacherData = teacherData;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeacherAssignment teacherAssignment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var subject = await _subjectData.GetById(teacherAssignment.SubjectId);
                if (subject == null)
                {
                    ModelState.AddModelError("Subject Id", "Subject Id doesn't exist");
                    return BadRequest(ModelState);
                }

                var teacher = await _teacherData.GetById(teacherAssignment.TeacherId);
                if (teacher == null)
                {
                    ModelState.AddModelError(" Teacher Id", "Teacher Id doesn't exist");
                    return BadRequest(ModelState);
                }

                var isExistSubject = await _db.IsExistSubject(teacherAssignment.SubjectId);
                if (isExistSubject)
                {
                    ModelState.AddModelError("Subject To Teacher", $"Teacher is already assigned to subjectId:{teacherAssignment.SubjectId}");
                    return BadRequest(ModelState);
                }

                var numberOfClassesTeacherIsAssigned = await _db.GetTotalTeacherAssignmentForTecherId(teacherAssignment.TeacherId);

                if (numberOfClassesTeacherIsAssigned >= teacher.NumberOfClasses)
                {
                    ModelState.AddModelError("Teacher", "Maximum teacher assignment reached");
                    return BadRequest(ModelState);
                }

                await _db.Create(teacherAssignment);
                return Ok("SuccessFully created");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{AssignmentId}")]
        public async Task<IActionResult> Update(int AssignmentId, [FromBody] UpdateTeacherAssignmentDto assignmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var assignmentData = await _db.GetById(AssignmentId);
                if (assignmentData == null)
                {
                    ModelState.AddModelError("AssignmentId", "Invalid Assignment Id");
                    return BadRequest(ModelState);
                }

                var teacher = await _teacherData.GetById(assignmentDto.TeacherId);
                if (teacher == null)
                {
                    ModelState.AddModelError(" Teacher Id", "Teacher Id doesn't exist");
                    return BadRequest(ModelState);
                }

                bool isActive = await _db.IsLinkedToActiveSemester(assignmentData.TeacherId);
                if (isActive)
                {
                    ModelState.AddModelError("Active", "Cannot Update the Assignment linked to Active semester");
                    return BadRequest(ModelState);
                }
                var numberOfClassesTeacherIsAssigned = await _db.GetTotalTeacherAssignmentForTecherId(assignmentDto.TeacherId);

                if (numberOfClassesTeacherIsAssigned >= teacher.NumberOfClasses)
                {
                    ModelState.AddModelError("Teacher", "Maximum teacher assignment reached");
                    return BadRequest(ModelState);
                }

                await _db.Update(AssignmentId, assignmentDto);
                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var assignmentData = await _db.GetById(id);
                if (assignmentData == null)
                {
                    ModelState.AddModelError("AssignmentId", "Invalid Assignment Id");
                    return BadRequest(ModelState);
                }

                bool isActive = await _db.IsLinkedToActiveSemester(assignmentData.TeacherId);
                if (isActive)
                {
                    ModelState.AddModelError("Active", "Cannot Update the Assignment linked to Active semester");
                    return BadRequest(ModelState);
                }

                await _db.Delete(id);
                return Ok("Successfully deleted");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _db.GetAllTeacherAssignmentData();
                if (result == null)
                {
                    ModelState.AddModelError("No data", "Cannot find any data");
                    return BadRequest(ModelState);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
