using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URLShortenerApiApplication.Entities
{
    public class URL
    { 

        [Key]
        public int Id { get; set; }
        public string GneratedURL { get; set; }
        public string OriginalURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("UserId")]
        public string? UserId { get; set; } // Foreign key to the User table
    }
}
