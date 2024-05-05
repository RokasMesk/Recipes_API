#pragma warning disable CS8618
#pragma warning disable S101

using System.ComponentModel.DataAnnotations;

namespace Recipe.Models.DTO
{
    public class RegisterRequestDTO
    {
        [MinLength(4, ErrorMessage = "Length must be greated than 4(userName)")]
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Length must be greated than 8(Email)")]
        public string Email { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Length must be greated than 8(password)")]
        public string Password { get; set; }
    }
}
