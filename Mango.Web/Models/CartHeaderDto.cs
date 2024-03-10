using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models.Dto
{
    public class CartHeaderDto
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }

        [NotMapped]
        public double Discount { get; set; }

        [NotMapped]
        public double CartTotal { get; set; }

        [Required]
        public string? Name { get; set; }

       

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
