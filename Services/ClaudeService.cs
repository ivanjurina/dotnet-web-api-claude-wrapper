using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using dotnet_webapi_claude_wrapper.Configuration;
using dotnet_webapi_claude_wrapper.Contracts;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using dotnet_webapi_claude_wrapper.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using dotnet_webapi_claude_wrapper.Extensions;
using Anthropic;
using Microsoft.Extensions.Options;
using dotnet_webapi_claude_wrapper.Contracts.Configuration;
using dotnet_webapi_claude_wrapper.Contracts.Models;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using dotnet_webapi_claude_wrapper.Repositories;
using Message = dotnet_webapi_claude_wrapper.DataModel.Entities.Message;

namespace dotnet_webapi_claude_wrapper.Services
{
    public interface IClaudeService
    {
        Task<ChatResponse> ChatAsync(int userId, ChatRequest request);
    }
    
    public class ClaudeService : IClaudeService
    {
        private readonly AnthropicClient _client;
        private readonly ClaudeSettings _settings;
        private readonly IClaudeRepository _repository;

        public ClaudeService(
            IOptions<ClaudeSettings> settings,
            IClaudeRepository repository)
        {
            _settings = settings.Value;
            _repository = repository;
            _client = new AnthropicClient(_settings.ApiKey);
        }

        public async Task<ChatResponse> ChatAsync(int userId, ChatRequest request)
        {
            var chat = await _repository.GetOrCreateChatAsync(userId, request.ConversationId);
            var chatHistory = await _repository.GetChatMessagesAsync(chat.Id);

            List<Message> messages = chatHistory.Select(m => new Message(m.Role, m.Content)).ToList();
            messages.Add(new Message("user", request.Message));

            var response = await _client.CreateMessageAsync(
                model: CreateMessageRequestModel.Claude35Sonnet20240620,
                messages: messages,
                maxTokens: _settings.MaxTokens,
                system: _settings.DefaultSystem);

            // Save user message
            var userMessage = new DataModel.Entities.Message
            {
                ChatId = chat.Id,
                Role = "user",
                Content = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            // Save assistant message
            var assistantMessage = new DataModel.Entities.Message
            {
                ChatId = chat.Id,
                Role = "assistant",
                Content = response.Content.Value,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.SaveMessagesAsync(userMessage, assistantMessage);

            return new ChatResponse
            {
                Message = response.Content.Value,
                ConversationId = chat.ConversationId
            };
        }
    }
}