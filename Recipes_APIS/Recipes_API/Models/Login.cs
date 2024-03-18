namespace Recipe.Models
{
    public class Login
    {
        public string Identifier { get; set; } // Change to Identifier to accept either username or email
        public string Password { get; set; }
    }
}
