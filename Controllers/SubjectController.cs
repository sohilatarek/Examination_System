using ApplicationLayer.DTO;
using ApplicationLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{

    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService ?? throw new ArgumentNullException(nameof(subjectService));
        }

        //To Add New Subject
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectDTO subjectDTO)
        {
            try
            {
                var result = await _subjectService.AddSubjectAsync(subjectDTO);

                return CreatedAtAction(nameof(GetSubjectById), new { subjectId = result.SubjectID }, result);
            }
            catch (Exception ex)
            {

                return BadRequest($"Failed to add subject. {ex.Message}");
            }
        }

        //To Get Specific Subjects By Id       
        [HttpGet("{subjectId}", Name = nameof(GetSubjectById))]
        [Authorize(Roles = "Admin")] // This endpoint can be accessed by both Admin and Student roles
        public async Task<IActionResult> GetSubjectById(int subjectId)
        {
            try
            {
                var result = await _subjectService.GetSubjectByIdAsync(subjectId);

                return result != null ? Ok(result) : NotFound($"Subject with ID {subjectId} not found");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest($"Error retrieving subject. {ex.Message}");
            }
        }

        //To Edit Specific Subject By Id
        [HttpPut("{subjectId}", Name = nameof(UpdateSubject))]
        [Authorize(Roles = "Admin")] // Make sure to secure this endpoint
        public async Task<IActionResult> UpdateSubject(int subjectId, [FromBody] SubjectDTO subjectDTO)
        {
            try
            {
                var result = await _subjectService.UpdateSubjectAsync(subjectId, subjectDTO);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest($"Failed to update subject. {ex.Message}");
            }
        }

        //To Delete Subject
        [HttpDelete("{subjectId}", Name = nameof(DeleteSubject))]
        [Authorize(Roles = "Admin")] // Make sure to secure this endpoint
        public async Task<IActionResult> DeleteSubject(int subjectId)
        {
            try
            {
                var result = await _subjectService.DeleteSubjectAsync(subjectId);

                if (result)
                    return NoContent(); // 204 No Content
                else
                    return NotFound($"Subject with ID {subjectId} not found");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return BadRequest($"Failed to delete subject. {ex.Message}");
            }
        }

        //To Get All Subjects In The System
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSubjects([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var subjects = await _subjectService.GetSubjectsAsync(page, pageSize);
            return Ok(subjects);
        }

    }



}
