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
        public URLShortenerController(IURLShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;

        }

        [HttpPost]
        [Route("Short_your_URL")]
        public async Task<IActionResult> ShortYourUrl(ShortenResquestDto url)
        {
            if (url == null)
            {
                return BadRequest("URL cannot be null.");
            }
            var result = await _urlShortenerService.URLShortener(url);
            if (result == null)
            {
                return BadRequest("Error shortening the URL.");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Get_Orginal_URL")]
        public async Task<IActionResult> GetTheMainUrl(string url )
        {
            var result = await _urlShortenerService.getTheMainURL(url);
            if (result == null)
            {
                return NotFound("URL not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("Customize_Shorten_URL")]
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
        [Route("List_All_User_URLs")]
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
        [Route("Get_User_Info")]
        public async Task<IActionResult> GetUserInfo(int userId)
        {
           
            if (userId == null)
            {
                return BadRequest("You Cant Leave The Argument Null");
            }
            var result = await _urlShortenerService.GetTheUserInfo(userId);

            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete_User_URL")]
        public ActionResult DeleteUserURL(int userId, string url)
        {
            if (userId <= 0 || string.IsNullOrEmpty(url))
            {
                return BadRequest("Invalid input parameters.");
            }

            var result = _urlShortenerService.DeleteYourURl(userId, url);
            if (result==null)
            {
                return NotFound("URL not found or does not belong to the user.");
            }
            return Ok("URL deleted successfully.");
        }

        [HttpGet]
        [Route("Redirect")]
        public async Task<IActionResult> RedirectToUrl(string url)
        {
            var  mainnurl = await _urlShortenerService.getTheMainURLRedirect(url);
            if (mainnurl == null)
            {
                return NotFound("URL not found.");
            }
            
            return Redirect(mainnurl); // Redirect to the original URL

        }

        [HttpGet]
        [Route("Get_URl_INFO")]
        public async Task<IActionResult> GetTheUrlInfo(int user , string url)
        {
            if (string.IsNullOrEmpty(url) || user <= 0)
            {
                return BadRequest("URL or UserId cannot be null or empty.");
            }

           var result = await _urlShortenerService.GetUrlInfo(user, url); 
            if (result == null)
            {
                return NotFound("URL not found.");
            }
            return  Ok(result);
        }

        [HttpPut]
        [Route("Update_URL_Date")]
        public async Task<IActionResult> UpdateUrlExpiringDate(string url , DateTime dateTime)
        {
            if (string.IsNullOrEmpty(url) || dateTime == default)
            {
                return BadRequest("Invalid input parameters.");
            }

            var result = await _urlShortenerService.UpdateUrlExpiringDate(url, dateTime);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete_User")]
        public async Task<IActionResult> DeleteUser(int user)
        {
            if (user == null)
                return BadRequest("User ID cannot be null.");

           var result = _urlShortenerService.DeleteUser(user);
            if (result == null)
            {
                return NotFound("User not found or does not have any URLs.");
            }
            return Ok(result);
        }
    }
}
