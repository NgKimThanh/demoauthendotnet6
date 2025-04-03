using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNet6Authen.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Password { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? TokenCreated { get; set; }

        public DateTime? TokenExpires { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Role { get; set; }

        public virtual ICollection<UserRefreshToken> UserRefreshToken { get; set; }
    }
}
