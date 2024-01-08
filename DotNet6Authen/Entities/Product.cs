using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DotNet6Authen.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(250)]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int? GroupOfProductID { get; set; }
        [ForeignKey("GroupOfProductID")]

        public GroupOfProduct GroupOfProduct { get; set; }
    }
}
