using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddy.Services.Interfaces;

namespace StudyBuddy.Controllers
{
    [ApiController]
    [Route("api/ai")]
    [Authorize]
    public class AiChatController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiChatController(IAiService aiService)
        {
            _aiService = aiService;
        }

        public class ChatRequest
        {
            public List<ChatMessage> History { get; set; } = new();
            public string Message { get; set; } = "";
        }

        public class ChatMessage
        {
            public string Role { get; set; } = "";
            public string Content { get; set; } = "";
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            var history = request.History
                .Select(m => (m.Role, m.Content))
                .ToList();

            var response = await _aiService.ChatAsync(history, request.Message);
            return Ok(new { response });
        }
    }
}
