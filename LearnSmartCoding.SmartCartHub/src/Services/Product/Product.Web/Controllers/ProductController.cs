using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Products.Core.Models;
using Products.Service;
using Products.Web.RequestModels;

namespace Products.Web.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController(IProductService productService, 
        IStorageService storageService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IStorageService storageService = storageService;

        /// <summary>
        /// Get a product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve.</param>
        /// <returns>The requested product.</returns>
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "ProductName")
        {
            var products = await _productService.GetAllProductsAsync(pageNumber, pageSize, sortBy);
            return Ok(products);//returns at least empty arrary if no products exist
        }

        [HttpPost(Name = "AddProduct")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> AddProduct(CreateProduct productModel )
        {
            if (productModel.Images != null)
            {
                foreach (var file in productModel.Images)
                {
                    if (!IsValidFile(file))
                    {
                        ModelState.AddModelError("ProductImages", "Invalid file format. Allowed formats: jpg, png, jpeg");
                        return BadRequest(ModelState);
                    }
                }
            }

            var product = MapToProduct(productModel);


            if (productModel.Images != null)
            {
                //Change this to store it in Azure Blob storage
                foreach (var file in productModel.Images)
                {
                    var fileName = $"{productModel.CategoryId}_{productModel.ProductName.Replace(" ", "_")}.{file.FileName.Split('.').LastOrDefault()}";
                    var filePath = "";

                    //we try to upload to azure storage, if that is not configured then it will upload to local folder

                    using (var stream = file.OpenReadStream())
                    {
                        var containerName = "products";
                        var folderName = "images";
                        filePath = await storageService.UploadImageAsync(stream, containerName, folderName, fileName);
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath = await UploadFileToLocalMachineAsync(fileName, file);
                        //filePath = await UploadFileToProjectLocationAsync(fileName, file); //you can use any one of these to store file
                    }

                    product?.ProductImages?.Add(new ProductImageModel() { ImageUrl = filePath });
                }
            }
            var addedProduct = await _productService.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { productId = addedProduct.ProductId }, addedProduct);
        }



        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> UpdateProduct(int productId, UpdateProduct productModel)
        {
            if (productId != productModel.ProductId)
            {
                ModelState.AddModelError("ProductId", "ProductId MisMatch");
                return BadRequest(ModelState);
            }

            if (productModel.Images != null)
            {
                foreach (var file in productModel.Images)
                {
                    if (!IsValidFile(file))
                    {
                        ModelState.AddModelError("ProductImages", "Invalid file format. Allowed formats: jpg, png, jpeg");
                        return BadRequest(ModelState);
                    }
                }
            }

            var product = MapToProduct(productModel);


            if (productModel.Images != null)
            {
                //Change this to store it in Azure Blob storage
                foreach (var file in productModel.Images)
                {
                    var fileName = $"{productModel.CategoryId}_{productModel.ProductName.Replace(" ", "_")}.{file.FileName.Split('.').LastOrDefault()}";
                    var filePath = "";

                    //we try to upload to azure storage, if that is not configured then it will upload to local folder

                    using (var stream = file.OpenReadStream())
                    {
                        var containerName = "products";
                        var folderName = "images";
                        filePath = await storageService.UploadImageAsync(stream, containerName, folderName, fileName);
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath = await UploadFileToLocalMachineAsync(fileName, file);
                        //filePath = await UploadFileToProjectLocationAsync(fileName, file); //you can use any one of these to store file
                    }

                    product?.ProductImages?.Add(new ProductImageModel() { ImageUrl = filePath });
                }
            }
            var updatedProductModel = await _productService.UpdateProductAsync(product);
            return Ok(updatedProductModel);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var success = await _productService.DeleteProductAsync(productId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
        private ProductModel MapToProduct(CreateProduct productModel)
        {
            var productEntity = new ProductModel
            {
                ProductName = productModel.ProductName,
                Price = productModel.Price,
                Quantity = productModel.Quantity,
                CategoryId = productModel.CategoryId,
                ProductImages = new List<ProductImageModel>()
            };
            return productEntity;
        }
        private bool IsValidFile(IFormFile file)
        {
            List<string> validFormats = new List<string>() { ".jpg", ".png", ".jpeg" };
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return validFormats.Contains(extension);
        }

        [HttpGet("images/{imageId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetProductImageByIdAsync(int imageId)
        {
            var image = await _productService.GetProductImageByIdAsync(imageId);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpGet("{productId}/images")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<IActionResult> GetImagesByProductIdAsync(int productId)
        {
            var images = await _productService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }

        [HttpPost("{productId}/images")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> AddProductImageAsync(int productId, [FromBody] ProductImageModel imageModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedImage = await _productService.AddProductImageAsync(imageModel);
            return CreatedAtAction(nameof(GetProductImageByIdAsync), new { imageId = addedImage.ImageId }, addedImage);
        }

        [HttpDelete("images/{imageId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<IActionResult> DeleteProductImageAsync(int imageId)
        {
            var isDeleted = await _productService.DeleteProductImageAsync(imageId);
            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        #region Private Methods
        private async Task<string> UploadFileToLocalMachineAsync(string fileName, IFormFile file)
        {
            var tempFolderPath = Path.GetTempPath();
            // Construct the file name based on productId and productName
            var folderPath = Path.Combine(tempFolderPath, "SmartCartHub");   

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        private async Task<string> UploadFileToProjectLocationAsync(string fileName, IFormFile file)
        {
            // Construct the file name based on productId and productName

            // Example: Save the file to a server directory
            var folderPath = Path.Combine("uploaded/products/images");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
        #endregion
    }

}
