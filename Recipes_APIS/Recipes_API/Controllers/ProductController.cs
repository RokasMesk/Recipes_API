using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models.DTO;
using Recipe.Models;
using Recipe.Repositories.Interface;
using Recipe.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> UpdateVerificationForProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            var updatedProduct =await  _productRepository.UpdateVerifiedAsync(product);
            if (updatedProduct != null)
            {
                var response = new ProductDTO
                {
                    Id = updatedProduct.Id,
                    ProductName = updatedProduct.ProductName
                };
                return Ok(response);
			}
            return NotFound();
            

        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            // convert domain model to DTO
            var response = new List<ProductDTO>();
            foreach (var product in products)
            {
                if (product.ProductName != null)
                {
                    response.Add(new ProductDTO
                    {
                        Id = product.Id,
                        ProductName = product.ProductName
                    });
                }
            }
            return Ok(response);
        }
		// GET: api/Product/nonverified
		[HttpGet("nonverified")]
		public async Task<IActionResult> GetNonVerifiedProducts()
		{
			var nonVerifiedProducts = await _productRepository.GetAllNonVerifiedProducts();
			// convert domain model to DTO
			var response = new List<ProductDTO>();
			foreach (var product in nonVerifiedProducts)
			{
				if (product.ProductName != null)
				{
					response.Add(new ProductDTO
					{
						Id = product.Id,
						ProductName = product.ProductName
					});
				}
			}
			return Ok(response);
		}
		[HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.ProductName == null) // Modified condition
            {
                return NotFound();
            }
            var response = new ProductDTO
            {
                Id = product.Id,
                ProductName = product.ProductName
            };
            return Ok(response);
        }
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO request)
		{

			var procuct = new Product
			{
				ProductName = request.ProductName

			};
			var productFromDb = await _productRepository.CreateAsync(procuct);
            var response = new ProductDTO
            {
                Id = productFromDb.Id,
                ProductName=productFromDb.ProductName

			};
			return Ok(response);
		}

	}
}
