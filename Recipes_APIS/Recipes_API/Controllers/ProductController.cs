using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models.DTO;
using Recipe.Models;
using Recipe.Repositories.Interface;
using Recipe.Repositories.Implementation;

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

    }
}
