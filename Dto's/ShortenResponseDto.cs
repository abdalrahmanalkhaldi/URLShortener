namespace URLShortenerApiApplication
{
    public class ShortenResponseDto
    {
        public string ShortenedUrl { get; set; }
        public string OriginalUrl { get; set; }
        public int ClickCount { get; set; }

        //public ShortenResponseDto(string shortenedUrl, string originalUrl, int clickCount)
        //{
        //    ShortenedUrl = shortenedUrl;
        //    OriginalUrl = originalUrl;
        //    ClickCount = clickCount;
        //}
    }
}
