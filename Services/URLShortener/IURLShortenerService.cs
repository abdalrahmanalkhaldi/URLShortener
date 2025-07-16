using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Services.URLShortener
{
    public interface IURLShortenerService
    {
       public Task<ShortenResponseDto> URLShortener(ShortenResquestDto url);
    }
}
