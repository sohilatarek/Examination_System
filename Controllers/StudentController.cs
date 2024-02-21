using ApplicationLayer.DTO;
using ApplicationLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [ApiController]
    [Route("api/students")]
    [Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        //For Getting All Students With Pagination
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var students = await _studentService.GetStudentsAsync(page, pageSize);
            return Ok(students);
        }

        //To Enable/Disable Activity
        [HttpPut("{studentId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStudentStatus(string studentId, [FromBody] ChangeStudentStatusDTO statusDTO)
        {
            await _studentService.ChangeStudentStatusAsync(studentId, statusDTO);
            return NoContent();
        }
    }
}
