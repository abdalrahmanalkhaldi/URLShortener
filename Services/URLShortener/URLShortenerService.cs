using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Entities;
using URLShortenerApiApplication.Models;

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
            if(url.Url==null || url.UserId <= 0)
            {
                throw new Exception("there is somthing miising th eurl or the userId , make sure to fuck them in the request");
            }
            
            

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
            var expierDate  = url.ExpirationDays;

            var shortenedUrlString = $"https://short.url/{shortenedUrl}";

            var nn = new URL
            {
                GneratedURL = shortenedUrlString,
                OriginalURL = newurl,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = expierDate

            };

            await _context.Urls.AddAsync(nn);
            await _context.SaveChangesAsync();

            return await Task.FromResult( new ShortenResponseDto
            {
                ShortenedUrl = nn.GneratedURL,
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
        public async Task<string> getTheMainURLRedirect(string url)
        {
            var generated = _context.Urls.Where(x => x.GneratedURL == url).ToString();
            
            if (generated == null)
            {
                throw new Exception("Shortened URL not found.");
            }

            var result = await _context.Urls.Where(x => x.GneratedURL == url)
                .Select(x => x.OriginalURL)
                .FirstOrDefaultAsync();

            _context.Urls.Where(x => x.GneratedURL == url).FirstOrDefault().ClickCount += 1; // Increment the click count

            _context.SaveChanges();
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

        public async Task<UserModel> GetTheUserInfo(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("Invalid user ID.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new UserModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Token = user.Token

            };
        }


        public string DeleteYourURl(int userId, string url)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var urlToDelete = _context.Urls.FirstOrDefault(u => u.UserId == userId && u.GneratedURL == url);
            if (urlToDelete == null)
            {
                throw new Exception("URL not found for the user.");
            }

            _context.Urls.Remove(urlToDelete);
            _context.SaveChanges();

            return ("URL deleted successfully.");
        }

        public async Task<URL> GetUrlInfo(int user , string url)
        {
            if (user <= 0 || string.IsNullOrEmpty(url))
            {
                throw new Exception("Invalid user ID or URL.");
            }

            var urlInfo = await _context.Urls.FirstOrDefaultAsync(u => u.GneratedURL == url && u.UserId == user);
            if (urlInfo == null)
            {
                throw new Exception("URL not found.");
            }

            return new URL
            {
                GneratedURL = urlInfo.GneratedURL,
                OriginalURL = urlInfo.OriginalURL,
                CreatedAt = urlInfo.CreatedAt,
                ExpireAt = urlInfo.ExpireAt,
                UserId = urlInfo.UserId,
                ClickCount = urlInfo.ClickCount
            };
        }

        public async Task<string> UpdateUrlExpiringDate(string url, DateTime dateTime)
        {
            if (string.IsNullOrEmpty(url) || dateTime == default)
            {
                throw new Exception("Invalid input parameters.");
            }

            var urlToUpdate = await _context.Urls.FirstOrDefaultAsync(u => u.GneratedURL == url);
            if (urlToUpdate == null)
            {
                throw new Exception("URL not found.");
            }

            urlToUpdate.ExpireAt = dateTime;

            _context.Urls.Update(urlToUpdate);
            await _context.SaveChangesAsync();

            return ("URL expiration date updated successfully.");
        }


        public string DeleteUser(int user)
        {
            if (user == null)
                throw new Exception("User ID cannot be null.");

            var userToDelete = _context.Users.Find(user);
            if (userToDelete == null)
            {
                throw new Exception("User not found.");
            }

            _context.Users.Remove(userToDelete);
            _context.Urls.RemoveRange(_context.Urls.Where(u => u.UserId == user));

            _context.SaveChanges();

            return ("User deleted successfully.");

        }

    }
}

