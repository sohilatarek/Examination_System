using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationLayer.DTO;
using ApplicationLayer.IServices;
using ApplicationLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using RepositoryLayer.Models;

namespace APILayer.Controllers
{
    [ApiController]
    [Route("api/exams")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IQuestionService _questionService;

        public ExamController(IExamService examService, IQuestionService questionService)
        {
            _examService = examService;
            _questionService = questionService;
        }

        [HttpGet("getExam")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetExam(int subjectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var examDTO = await _examService.CreateExamAsync(subjectId, userId);
                return Ok(examDTO);
            }
            catch (Exception ex)
            {
             
                return BadRequest(new { Message = "Failed to get exam" });
            }
        }
        [HttpPost("submitExam")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitExam([FromBody] SubmitExamRequestDTO submitExamRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (submitExamRequest == null || submitExamRequest.ExamId <= 0 || submitExamRequest.StudentAnswers == null)
                {
                    Console.WriteLine("Failed to retrieve student subjects");
                    return BadRequest(new { Message = "Invalid data submitted" });
                }

               
                await _examService.SubmitExamAsync(submitExamRequest.ExamId, submitExamRequest.StudentAnswers, userId);

               
                return Ok(new { Message = "Exam submitted successfully." });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { Message = "An error occurred while processing the exam" });
            }
        }


        [HttpGet("getExamHistoryStudent")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetExamHistory()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { Message = "User ID not found." });
                }

                var examHistory = await _examService.GetExamHistoryAsync(userId);

                return Ok(examHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching exam history." });
            }
        }



        [HttpGet("getAllExamsHistoryAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllExamsHistory()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { Message = "User ID not found." });
                }
                var examsHistory = await _examService.GetAdminExamHistoryAsync();

                return Ok(examsHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching exams history." });
            }
        }



    }
}






