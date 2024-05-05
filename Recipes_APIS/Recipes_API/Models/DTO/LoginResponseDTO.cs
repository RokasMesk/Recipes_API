#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models.DTO
{
    public class LoginResponseDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public string UserId { get; set; }
    }
}
