using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Recipe.Controllers;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;
using System.Runtime.Serialization;
using System.Threading.Tasks;
namespace Recipe.Tests.Controllers
{
    public class UserControllerTests
    {
        [Test]
        public async Task Login_ValidCredentials_ReturnsOkObjectResult()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>());
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object, Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null);
            var mockTokenRepository = new Mock<ITokenRepository>();
            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
            var roles = new List<string>() { "User" };
            var loginRequest = new LoginRequestDTO { Identifier = "testuser", Password = "password" };
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
           // mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).Returns()Task.FromResult(roles));
            mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));
            mockTokenRepository.Setup(x => x.CreateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>())).Returns("sample-jwt-token");

            var controller = new UserController(mockUserManager.Object, mockSignInManager.Object, mockTokenRepository.Object);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOf(typeof(LoginResponseDTO), okResult.Value);
            var response = okResult.Value as LoginResponseDTO;
            Assert.AreEqual(user.UserName, response.Username);
            Assert.AreEqual(user.Email, response.Email);
            Assert.AreEqual(roles, response.Roles);
            Assert.NotNull(response.Token);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>());
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(mockUserManager.Object, Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null);
            var mockTokenRepository = new Mock<ITokenRepository>();
            var loginRequest = new LoginRequestDTO { Identifier = "testuser", Password = "wrongpassword" };
            mockSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var controller = new UserController(mockUserManager.Object, mockSignInManager.Object, mockTokenRepository.Object);

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult.Value);
            Assert.IsInstanceOf(typeof(SerializableError), badRequestResult.Value);
            
        }

    }
}
