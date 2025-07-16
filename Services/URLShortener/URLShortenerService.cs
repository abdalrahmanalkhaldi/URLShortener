using Microsoft.AspNetCore.Http.HttpResults;
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
    }
}

