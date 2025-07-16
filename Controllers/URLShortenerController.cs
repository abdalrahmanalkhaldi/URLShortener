using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Services.URLShortener;

namespace URLShortenerApiApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
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
        [Route("shorten")]
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
        [Route("get/{shortenedUrl}")]
        public async Task<IActionResult> RedirectTolUrl(string shortenedUrl)
        {
            var mm = _context.Urls
                .FirstOrDefault(x => x.GneratedURL == $"https://short.url/{shortenedUrl}");
            if (mm == null)
            {
                return NotFound("Shortened URL not found.");
            }

            return Redirect(mm.OriginalURL);
        }

    }
}
