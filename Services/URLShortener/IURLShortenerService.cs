using URLShortenerApiApplication.Entities;
using URLShortenerApiApplication.Models;

namespace URLShortenerApiApplication.Services.URLShortener
{
    public interface IURLShortenerService
    {
       public Task<ShortenResponseDto> URLShortener(ShortenResquestDto url);
        public Task<string> getTheMainURL(string url);
        public Task<string> getTheMainURLRedirect(string url);
        public Task<ShortenResponseDto> CustumizeShorten(string url, int userId, string customurl);
        public Task<List<URL>> ListAllUserURLs (int userId);
        public Task<UserModel> GetTheUserInfo(int userId);
        public string DeleteYourURl(int userId, string url);
        public Task<URL> GetUrlInfo(int userId, string url);
        public Task<string> UpdateUrlExpiringDate(string url , DateTime dateTime);
        public string DeleteUser(int userId);



    }
}
