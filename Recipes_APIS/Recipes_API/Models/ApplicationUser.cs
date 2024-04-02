using Microsoft.AspNetCore.Identity;

namespace Recipe.Models
{
    public class ApplicationUser :IdentityUser
    {
        public bool? IsAdmin { get; set; }
    }
}
