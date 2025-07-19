using Azure.Identity;
using System.Security.Cryptography;
using System.Text;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Entities;
using URLShortenerApiApplication.Models;
using URLShortenerApiApplication.Services.TokenService;

namespace URLShortenerApiApplication.Services.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        public RegisterService(AppDbContext context,ITokenService token)
        {
            _context = context;
            _tokenService = token;
        }
        public async Task<UserModel> RegisterAsync(RegisterDto registerDto)
        {

            var user = _context.Users.FirstOrDefault(x => x.Username == registerDto.Username.ToLower());
            if (user != null)
            {
                return null; // User already exists
            }
            using var hmac = new HMACSHA512();

            var Token = _tokenService.GenerateToken(registerDto.Username);
            var newUser = new User
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Token = Token

            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserModel
            {
                Username = newUser.Username,
                Token = Token
            };

        }
    }
    
    
}
