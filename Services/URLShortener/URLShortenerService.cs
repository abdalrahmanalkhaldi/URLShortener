using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Text;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Services.URLShortener
{
    public class URLShortenerService : IURLShortenerService
    {
        private readonly AppDbContext _context;
        public URLShortenerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ShortenResponseDto> URLShortener (ShortenResquestDto url)
        {
            if(url.Url==null || url.UserId == null)
            {
                throw new Exception("there is somthing miising th eurl or the userId , make sure to fuck them in the request");
            }
            
            //int ClickCount = 0;

            var userId = url.UserId;
            var newurl = url.Url.ToLower();

            

            var alfabit = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var shortenedUrl = new StringBuilder();

            int length = alfabit.Length;

            for (int i = 0; i < 6; i++)
            {
                int index = random.Next(length);
                shortenedUrl.Append(alfabit[index]);
            }

            var shortenedUrlString = $"https://short.url/{shortenedUrl}";

            var nn = new URL
            {
                GneratedURL = shortenedUrlString,
                OriginalURL = newurl,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
                
            };

            await _context.Urls.AddAsync(nn);
            await _context.SaveChangesAsync();

            return await Task.FromResult( new ShortenResponseDto
            {
                ShortenedUrl = nn.GneratedURL,
                //ClickCount = ClickCount+1
            });

        }


        public async Task<string> getTheMainURL(string url )
        {
            var generated = _context.Urls.Where(x => x.GneratedURL == url).ToString();
               
            if (generated == null)
            {
                throw new Exception("Shortened URL not found.");
            }
            
            var result = await _context.Urls.Where(x=>x.GneratedURL == url)
                .Select(x => x.OriginalURL)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                throw new Exception("Original URL not found.");
            }
            return result;

        }

        public async Task<ShortenResponseDto> CustumizeShorten (string url , int userId , string customurl)
        {
            if (string.IsNullOrEmpty(url) || userId <= 0 || string.IsNullOrEmpty(customurl))
            {
                throw new ArgumentException("Invalid input parameters.");
            }
            var newurl = $"https://short.url/{customurl}";
            var t = new URL
            {
                UserId = userId,
                GneratedURL = newurl,
                OriginalURL = url,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Urls.AddAsync(t);
            await _context.SaveChangesAsync();

            return await Task.FromResult(new ShortenResponseDto
            {
                ShortenedUrl = t.GneratedURL,
            });
        }


        public async Task<List<URL>> ListAllUserURLs(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            var tt = _context.Urls.Where(x => x.UserId == userId)
                .ToListAsync();

            return await tt;
        }

        }
}

