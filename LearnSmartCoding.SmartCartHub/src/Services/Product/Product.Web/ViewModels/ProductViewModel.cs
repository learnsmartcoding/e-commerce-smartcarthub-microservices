using Products.Core.Models;
using Products.Web.Common;
using System.ComponentModel.DataAnnotations;

namespace Products.Web.ViewModels
{
    public class ProductViewModel: AbstractValidatableObject
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProductName is required.")]
        public required string ProductName { get; set; } = null!;
        
        [Required(ErrorMessage = "ProductDescription is required.")]
        public required string ProductDescription { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public required int Quantity { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, short.MaxValue, ErrorMessage = "Category must be greater than 0.")]
        public required int CategoryId { get; set; }
        public List<ProductImageModel>? ProductImages { get; set; }
    }
}
