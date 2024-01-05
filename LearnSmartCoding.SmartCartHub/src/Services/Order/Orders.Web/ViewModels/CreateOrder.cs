using Orders.Core.Entities;
using Orders.Core.Models;
using Orders.Service;
using Orders.Web.Common;
using System.ComponentModel.DataAnnotations;

namespace Orders.Web.ViewModels
{
    public class CreateOrder : AbstractValidatableObject
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public required List<OrderItemModel> OrderItems { get; set; }

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
    CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();            
            var productService = validationContext.GetService<IProductService>();

            var productIds = OrderItems.Select(s => s.ProductId).Distinct().ToList();
            var invalidProductIds = await productService.GetInvalidProductIdsAsync(productIds);

            if (invalidProductIds.Any())
            {
                errors.Add(new ValidationResult($"One or more ProductId {invalidProductIds} doesn't exist", new[] { "ProductId" }));
            }

            //validate quantity as it is possible to go to OutOfStack when order is placed.

            return errors;
        }
    }

}
