using dotnet_webapi_claude_wrapper.Contracts;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using dotnet_webapi_claude_wrapper.Repositories;

namespace dotnet_webapi_claude_wrapper.Services
{
    public interface IChatService
    {
        Task<ChatResponse> ChatAsync(int userId, ChatRequest request, string provider = "chatgpt");
        Task<Chat> GetChatHistoryAsync(int userId, string conversationId);
    }

    public class ChatService : IChatService
    {
        private readonly IClaudeService _claudeService;
        private readonly IChatGptService _chatGptService;
        private readonly IChatRepository _repository;
        public ChatService(
            IClaudeService claudeService,
            IChatGptService chatGptService,
            IChatRepository repository)
        {
            _claudeService = claudeService;
            _chatGptService = chatGptService;
            _repository = repository;
        }

        public async Task<ChatResponse> ChatAsync(int userId, ChatRequest request, string provider = "chatgpt")
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
            return await _repository.GetOrCreateChatAsync(userId, conversationId);
        }
    }
} 