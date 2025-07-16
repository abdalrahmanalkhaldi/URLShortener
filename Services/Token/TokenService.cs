using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Services.TokenService
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(User username)
        {
            return "GeneratedTokenFor_" + username.Username;
        }
    }
}
