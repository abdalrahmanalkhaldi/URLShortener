using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Dto_s;
using URLShortenerApiApplication.Entities;
using URLShortenerApiApplication.Services.URLShortener;

namespace URLShortenerApiApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class URLShortenerController : ControllerBase
    {
        private readonly IURLShortenerService _urlShortenerService;
        private readonly AppDbContext _context;
        public URLShortenerController(IURLShortenerService urlShortenerService , AppDbContext context)
        {
            _urlShortenerService = urlShortenerService;
            _context = context;
        }

        [HttpPost]
        [Route("Short your URL")]
        public async Task<IActionResult> ShortYourUrl(ShortenResquestDto url)
        {
            var result = await _urlShortenerService.URLShortener(url);
            if (result == null)
            {
                return BadRequest("Error shortening the URL.");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Get Orginal URL")]
        public async Task<IActionResult> RedirectTolUrl(string url )
        {
            var result = await _urlShortenerService.getTheMainURL(url);
            return Ok(result);
        }

        [HttpPost]
        [Route("Customize Shorten URL")]
        public async Task<IActionResult> CustumizeYourURL(string url, int userId, string customurl)
        {
            if (string.IsNullOrEmpty(customurl) || string.IsNullOrEmpty(url) || userId <= 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            var result = await _urlShortenerService.CustumizeShorten(url, userId, customurl);
            if (result == null)
            {
                return BadRequest("Error customizing the URL.");
            }
            return Ok(result);
        }


        [HttpGet]
        [Route("List All User URLs")]
        public async Task<ActionResult<List<URL>>> ListAllUserURLs(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var result = await _urlShortenerService.ListAllUserURLs(userId);
            if (result == null || result.Count == 0)
            {
                return NotFound("No URLs found for the user.");
            }
            return Ok(result);
           
        }

        [HttpGet]
        [Route("Get User Info")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var result = await _context.Users.Where(u => u.UserId == userId).ToListAsync();
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete User URL")]
        public ActionResult DeleteUserURL(int userId, string url)
        {
            if (userId <= 0 || string.IsNullOrEmpty(url))
            {
                return BadRequest("Invalid input parameters.");
            }

            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var urlToDelete = _context.Urls.FirstOrDefault(u => u.UserId == userId && u.GneratedURL == url);
            if (urlToDelete == null)
            {
                return NotFound("URL not found for the user.");
            }

            _context.Urls.Remove(urlToDelete);
            _context.SaveChanges();

            return Ok("URL deleted successfully.");
        }
    }
}
