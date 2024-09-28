using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Week8_BT1_API_CURD.Models;
using Week8_BT1_API_CURD.Services;

namespace Week8_BT1_API_CURD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : Controller
    {
        private readonly ClassService _classService;

        public ClassController(ClassService classService)
        {
            _classService = classService;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            var classes = await _classService.GetClassesAsync();
            return Ok(classes);
        }

        // GET: api/Classes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(int id)
        {
            var classObj = await _classService.GetClassByIdAsync(id);
            if (classObj == null)
            {
                return NotFound();
            }
            return Ok(classObj);
        }

        // POST: api/Classes
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class classObj)
        {
            try
            {
                var newClass = await _classService.AddClassAsync(classObj);
                return CreatedAtAction("GetClass", new { id = newClass.ClassID }, newClass);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Classes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(int id, Class classObj)
        {
            if (id != classObj.ClassID)
            {
                return BadRequest();
            }

            try
            {
                await _classService.UpdateClassAsync(classObj);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/Classes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                return Ok(new { message = "Class, students, and teacher deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the class.", error = ex.Message });
            }
        }


    }
}
