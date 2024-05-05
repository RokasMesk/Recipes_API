using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models.DTO
{
    public class LoginRequestDTO
    {
        [MinLength(4, ErrorMessage = "Length must be greated than 4(Identifier)") ]
        public string Identifier { get; set; } // Change to Identifier to accept either username or email
        [MinLength(4, ErrorMessage = "Length must be greated than 4(Password)")]
        public string Password { get; set; }
    }
}
