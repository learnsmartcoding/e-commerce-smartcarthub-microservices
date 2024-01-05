using Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Core.Models
{
    public partial class WishlistModel
    {
        public int WishlistId { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "ProductId is required.")]
        public required int ProductId { get; set; }

        public WishlistProductModel? Product { get; set; }

    }

    public class WishlistProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }
        
    }
}
