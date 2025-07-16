using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using URLShortenerApiApplication.Entities;
using URLShortenerApiApplication.Models;

namespace URLShortenerApiApplication.Services.TokenService
{


    public class TokenService : ITokenService
    {
        private readonly JwtToken _tokenRes;

        public TokenService(JwtToken tokenRes)
        {
            _tokenRes = tokenRes;
        }
        public string GenerateToken(User user)
        {
            var JWtTokenHandeler = new JwtSecurityTokenHandler();
            var JwtTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenRes.Issuer,
                Audience = _tokenRes.Audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenRes.SigningKey)),
                    SecurityAlgorithms.HmacSha256Signature
                ),

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                })
            };
            var SecurityToken = JWtTokenHandeler.CreateToken(JwtTokenDescriptor);
            var accessToken = JWtTokenHandeler.WriteToken(SecurityToken);




            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenDescription = new SecurityTokenDescriptor
            //{
            //    Issuer = _tokenRes.Issuer,
            //    Audience = _tokenRes.Audience,

            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenRes.SigningKey))
            //    , SecurityAlgorithms.HmacSha256),
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier,user.Username),
            //    })
            //};
            //var SecurityToken = tokenHandler.CreateToken(tokenDescription);
            //var accessToken = tokenHandler.WriteToken(SecurityToken);

            return (accessToken);
        }
    }
}
