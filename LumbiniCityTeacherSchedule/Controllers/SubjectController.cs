using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.Models.DTO;
using LumbiniCityTeacherSchedule.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.InteropServices;

namespace LumbiniCityTeacherSchedule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly ISubjectData _db;
        private readonly ISemesterData _semesterData;
        private readonly ISemesterInstanceData _semesterInstanceData;

        public SubjectController(ISubjectData db, ISemesterData semesterData, ISemesterInstanceData semesterInstanceData)
        {
            _db = db;
            _semesterData = semesterData;
            _semesterInstanceData = semesterInstanceData;
        }

        [HttpGet("{SemesterId}")]
        public async Task<IActionResult> GetAllSubjectBySemesterId(int semesterId)
        {
            try
            {
                var semester = await _semesterData.Get(semesterId);
                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }

                var result = await _db.GetAllSubjectBySemesterId(semesterId);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500,"Internal server error");
            
            }
            


        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(int Id) 
        {
            try
            {
                var result = await _db.GetById(Id);
                if(result == null)
                {
                    return NotFound("Cannot find subject");
                }
                return Ok(result);
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");

            }
                    
        }

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var semester = await _semesterData.Get(subject.SemesterId);
                if (semester == null)
                {
                    return NotFound("Cannot find the semester");
                }
                var isActive = await _semesterInstanceData.GetActiveInstanceBySemesterId(subject.SemesterId);
                if(isActive?.SemesterStatus == "active") 
                {
                    ModelState.AddModelError("Active Semester", "Cannot Create while semester is active");
                    return BadRequest(ModelState);
                }

                var isExistingCode = await _db.ValidateSubjectCode(subject.SubjectCode!);
                if (isExistingCode)
                {
                    ModelState.AddModelError("Duplicate Code", "Subject code must be Unique");
                    return BadRequest(ModelState);
                }
                await _db.Create(subject);
                return Ok("Created Successfully");
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        
        }

        [HttpPut("{SubjectId}")]
        public async Task<IActionResult> Update(int SubjectId, [FromBody] UpdateSubjectDto dto) 
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var isSubjectExistsOrNot = await _db.GetById(SubjectId);
                if (isSubjectExistsOrNot == null) 
                {
                    ModelState.AddModelError("Invalid Operation", "Cannot update subject with invalid or non existing id");
                    return BadRequest(ModelState);
                }
                var isActive = await _db.IsSemesterActive(SubjectId);
                if (isActive) {
                    ModelState.AddModelError("Active semester", "Cannot Update Active semester");
                    return BadRequest(ModelState);
                }

                var isSubjectCodeValid = await _db.ValidateSubjectCodeForUpdate(SubjectId, dto.SubjectCode!);
                if (isSubjectCodeValid)
                {
                    ModelState.AddModelError("Duplicate Code", "Subject code must be Unique");
                    return BadRequest(ModelState);
                }

                await _db.Update(SubjectId, dto);
                return Ok("Updated Successfully");

            } 
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("{SubjectId}")]
        public async Task<IActionResult> Delete(int SubjectId)
        {
            try
            {
                var isSubjectExistsOrNot = await _db.GetById(SubjectId);
                if (isSubjectExistsOrNot == null)
                {
                    ModelState.AddModelError("Invalid Operation", "Cannot delete subject with invalid or non existing id");
                    return BadRequest(ModelState);
                }

                var isActive = await _db.IsSemesterActive(SubjectId);
                if (isActive)
                {
                    ModelState.AddModelError("Active semester", "Cannot Delete Active semester");
                    return BadRequest(ModelState);
                }

                await _db.Delete(SubjectId);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
