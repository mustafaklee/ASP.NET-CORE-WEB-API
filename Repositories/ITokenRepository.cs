using Microsoft.AspNetCore.Identity;

namespace MyFirstApiProject.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
