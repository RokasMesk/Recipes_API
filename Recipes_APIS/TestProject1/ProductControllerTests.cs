using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Recipe.Controllers;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;

namespace Recipes.Tests
{
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _controller = new ProductController(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, ProductName = "Product 1" },
                new Product { Id = 2, ProductName = "Product 2" }
            };
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsInstanceOf<List<ProductDTO>>(okResult.Value);
            var products = okResult.Value as List<ProductDTO>;
            ClassicAssert.AreEqual(expectedProducts.Count, products.Count);
            for (int i = 0; i < expectedProducts.Count; i++)
            {
                ClassicAssert.AreEqual(expectedProducts[i].Id, products[i].Id);
                ClassicAssert.AreEqual(expectedProducts[i].ProductName, products[i].ProductName);
            }
        }

        [Test]
        public async Task GetProductById_WithValidId_ReturnsOkResult_WithProductDTO()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, ProductName = "Product 1" };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsInstanceOf<ProductDTO>(okResult.Value);
            var product = okResult.Value as ProductDTO;
            ClassicAssert.AreEqual(expectedProduct.Id, product.Id);
            ClassicAssert.AreEqual(expectedProduct.ProductName, product.ProductName);
        }

        [Test]
        public async Task GetProductById_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidProductId = 999; // Assume this ID does not exist in the repository
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(invalidProductId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(invalidProductId);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetProductById_WithInvalidProductName_ReturnsNotFoundResult()
        {
            // Arrange
            var productId = 999; // Assume this ID does not exist in the repository
            var expectedProduct = new Product { Id = productId, ProductName = null };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
