using URLShortenerApiApplication.Dto_s;
using URLShortenerApiApplication.Models;

namespace URLShortenerApiApplication.Services
{
    public interface ILoginService
    {
       public Task<UserModel?> LoginAsync(LoginDto loginDto);
    }
}
