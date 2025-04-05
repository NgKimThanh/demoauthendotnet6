using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNet6Authen.Entities
{
    [Table("UserRefreshTokens")]
    public class UserRefreshToken
    {
        [Key]
        public int ID { get; set; }

        public int? UserID { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string DeviceInfo { get; set; } // Lưu thông tin trình duyệt

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
