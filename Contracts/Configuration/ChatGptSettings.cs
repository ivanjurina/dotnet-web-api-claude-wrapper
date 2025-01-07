namespace dotnet_webapi_claude_wrapper.Contracts
{
    public class ChatGptSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public int MaxTokens { get; set; } = 1024;
    }
} 