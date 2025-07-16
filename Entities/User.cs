using System.ComponentModel.DataAnnotations;

namespace URLShortenerApiApplication.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Token { get; set; }



        // Navigation property for related URLs
        public ICollection<URL> Urls { get; set; } = new List<URL>();

    }
}
