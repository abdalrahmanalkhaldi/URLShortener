using Microsoft.AspNetCore.Mvc;
using URLShortenerApiApplication.Dto_s;
using URLShortenerApiApplication.Services;
using URLShortenerApiApplication.Services.RegisterService;
using URLShortenerApiApplication.Services.TokenService;

namespace URLShortenerApiApplication.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly ITokenService _tokenService;
        
        public AuthController(IRegisterService registerService , ILoginService loginService , ITokenService tokenService)
        {
            _registerService = registerService;
            _loginService = loginService;
            _tokenService = tokenService;

        }

        [HttpPost]
        [Route("api/[controller]/login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            if (loginDto==null)
            {
                return BadRequest("Username and password cannot be empty.");
            }

            var result = await _loginService.LoginAsync(loginDto);
            if (result == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("api/[controller]/register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Username and password cannot be empty.");
            }

            var result = await _registerService.RegisterAsync(registerDto);
            if (result == null)
            {
                return BadRequest("User already exists.");
            }
            return Ok(result);
        }

    }
}
