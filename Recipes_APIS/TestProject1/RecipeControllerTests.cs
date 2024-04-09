using NUnit.Framework;
using Moq;
using Recipe.Controllers;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework.Legacy;
namespace TestProject1
{
    public class RecipeControllerTests
    {
        private RecipeController _controller;
        private Mock<IRecipeRepository> _recipeRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IRecipeTypeRepository> _recipeTypeRepositoryMock;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;

        [SetUp]
        public void Setup()
        {
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _recipeTypeRepositoryMock = new Mock<IRecipeTypeRepository>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            var user = new ApplicationUser { UserName = "testUser", Id = "testUserId" };

            _mockUserManager.Setup(um => um.FindByNameAsync(user.UserName))
                            .ReturnsAsync(user);

            _controller = new RecipeController(_recipeRepositoryMock.Object, _productRepositoryMock.Object, _recipeTypeRepositoryMock.Object, _mockUserManager.Object);
        }

        [Test]
        public async Task CreateRecipe_ValidRequest_ReturnsOk()
        {
            // Arrange
            var createRecipeDTO = new CreateRecipeDTO
            {
                Title = "Test Recipe",
                ShortDescription = "Short description for test recipe",
                Description = "Detailed description for test recipe",
                ImageUrl = "https://example.com/test_image.jpg",
                Preparation = "Prepare the test recipe by following these steps...",
                SkillLevel = "Intermediate",
                TimeForCooking = 30, // Cooking time in minutes
                Products = new int[] { 1, 2, 3 }, // IDs of products related to this recipe
                Type = 1, // ID of recipe type
                Author = "Test"
            };

            var expectedRecipe = new Recipee
            {
                Id = 1, // Mocked ID generated after creation
                Title = createRecipeDTO.Title,
                ShortDescription = createRecipeDTO.ShortDescription,
                Description = createRecipeDTO.Description,
                ImageUrl = createRecipeDTO.ImageUrl,
                Preparation = createRecipeDTO.Preparation,
                SkillLevel = createRecipeDTO.SkillLevel,
                TimeForCooking = createRecipeDTO.TimeForCooking,
                Products = new List<Product>(), // Mocked list of related products
                Type = new RecipeType { Id = createRecipeDTO.Type }, // Mocked recipe type
            };

            // Mock product repository behavior
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync((int id) => new Product { Id = id, ProductName = $"Product {id}" });

            // Mock recipe type repository behavior
            _recipeTypeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync((int id) => new RecipeType { Id = id, Type = $"Type {id}" });

            _recipeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Recipee>()))
                                 .ReturnsAsync(expectedRecipe);

            // Act
            var result = await _controller.CreateRecipe(createRecipeDTO) as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<RecipeDTO>(result.Value);

            var recipeDTO = result.Value as RecipeDTO;
            ClassicAssert.AreEqual(expectedRecipe.Id, recipeDTO.Id);
            ClassicAssert.AreEqual(expectedRecipe.Title, recipeDTO.Title);
        }

        [Test]
        public async Task UpdateRecipeById_ValidRequest_ReturnsOk()
        {
            // Arrange
            int recipeId = 1;
            var updateRecipeDTO = new UpdateRecipeRequestDTO
            {
                Title = "Updated Test Recipe",
                ShortDescription = "Updated short description for test recipe",
                Description = "Updated detailed description for test recipe",
                ImageUrl = "https://example.com/updated_image.jpg",
                Preparation = "Updated preparation steps for the test recipe...",
                SkillLevel = "Advanced",
                TimeForCooking = 45, // Cooking time in minutes
                Products = new List<int> { 4, 5 }, // Updated IDs of products related to this recipe
                Type = 2 // Updated ID of recipe type
            };

            var expectedRecipe = new Recipee
            {
                Id = recipeId,
                Title = updateRecipeDTO.Title,
                ShortDescription = updateRecipeDTO.ShortDescription,
                Description = updateRecipeDTO.Description,
                ImageUrl = updateRecipeDTO.ImageUrl,
                Preparation = updateRecipeDTO.Preparation,
                SkillLevel = updateRecipeDTO.SkillLevel,
                TimeForCooking = updateRecipeDTO.TimeForCooking,
                Products = new List<Product>(), // Mocked list of related products
                Type = new RecipeType { Id = updateRecipeDTO.Type } // Mocked recipe type
            };

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync((int id) => new Product { Id = id, ProductName = $"Product {id}" });

            _recipeTypeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                                     .ReturnsAsync((int id) => new RecipeType { Id = id, Type = $"Type {id}" });

            _recipeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Recipee>()))
                                 .ReturnsAsync(expectedRecipe);

            // Act
            var result = await _controller.UpdateRecipeById(recipeId, updateRecipeDTO) as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<RecipeDTO>(result.Value);

            var recipeDTO = result.Value as RecipeDTO;
            ClassicAssert.AreEqual(expectedRecipe.Id, recipeDTO.Id);
            ClassicAssert.AreEqual(expectedRecipe.Title, recipeDTO.Title);
        }

        [Test]
        public async Task DeleteById_ExistingId_ReturnsOk()
        {
            // Arrange
            int recipeIdToDelete = 1;

            _recipeRepositoryMock.Setup(repo => repo.DeleteAsync(recipeIdToDelete))
                                 .ReturnsAsync(new Recipee { Id = recipeIdToDelete });

            // Act
            var result = await _controller.DeleteById(recipeIdToDelete) as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetAllRecipes_ReturnsAllRecipes()
        {
            // Arrange
            var recipes = new List<Recipee>
    {
        new Recipee
        {
            Id = 1,
            Title = "Recipe 1",
            ShortDescription = "Short description for Recipe 1",
            Description = "Detailed description for Recipe 1",
            ImageUrl = "https://example.com/recipe1_image.jpg",
            Preparation = "Preparation steps for Recipe 1...",
            SkillLevel = "Beginner",
            TimeForCooking = 20, // Cooking time in minutes for Recipe 1
            Products = new List<Product>(), // You can add related products if needed
            Type = new RecipeType { Id = 1, Type = "Type 1" }, // You can create a RecipeType object,
            User=new ApplicationUser()
        },
        new Recipee
        {
            Id = 2,
            Title = "Recipe 2",
            ShortDescription = "Short description for Recipe 2",
            Description = "Detailed description for Recipe 2",
            ImageUrl = "https://example.com/recipe2_image.jpg",
            Preparation = "Preparation steps for Recipe 2...",
            SkillLevel = "Intermediate",
            TimeForCooking = 30, // Cooking time in minutes for Recipe 2
            Products = new List<Product>(), // You can add related products if needed
            Type = new RecipeType { Id = 2, Type = "Type 2" }, // You can create a RecipeType object
             User=new ApplicationUser()
        },
    };

            _recipeRepositoryMock.Setup(repo => repo.GetAllAsync())
                                 .ReturnsAsync(recipes);

            // Act
            var result = await _controller.GetAllRecipes() as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<List<RecipeDTO>>(result.Value);

            var recipesDTO = result.Value as List<RecipeDTO>;
            ClassicAssert.AreEqual(recipes.Count, recipesDTO.Count);
        }

        [Test]
        public async Task GetRecipeById_ExistingId_ReturnsRecipe()
        {
            // Arrange
            int recipeId = 1;
            var recipe = new Recipee
            {
                Id = recipeId,
                Title = "Recipe 1",
                ShortDescription = "Short description for Recipe 1",
                Description = "Detailed description for Recipe 1",
                ImageUrl = "https://example.com/recipe1_image.jpg",
                Preparation = "Preparation steps for Recipe 1...",
                SkillLevel = "Beginner",
                TimeForCooking = 20, // Cooking time in minutes for Recipe 1
                Products = new List<Product>(), // You can add related products if needed
                Type = new RecipeType { Id = 1, Type = "Type 1" },
                User = new ApplicationUser()
            };

            _recipeRepositoryMock.Setup(repo => repo.GetByIdAsync(recipeId))
                                 .ReturnsAsync(recipe);

            // Act
            var result = await _controller.GetRecipeById(recipeId) as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<RecipeDTO>(result.Value);

            var recipeDTO = result.Value as RecipeDTO;
            ClassicAssert.AreEqual(recipe.Id, recipeDTO.Id);
            ClassicAssert.AreEqual(recipe.Title, recipeDTO.Title);
        }

        [Test]
        public async Task SearchByTitle_ValidTitle_ReturnsMatchingRecipes()
        {
            // Arrange
            string titleToSearch = "Test";
            var matchingRecipes = new List<Recipee>
    {
        new Recipee
        {
            Id = 1,
            Title = "Test Recipe 1",
            ShortDescription = "Short description for Test Recipe 1",
            Description = "Detailed description for Test Recipe 1",
            ImageUrl = "https://example.com/test_recipe1_image.jpg",
            Preparation = "Preparation steps for Test Recipe 1...",
            SkillLevel = "Beginner",
            TimeForCooking = 20,
            Products = new List<Product>(),
            Type = new RecipeType { Id = 1, Type = "Type 1" }
        },
        new Recipee
        {
            Id = 2,
            Title = "Test Recipe 2",
            ShortDescription = "Short description for Test Recipe 2",
            Description = "Detailed description for Test Recipe 2",
            ImageUrl = "https://example.com/test_recipe2_image.jpg",
            Preparation = "Preparation steps for Test Recipe 2...",
            SkillLevel = "Intermediate",
            TimeForCooking = 30,
            Products = new List<Product>(),
            Type = new RecipeType { Id = 2, Type = "Type 2" }
        },
    };

            _recipeRepositoryMock.Setup(repo => repo.SearchByTitleAsync(titleToSearch))
                                 .ReturnsAsync(matchingRecipes);

            // Act
            var result = await _controller.SearchByTitle(titleToSearch) as ObjectResult;

            // Assert
            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<List<Recipee>>(result.Value);

            var recipes = result.Value as List<Recipee>;
            ClassicAssert.AreEqual(matchingRecipes.Count, recipes.Count);
            ClassicAssert.AreEqual(matchingRecipes[0].Id, recipes[0].Id);
            ClassicAssert.AreEqual(matchingRecipes[1].Title, recipes[1].Title);
        }


        [Test]
        public async Task GetRecipesByUserName_UserExists_ReturnsRecipes()
        {
            string userName = "testUser";
            var user = new ApplicationUser { UserName = userName, Id = "testUserId" };
            var recipes = new List<Recipee>
    {
        new Recipee
        {
            Id = 1,
            Title = "Recipe 1",
            ShortDescription = "Short description for Recipe 1",
            Description = "Detailed description for Recipe 1",
            ImageUrl = "https://example.com/recipe1_image.jpg",
            Preparation = "Preparation steps for Recipe 1...",
            SkillLevel = "Beginner",
            TimeForCooking = 20,
            Products = new List<Product>(),
            Type = new RecipeType { Id = 1, Type = "Type 1" }
        },
        new Recipee
        {
            Id = 2,
            Title = "Recipe 2",
            ShortDescription = "Short description for Recipe 2",
            Description = "Detailed description for Recipe 2",
            ImageUrl = "https://example.com/recipe2_image.jpg",
            Preparation = "Preparation steps for Recipe 2...",
            SkillLevel = "Intermediate",
            TimeForCooking = 30,
            Products = new List<Product>(),
            Type = new RecipeType { Id = 2, Type = "Type 2" }
        },
    };

            _recipeRepositoryMock.Setup(repo => repo.GetRecipesByUserId(user.Id)).ReturnsAsync(recipes);

            var result = await _controller.GetRecipesByUserName(userName) as ObjectResult;

            ClassicAssert.NotNull(result);
            ClassicAssert.AreEqual(200, result.StatusCode);
            ClassicAssert.IsInstanceOf<List<RecipeDTO>>(result.Value);

            var recipesDTO = result.Value as List<RecipeDTO>;

            ClassicAssert.AreEqual(recipes.Count, recipesDTO.Count);
            ClassicAssert.AreEqual(recipes[0].Id, recipesDTO[0].Id);
            ClassicAssert.AreEqual(recipes[1].Title, recipesDTO[1].Title);
        }


    }
}
