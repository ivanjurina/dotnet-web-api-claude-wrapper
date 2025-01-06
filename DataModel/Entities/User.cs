using System.ComponentModel.DataAnnotations;

namespace dotnet_webapi_claude_wrapper.DataModel.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }  


    }
}