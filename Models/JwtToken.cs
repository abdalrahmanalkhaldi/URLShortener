namespace URLShortenerApiApplication.Models
{
    public class JwtToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public int Liftime { get; set; }
        public string SigningKey { get; set; }

    }
}
