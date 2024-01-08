using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNet6Authen.Entities
{
    [Table("GroupOfProduct")]
    public class GroupOfProduct
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(250)]
        public string GroupProductName { get; set; }
        
        public virtual ICollection<Product> Product { get; set; }
    }
}
