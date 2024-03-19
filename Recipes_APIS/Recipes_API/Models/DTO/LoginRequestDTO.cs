namespace Recipe.Models.DTO
{
    public class LoginRequestDTO
    {
        public string Identifier { get; set; } // Change to Identifier to accept either username or email
        public string Password { get; set; }
    }
}
