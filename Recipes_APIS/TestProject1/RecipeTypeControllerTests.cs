using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Recipe.Controllers;
using Recipe.Models.DTO;
using Recipe.Repositories;
using Recipe.Repositories.Interface;
using NUnit.Framework.Legacy;

namespace Recipe.Type.Controllers
{
    public class RecipeTypeControllerTests
    {
        private Mock<IRecipeTypeRepository> _mockRecipeTypeRepository;
        private RecipeTypeController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRecipeTypeRepository = new Mock<IRecipeTypeRepository>();
            _controller = new RecipeTypeController(_mockRecipeTypeRepository.Object);
        }

        [Test]
        public async Task GetAllRecipeTypesAsync_ReturnsOkObjectResultWithData()
        {
            // Arrange
            var recipeTypes = new List<RecipeTypeDTO>
            {
                new RecipeTypeDTO { Id = 1, Type = "Type1" },
                new RecipeTypeDTO { Id = 2, Type = "Type2" }
            };
            _mockRecipeTypeRepository.Setup(repo => repo.GetAllTypes()).ReturnsAsync(recipeTypes);

            // Act
            var result = await _controller.GetAllRecipeTypesAsync();

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.IsNotNull(okResult.Value);
            var responseData = okResult.Value as List<RecipeTypeDTO>;
            ClassicAssert.IsNotNull(responseData);
            ClassicAssert.AreEqual(recipeTypes.Count, responseData.Count);
            // You can further assert specific data if needed
        }

        [Test]
        public async Task GetAllRecipeTypesAsync_ReturnsOkObjectResultWithNoData()
        {
            // Arrange
            List<RecipeTypeDTO> recipeTypes = null;
            _mockRecipeTypeRepository.Setup(repo => repo.GetAllTypes()).ReturnsAsync(recipeTypes);

            // Act
            var result = await _controller.GetAllRecipeTypesAsync();

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.AreEqual(200, okResult.StatusCode);
            ClassicAssert.IsNull(okResult.Value);
        }
    }
}
