using Products.Service;
using Products.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Products.Web.RequestModels
{
  
    public class CreateProduct: ProductViewModel
    {
        public List<IFormFile>? Images { get; set; }
        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
      CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var categoryService = validationContext.GetService<ICategoryService>();
            var productService = validationContext.GetService<IProductService>();

            var category = await categoryService.GetCategoryByIdAsync(CategoryId);            

            if (category == null)
            {
                errors.Add(new ValidationResult($"Category id {CategoryId} doesn't exist", new[] { nameof(CategoryId) }));
            }

            if (Price > 5000)
            {
                errors.Add(new ValidationResult($"Price cannot be more than $5000. Entered price is {Price} ", new[] { nameof(Price) }));
            }

            return errors;
        }
    }
}
