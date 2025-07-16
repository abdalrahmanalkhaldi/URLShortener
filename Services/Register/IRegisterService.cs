using URLShortenerApiApplication.Models;

namespace URLShortenerApiApplication.Services.RegisterService
{
    public interface IRegisterService
    {
        public Task<UserModel?> RegisterAsync(RegisterDto registerDto);
    }
}
