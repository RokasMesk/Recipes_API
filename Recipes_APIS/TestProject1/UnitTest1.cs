using NUnit.Framework;
using Moq;
using Recipe.Controllers;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestProject1
{
    public class RecipeControllerTests
    {
        private RecipeController _controller;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IRecipeTypeRepository> _recipeTypeRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _recipeTypeRepositoryMock = new Mock<IRecipeTypeRepository>();
            _controller = new RecipeController(_recipeRepositoryMock.Object, _productRepositoryMock.Object, _recipeTypeRepositoryMock.Object);
        }

        [Test]
        public async Task CreateRecipe_ValidInput_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeDTO { /* provide valid input */ };
            var recipe = new Recipee { /* create a recipe object */ };
            _recipeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Recipee>())).ReturnsAsync(recipe);

            // Act
            var result = await _controller.CreateRecipe(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }

        [Test]
        public async Task UpdateRecipeById_ValidInput_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var request = new UpdateRecipeRequestDTO { /* provide valid input */ };
            var recipe = new Recipee { /* create a recipe object */ };
            _recipeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Recipee>())).ReturnsAsync(recipe);

            // Act
            var result = await _controller.UpdateRecipeById(id, request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }

        [Test]
        public async Task DeleteById_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var recipe = new Recipee { /* create a recipe object */ };
            _recipeRepositoryMock.Setup(repo => repo.DeleteAsync(id)).ReturnsAsync(recipe);

            // Act
            var result = await _controller.DeleteById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }

        [Test]
        public async Task GetAllRecipes_ReturnsOk()
        {
            // Arrange
            var recipes = new List<Recipee> { /* create list of recipe objects */ };
            _recipeRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(recipes);

            // Act
            var result = await _controller.GetAllRecipes();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }

        [Test]
        public async Task GetRecipeById_ExistingId_ReturnsOk()
        {
            // Arrange
            var id = 1;
            var recipe = new Recipee { /* create a recipe object */ };
            _recipeRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(recipe);

            // Act
            var result = await _controller.GetRecipeById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }

        [Test]
        public async Task SearchByTitle_ValidTitle_ReturnsOk()
        {
            // Arrange
            var title = "Sample Title";
            var recipes = new List<Recipee> { /* create list of recipe objects */ };
            _recipeRepositoryMock.Setup(repo => repo.SearchByTitleAsync(title)).ReturnsAsync(recipes);

            // Act
            var result = await _controller.SearchByTitle(title);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            // Add more assertions as needed
        }
    }
}
