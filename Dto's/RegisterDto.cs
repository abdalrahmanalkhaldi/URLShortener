using System.ComponentModel.DataAnnotations;

namespace URLShortenerApiApplication
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
