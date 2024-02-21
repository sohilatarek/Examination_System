using ApplicationLayer.DTO;
using ApplicationLayer.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [ApiController]
    [Route("api/admin/questions")]
    [Authorize(Roles = "Admin")]

    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionForAdmin addQuestionDTO)
        {
            await _questionService.AddQuestionAsync(addQuestionDTO);
            return CreatedAtAction(nameof(AddQuestion), new { id = addQuestionDTO.SubjectId }, addQuestionDTO);
        }
    }


}
