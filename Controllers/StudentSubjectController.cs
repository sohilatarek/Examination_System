using ApplicationLayer.DTO;
using ApplicationLayer.IServices;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.IRepository;
using RepositoryLayer.Models;
using RepositoryLayer.Repository;
using System.Security.Claims;

namespace APILayer.Controllers
{

    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/student/subjects")]
    public class StudentSubjectController : ControllerBase
    {
        private readonly IStudentSubjectService _studentSubjectService;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public StudentSubjectController(IStudentSubjectService studentSubjectService, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _studentSubjectService = studentSubjectService;
            _userManager = userManager;
        }

        //To Add New Subject To The Student
        [HttpPost("add")]
        public async Task<IActionResult> AddStudentSubject(int subjectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await _studentSubjectService.AddStudentSubjectAsync(userId, subjectId);

            return Ok($"Subject '{subjectId}' added successfully to user '{user.UserName}'");
        }

        //To Get All Subject Of the Student
        [HttpGet]
        public async Task<IActionResult> GetStudentSubjects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subjects = await _studentSubjectService.GetStudentSubjectsAsync(userId);

            if (subjects != null)
            {
                return Ok(subjects);
            }

            return BadRequest(new { Error = "Failed to retrieve student subjects" });
        }
       
       
    }
}

