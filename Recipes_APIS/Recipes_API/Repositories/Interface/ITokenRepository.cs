using Microsoft.AspNetCore.Identity;
using Recipe.Models;

namespace Recipe.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
