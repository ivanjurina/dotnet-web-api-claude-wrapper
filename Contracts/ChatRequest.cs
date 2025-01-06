namespace dotnet_webapi_claude_wrapper.Contracts
{
    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public string? ConversationId { get; set; }
    }
}