using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Core.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string ProductDescription { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int? CategoryId { get; set; }
        public List<ProductImageModel>? ProductImages { get; set; }

    }

    public class ProductImageModel
    {
        public int ImageId { get; set; }

        public int? ProductId { get; set; }

        public string ImageUrl { get; set; } = null!;
        
    }

    public class ProductReviewModel
    {
        public int ReviewId { get; set; }

        public int ProductId { get; set; }

        public int? UserId { get; set; }

        public int Rating { get; set; }

        public string? ReviewText { get; set; }= null!;

        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
