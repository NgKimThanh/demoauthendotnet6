using System.ComponentModel.DataAnnotations;

namespace DotNet6Authen.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
