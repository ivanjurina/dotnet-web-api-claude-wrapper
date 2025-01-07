using dotnet_webapi_claude_wrapper.Contracts;
using dotnet_webapi_claude_wrapper.DataModel.Entities;

namespace dotnet_webapi_claude_wrapper.Services
{
    public interface IChatService
    {
        Task<ChatResponse> ChatAsync(int userId, ChatRequest request, string provider = "claude");
        Task<Chat> GetChatHistoryAsync(int userId, string conversationId);
    }

    public class ChatService : IChatService
    {
        private readonly IClaudeService _claudeService;
        private readonly IChatGptService _chatGptService;

        public ChatService(
            IClaudeService claudeService,
            IChatGptService chatGptService)
        {
            _claudeService = claudeService;
            _chatGptService = chatGptService;
        }

        public async Task<ChatResponse> ChatAsync(int userId, ChatRequest request, string provider = "claude")
        {
            return provider.ToLower() switch
            {
                "claude" => await _claudeService.ChatAsync(userId, request),
                "chatgpt" => await _chatGptService.ChatAsync(userId, request),
                _ => throw new ArgumentException($"Unsupported AI provider: {provider}")
            };
        }

        public async Task<Chat> GetChatHistoryAsync(int userId, string conversationId)
        {
            // We can use either service since they share the same database
            return await _claudeService.GetChatHistoryAsync(userId, conversationId);
        }
    }
} 