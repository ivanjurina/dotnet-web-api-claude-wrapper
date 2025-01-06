using System.ComponentModel.DataAnnotations;

namespace dotnet_webapi_claude_wrapper.Contracts
{
    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}