using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Services.URLShortener
{
    public interface IURLShortenerService
    {
       public Task<ShortenResponseDto> URLShortener(ShortenResquestDto url);
        public Task<string> getTheMainURL(string url);
        public Task<ShortenResponseDto> CustumizeShorten(string url, int userId, string customurl);
        public Task<List<URL>> ListAllUserURLs (int userId);


    }
}
