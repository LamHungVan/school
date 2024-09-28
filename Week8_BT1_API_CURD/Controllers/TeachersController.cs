using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Week8_BT1_API_CURD.Models;
using Week8_BT1_API_CURD.Services;

namespace Week8_BT1_API_CURD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly TeacherService _teacherService;

        public TeachersController(TeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            var teachers = await _teacherService.GetTeachersAsync();
            return Ok(teachers);
        }

        // GET: api/Teachers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound();

            return Ok(teacher);
        }

        // POST: api/Teachers
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            try
            {
                var createdTeacher = await _teacherService.CreateTeacherAsync(teacher);
                return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacher.TeacherID }, createdTeacher);
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? ex.Message;
                var errorMessage = $"Internal server error: {innerException}";
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Internal server error: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        // PUT: api/Teachers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.TeacherID)
                return BadRequest();

            try
            {
                await _teacherService.UpdateTeacherAsync(id, teacher);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Teachers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                await _teacherService.DeleteTeacherAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{teacherId}/details")]
        public async Task<IActionResult> GetTeacherDetails(int teacherId)
        {
            var teacherDetails = await _teacherService.GetTeacherDetailsAsync(teacherId);

            if (teacherDetails == null)
            {
                return NotFound();
            }

            return Ok(teacherDetails);
        }

    }
}
