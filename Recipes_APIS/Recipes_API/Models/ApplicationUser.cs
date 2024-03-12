using Microsoft.AspNetCore.Identity;

namespace Recipe.Models
{
    public class ApplicationUser :IdentityUser
    {
        public IEnumerable<Recipee>? Recipes { get; set; }
    }
}
