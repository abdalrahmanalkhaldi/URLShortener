using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Services.TokenService
{
    public interface ITokenService
    {

        //public Task<TokenResponseDto> GenerateToken(User user);
        public string GenerateToken(User username);
    }
}
