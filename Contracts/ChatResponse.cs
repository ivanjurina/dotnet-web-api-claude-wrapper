namespace dotnet_webapi_claude_wrapper.Contracts
{
    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ConversationId { get; set; } = string.Empty;
    }
}