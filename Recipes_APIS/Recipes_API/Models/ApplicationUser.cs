using Microsoft.AspNetCore.Identity;

namespace Recipe.Models
{
    public class ApplicationUser :IdentityUser
    {
        public ICollection<Recipee>? Recipes { get; set; }
    }
}
