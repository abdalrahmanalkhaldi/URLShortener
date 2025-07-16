namespace URLShortenerApiApplication.Services.URLShortener
{
    public class URLShortenerService : IURLShortenerService
    {
       public string UrlShortenerName { get; set; }

        public string URLShortener(string url)
        {
            // Simple URL shortening logic (for demonstration purposes)
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }

            // Generate a short URL by taking the last 6 characters of the hash of the URL
            var hash = System.Security.Cryptography.SHA256.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(url));
            var shortUrl = Convert.ToBase64String(hash).Substring(0, 6);

            return $"https://short.url/{shortUrl}";
        }
    }
}
