using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Azure.Core;
using Recipe.Repositories.Interface;
#pragma warning disable CS8601
#pragma warning disable CS8604
using Recipe.Models.DTO;
using Recipe.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IRecipeRepository _recipeRepository;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenRepository tokenRepository, IRecipeRepository recipeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenRepository = tokenRepository;
            _recipeRepository = recipeRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    IsAdmin = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok("Registration successful.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return BadRequest("Invalid model state.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Identifier);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(model.Identifier);
                }

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            Username = user.UserName,
                            Email = user.Email,
                            Roles = roles.ToList(),
                            Token = jwtToken,
                            UserId = user.Id
                        };
                        return Ok(response);
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
            return BadRequest("Invalid model state.");
        }

        [HttpGet]
        [Route("check-registration")]
        public async Task<IActionResult> CheckRegistration([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Ok("User is registered.");
            }
            return NotFound("User is not registered.");
        }

        [HttpPost("users/{userId}/favorites/{recipeId}")]
        public async Task<IActionResult> AddFavorite(string userId, int recipeId)
        {
            // Check if the recipe exists 
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            await _recipeRepository.AddRecipeToFavoritesAsync(userId, recipeId);

            return Ok("Recipe added to favorites");
        }

        [HttpDelete("users/{userId}/favorites/{recipeId}")]
        public async Task<IActionResult> RemoveFavorite(string userId, int recipeId)
        {
            await _recipeRepository.RemoveRecipeFromFavoritesAsync(userId, recipeId);

            return Ok("Recipe removed from favorites");
        }

        [HttpGet("users/{userId}/favorites")]
        public async Task<IActionResult> GetAllFavorites(string userId)
        {

            var favoriteRecipes = await _recipeRepository.GetFavoriteRecipesForUserAsync(userId);

            // Convert to RecipeDTOs 

            var response = favoriteRecipes.Select(recipe => new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ShortDescription = recipe.ShortDescription,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                Preparation = recipe.Preparation,
                SkillLevel = recipe.SkillLevel,
                TimeForCooking = recipe.TimeForCooking,
                Type = new RecipeType
                {
                    Id = recipe.Type.Id,
                    Type = recipe.Type.Type
                },
                Products = recipe.Products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    ProductName = x.ProductName
                }).ToList(),
                RecipeCreatorUserName = recipe.User.UserName,
            });

            return Ok(response);

        }
        [HttpPost]
        [Route("change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserEmail); // Get the current user from the request context
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // Change the user's password
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok("Password changed successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return BadRequest("Invalid model state.");
        }
    }
}
