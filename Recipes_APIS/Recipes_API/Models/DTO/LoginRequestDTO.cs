using System.ComponentModel.DataAnnotations;

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
