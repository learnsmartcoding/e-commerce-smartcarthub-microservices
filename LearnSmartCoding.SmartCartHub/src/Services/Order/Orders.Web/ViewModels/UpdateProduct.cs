using Products.Service;
using System.ComponentModel.DataAnnotations;

namespace Orders.Web.RequestModels
{
    public class UpdateProduct: CreateProduct
    {
        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
      CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();            
            var productService = validationContext.GetService<IProductService>();

            var product = await productService.GetProductByIdAsync(ProductId);

            if (product == null)
            {
                errors.Add(new ValidationResult($"Product Id {ProductId} doesn't exist", new[] { nameof(ProductId) }));
            }

            errors.AddRange(await base.ValidateAsync(validationContext, cancellation));

            return errors;
        }
    }
}
