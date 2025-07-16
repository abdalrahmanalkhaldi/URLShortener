namespace URLShortenerApiApplication
{
    public class ShortenResquestDto
    {
        public string Url { get; set; }
        //public string CustomCode { get; set; }
        //public bool IsCustomCode { get; set; }
        //public bool IsActive { get; set; } = true;
        //public int ExpirationDays { get; set; } = 30;
        public string UserId { get; set; } // Assuming this is the user ID for the owner of the shortened URL
    }
}
