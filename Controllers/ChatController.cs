using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using dotnet_webapi_claude_wrapper.Contracts;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using dotnet_webapi_claude_wrapper.Services;

namespace dotnet_webapi_claude_wrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatController(
            IChatService chatService,
            IHttpContextAccessor httpContextAccessor)
        {
            _chatService = chatService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("message")]
        public async Task<ActionResult<ChatResponse>> SendMessage(
            [FromBody] ChatRequest request,
            [FromQuery] string provider = "claude")
        {
            try
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext!.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId == 0)
                    return Unauthorized();

                var response = await _chatService.ChatAsync(userId, request, provider);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("history/{conversationId}")]
        public async Task<ActionResult<Chat>> GetChatHistory(string conversationId)
        {
            try
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext!.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                if (userId == 0)
                    return Unauthorized();

                var chat = await _chatService.GetChatHistoryAsync(userId, conversationId);
                return Ok(chat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving chat history.", error = ex.Message });
            }
        }
    }
} 