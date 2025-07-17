namespace URLShortenerApiApplication
{
    public class ShortenResquestDto
    {
        public string Url { get; set; }
        public string Token { get; set; } // Assuming this is a token for authentication or validation
        //public string CustomCode { get; set; }
        //public bool IsCustomCode { get; set; }
        //public bool IsActive { get; set; } = true;
        //public int ExpirationDays { get; set; } = 30;
        public int UserId { get; set; } // Assuming this is the user ID for the owner of the shortened URL
    }
}
