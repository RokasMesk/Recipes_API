#pragma warning disable CS8618
#pragma warning disable S101

namespace Recipe.Models.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set;}
    }
}
