using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using System.Text;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Dto_s;
using URLShortenerApiApplication.Models;
using URLShortenerApiApplication.Services;
using URLShortenerApiApplication.Services.TokenService;

namespace URLShortenerApiApplication.Services
{

    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _token;
        public LoginService(AppDbContext context, ITokenService token)
        {
            _context = context;
            _token = token;

        }
        public async Task<UserModel?> LoginAsync(LoginDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == loginDto.Username.ToLower());
            if (user ==null )
            {
                return default;
            }

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //if (!computedHash.SequenceEqual(user.PasswordHash))
            //{
            //    return default;
            //}

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return default;
                }
            }

            return await Task.FromResult(new UserModel
            {
                Username = user.Username,
                Token = _token.GenerateToken(user)
            });

        }

        
    }
}
