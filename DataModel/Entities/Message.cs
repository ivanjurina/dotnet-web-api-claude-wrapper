namespace dotnet_webapi_claude_wrapper.DataModel.Entities;

public class Message
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    public required string Content { get; set; }
    public required string Role { get; set; }
    public DateTime CreatedAt { get; set; }
} 