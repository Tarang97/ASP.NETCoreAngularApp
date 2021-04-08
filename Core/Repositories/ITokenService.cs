using Core.Entities.Identity;

namespace Core.Repositories
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}