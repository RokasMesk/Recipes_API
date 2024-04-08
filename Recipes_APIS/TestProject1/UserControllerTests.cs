using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Recipe.Controllers;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Recipe.Tests.Controllers
{
    public class UserControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private Mock<ITokenRepository> _mockTokenRepository;
        private Mock<IRecipeRepository> _mockRecipeRepository;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object, // Provide UserManager<ApplicationUser>
                Mock.Of<IHttpContextAccessor>(), // Provide HttpContextAccessor
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), // Provide IUserClaimsPrincipalFactory<ApplicationUser>
                null, // Configure IdentityOptions if needed
                null, // Configure ILogger<SignInManager<ApplicationUser>> if needed
                null, // Configure IAuthenticationSchemeProvider if needed
                null); // Configure IUserConfirmation<ApplicationUser> if needed

            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockRecipeRepository = new Mock<IRecipeRepository>();
            _controller = new UserController(_mockUserManager.Object, _mockSignInManager.Object, _mockTokenRepository.Object, _mockRecipeRepository.Object);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOkObjectResult()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
            var model = new LoginRequestDTO { Identifier = "testuser", Password = "password123" };
            _mockUserManager.Setup(m => m.FindByNameAsync(model.Identifier)).ReturnsAsync(user);
            _mockSignInManager.Setup(m => m.PasswordSignInAsync(user.UserName, model.Password, false, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            var roles = new List<string> { "User" };
            _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);
            _mockTokenRepository.Setup(m => m.CreateJwtToken(user, roles)).Returns("mockJwtToken");

            // Act
            var result = await _controller.Login(model);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            ClassicAssert.IsInstanceOf<LoginResponseDTO>(okResult.Value);
            var response = (LoginResponseDTO)okResult.Value;
            Assert.That(response.Username, Is.EqualTo(user.UserName));
            Assert.That(response.Email, Is.EqualTo(user.Email));
            CollectionAssert.AreEqual(roles, response.Roles);
            Assert.That(response.Token, Is.EqualTo("mockJwtToken"));
            Assert.That(response.UserId, Is.EqualTo(user.Id));
        }

        [Test]
        public async Task Login_IncorrectPassword_ReturnsBadRequest()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
            var model = new LoginRequestDTO { Identifier = "test@example.com", Password = "wrongpassword" };
            _mockUserManager.Setup(m => m.FindByEmailAsync(model.Identifier)).ReturnsAsync(user);
            _mockSignInManager.Setup(m => m.PasswordSignInAsync(user.UserName, model.Password, false, false))
                              .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Register_ValidModel_ReturnsOkObjectResult()
        {
            // Arrange
            var model = new RegisterRequestDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserManager.Setup(m => m.FindByEmailAsync(model.Email)).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(model);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Register_ExistingEmail_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var model = new RegisterRequestDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            var existingUser = new ApplicationUser { Email = model.Email };
            _mockUserManager.Setup(m => m.FindByEmailAsync(model.Email)).ReturnsAsync(existingUser);

            var result = await _controller.Register(model);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Register_IdentityErrors_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var model = new RegisterRequestDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" }
            };

            var identityResult = IdentityResult.Failed(errors.ToArray());

            _mockUserManager.Setup(m => m.FindByEmailAsync(model.Email)).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(identityResult);

            // Act
            var result = await _controller.Register(model);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            ClassicAssert.IsInstanceOf<SerializableError>(badRequestResult.Value);
            var errorsDictionary = (SerializableError)badRequestResult.Value;
            Assert.That(errorsDictionary.ContainsKey(string.Empty));
            var errorList = (string[])errorsDictionary[string.Empty];
            ClassicAssert.AreEqual(errors.Count, errorList.Length);
            foreach (var error in errors)
            {
                ClassicAssert.Contains(error.Description, errorList);
            }
        }

        [Test]
        public async Task Register_InvalidModelState_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var model = new RegisterRequestDTO();

            var controller = _controller;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            };
            controller.ModelState.AddModelError("test", "test error");

            // Act
            var result = await _controller.Register(model);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            
            ClassicAssert.IsInstanceOf<string>(badRequestResult.Value);
            var errorMessage = (string)badRequestResult.Value;
            ClassicAssert.AreEqual("Invalid model state.", errorMessage);
            
        }

        [Test]
        public async Task CheckRegistration_UserExists_ReturnsOkObjectResult()
        {
            // Arrange
            var userEmail = "test@example.com";
            var user = new ApplicationUser { Email = userEmail };
            _mockUserManager.Setup(m => m.FindByEmailAsync(userEmail)).ReturnsAsync(user);

            // Act
            var result = await _controller.CheckRegistration(userEmail);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task CheckRegistration_UserDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var userEmail = "test@example.com";
            _mockUserManager.Setup(m => m.FindByEmailAsync(userEmail)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.CheckRegistration(userEmail);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task AddFavorite_RecipeExists_ReturnsOkObjectResult()
        {
            // Arrange
            var userId = "testUser";
            var recipeId = 1;
            var recipe = new Recipee { Id = recipeId };
            _mockRecipeRepository.Setup(m => m.GetByIdAsync(recipeId)).ReturnsAsync(recipe);

            // Act
            var result = await _controller.AddFavorite(userId, recipeId);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddFavorite_RecipeDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var userId = "testUser";
            var recipeId = 1;
            _mockRecipeRepository.Setup(m => m.GetByIdAsync(recipeId)).ReturnsAsync((Recipee)null);

            // Act
            var result = await _controller.AddFavorite(userId, recipeId);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task RemoveFavorite_ReturnsOkObjectResult()
        {
            // Arrange
            var userId = "testUser";
            var recipeId = 1;

            // Act
            var result = await _controller.RemoveFavorite(userId, recipeId);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetAllFavorites_ReturnsOkObjectResultWithRecipes()
        {
            // Arrange
            var userId = "testUser";
            var recipeList = new List<Recipee>
            {
                new Recipee { Id = 1, Title = "Recipe 1", Products = new List<Product> { new Product { Id = 101, ProductName = "Ingredient 1" } } },
                new Recipee { Id = 2, Title = "Recipe 2", Products = new List<Product> { new Product { Id = 102, ProductName = "Ingredient 2" } } }
            };
            _mockRecipeRepository.Setup(m => m.GetFavoriteRecipesForUserAsync(userId)).ReturnsAsync(recipeList);

            // Act
            var result = await _controller.GetAllFavorites(userId);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.NotNull(okResult);
        }

    }
}
