using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Implementation;
using Recipe.Repositories.Interface;
using System.Data;
using System.Security.Claims;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRecipeTypeRepository _recipeTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecipeController(IRecipeRepository recipeRepository, IProductRepository productRepo, IRecipeTypeRepository recipeTypeRepository, UserManager<ApplicationUser> userManager)
        {
            _recipeRepository = recipeRepository;
            _productRepository = _productRepository = productRepo;
            _recipeTypeRepository=recipeTypeRepository;
            _userManager= userManager;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDTO request)
        {
            var authorEmail = request.Author;
            if (string.IsNullOrEmpty(authorEmail))
            {
                return BadRequest("Author email is required.");
            }

            var user = await _userManager.FindByEmailAsync(authorEmail);


            var recipe = new Recipee
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Preparation = request.Preparation,
                SkillLevel = request.SkillLevel,
                TimeForCooking = request.TimeForCooking,
                Products = new List<Product>(),
                Type = new RecipeType(),
                User = new ApplicationUser()

            };

            foreach (var productId in request.Products)
            {
                var existingProduct = await _productRepository.GetByIdAsync(productId);
                if (existingProduct is not null)
                {
                    recipe.Products.Add(existingProduct);
                }
            }
            var existingType = await _recipeTypeRepository.GetByIdAsync(request.Type);
            if (existingType != null)
            {
                recipe.Type = existingType;
            }

            if (user != null)
            {
                recipe.User = user;
            }
            recipe = await _recipeRepository.CreateAsync(recipe);
            var response = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ShortDescription = recipe.ShortDescription,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                Preparation = recipe.Preparation,
                SkillLevel = recipe.SkillLevel,
                TimeForCooking = recipe.TimeForCooking,
                Type = recipe.Type,
                Products = recipe.Products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    ProductName = x.ProductName == null ? "" : x.ProductName,
                }).ToList()

            };
            return Ok(response);
        }
        [HttpGet("nonverified")]
        public async Task<IActionResult> GetNonVerifiedRecipes()
        {
            var nonVerifiedRecipes = await _recipeRepository.GetAllNonVerifiedRecipes();
            var response = new List<RecipeDTO>();

            foreach (var recipe in nonVerifiedRecipes)
            {
                if (recipe.Title != null)
                {
                    response.Add(new RecipeDTO
                    {
                        Id = recipe.Id,
                        Title = recipe.Title,
                        ShortDescription = recipe.ShortDescription,
                        Description = recipe.Description,
                        ImageUrl = recipe.ImageUrl,
                        // Map other properties as needed
                    });
                }
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateRecipeById([FromRoute] int id, UpdateRecipeRequestDTO request)
        {
            // Convert This from DTO to Domain model
            var recipe = new Recipee
            {
                Id = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                Preparation = request.Preparation,
                SkillLevel = request.SkillLevel,
                TimeForCooking = request.TimeForCooking,
                Products = new List<Product>(),
                Type = new RecipeType()
            };
            foreach (var productID in request.Products)
            {
                var existingProduct = await _productRepository.GetByIdAsync(productID);
                if (existingProduct != null)
                {
                    recipe.Products.Add(existingProduct);
                }
            }
            var existingType = await _recipeTypeRepository.GetByIdAsync(request.Type);
            if (existingType != null)
            {
                recipe.Type = existingType;
            }
            var updatedBlogPost = await _recipeRepository.UpdateAsync(recipe);
            if (updatedBlogPost == null)
            {
                return NotFound();
            }
            // Convert Domain model back to dto
            var response = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                ShortDescription = recipe.ShortDescription,
                ImageUrl = recipe.ImageUrl,
                Preparation = recipe.Preparation,
                SkillLevel = recipe.SkillLevel,
                TimeForCooking = recipe.TimeForCooking,
                Products = recipe.Products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    ProductName = x.ProductName == null ? "" : x.ProductName,
                }).ToList(),
                Type = recipe.Type
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var deletedRecipe = await _recipeRepository.DeleteAsync(id);
            if (deletedRecipe == null) return NotFound();
            return Ok(deletedRecipe);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            // convert domain model to DTO
            var response = new List<RecipeDTO>();
            foreach (var recipe in recipes)
            {
                response.Add(new RecipeDTO
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
                        ProductName = x.ProductName == null ? "" : x.ProductName,
                    }).ToList(),
                    RecipeCreatorUserName = recipe.User.UserName,
                    Rating = recipe.Rating,
                    RatedPeopleCount = recipe.RatedPeopleCount,
                }) ; 
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetRecipeById([FromRoute] int id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            var reponse = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ShortDescription = recipe.ShortDescription,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                Preparation = recipe.Preparation,
                SkillLevel = recipe.SkillLevel,
                TimeForCooking = recipe.TimeForCooking,
                Type = recipe.Type,
                Products = recipe.Products.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    ProductName = x.ProductName == null ? "" : x.ProductName,
                }).ToList(),
                RecipeCreatorUserName = recipe.User.UserName
            };
            return Ok(reponse);
        }
        [HttpGet]
        [Route("title/{title}")]
        public async Task<IActionResult> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Title is required"); // Return 400 Bad Request if title is not provided
            }

            var result = await _recipeRepository.SearchByTitleAsync(title);

            if (result == null)
            {
                return NotFound(); // Return 404 Not Found if no matching recipe is found
            }

            return Ok(result);
        }
        [HttpGet]
        [Route("user/{userName}")]
        public async Task<IActionResult> GetRecipesByUserName([FromRoute] string userName)
        {
            // Retrieve the user by user name
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"User with user name '{userName}' not found.");
            }

            // Retrieve recipes associated with the user
            var recipes = await _recipeRepository.GetRecipesByUserId(user.Id);
            if (recipes == null || !recipes.Any())
            {
                return NotFound($"No recipes found for user '{userName}'.");
            }

            var response = recipes
                .Where(recipe => recipe is not null)
                .Select(recipe => new RecipeDTO
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
                    ProductName = x.ProductName == null ? "" : x.ProductName,
                }).ToList(),
                RecipeCreatorUserName = user.UserName
            }).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return Ok(response);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchRecipes([FromBody] List<string> selectedProductNames)
        {
            if (selectedProductNames == null || !selectedProductNames.Any())
            {
                return BadRequest("No product selected");
            }

            var recipes = await _recipeRepository.SearchBySelectedProductNamesAsync(selectedProductNames);
            return Ok(recipes);
        }

        [HttpPost("Rate")]
        public async Task<IActionResult> RateRecipe([FromBody] RecipeRatingDTO ratingDTO)
        {
            await _recipeRepository.AddRatingAsync(ratingDTO.UserId, ratingDTO.RecipeId, ratingDTO.Rating);

            return Ok();
        }

        [HttpGet("GetRating")]
        public async Task<IActionResult> GetRecipeRating(string userId, int id)
        {
            var averageRating = await _recipeRepository.GetAverageRatingAsync(userId, id);

            return Ok(averageRating);
        }

        [HttpPost("{recipeId:int}/comments")]
        public async Task<IActionResult> AddComment(CommentDTO comment) 
        {
            var commentInsert = new Comment
            {
                Text = comment.Comment,
                PostedAt = DateTime.UtcNow,
                UserId = comment.UserId,
                RecipeId = comment.RecipeId,
                AuthorName = comment.AuthorName,
            };

            // Save the comment
            await _recipeRepository.AddCommentAsync(commentInsert);

            return Ok(); // Comment added successfully
        }

        [HttpGet("{recipeId:int}/comments")]
        public async Task<IActionResult> GetCommentsByRecipeId(int recipeId)
        {
            var comments = await _recipeRepository.GetCommentsByRecipeIdAsync(recipeId);

            if (comments == null || !comments.Any())
            {
                return NotFound($"No comments found for recipe with ID '{recipeId}'.");
            }

            return Ok(comments);
        }
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> UpdateVerificationForRecipe(int id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe is null)
            {
                return NotFound();
            }

            var updatedRecipe = await _recipeRepository.UpdateVerifiedAsync(recipe);

            if (updatedRecipe != null && updatedRecipe.Title != null)
            {
                var response = new RecipeDTO
                {
                    Id = updatedRecipe.Id,
                    Title = updatedRecipe.Title,
                    // Map other properties as needed
                };
                return Ok(response);
            }

            return NotFound();
        }

    }
}
