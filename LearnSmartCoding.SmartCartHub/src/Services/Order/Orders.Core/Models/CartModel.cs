using System.ComponentModel.DataAnnotations;

namespace Orders.Core.Models
{
    public  class CartModel
    {
        public int CartId { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public required int Quantity { get; set; }
    }
}
