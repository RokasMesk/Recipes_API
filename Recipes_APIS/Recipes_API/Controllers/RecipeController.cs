﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Implementation;
using Recipe.Repositories.Interface;
using System.Data;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRecipeTypeRepository _recipeTypeRepository;
        public RecipeController(IRecipeRepository recipeRepository, IProductRepository productRepo, IRecipeTypeRepository recipeTypeRepository)
        {
            _recipeRepository = recipeRepository;
            _productRepository = _productRepository = productRepo;
            _recipeTypeRepository=recipeTypeRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDTO request)
        {
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
                Type = new RecipeType()

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
                    ProductName = x.ProductName
                }).ToList()

            };
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
                    ProductName = x.ProductName
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
                        ProductName = x.ProductName
                    }).ToList()
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
                    ProductName = x.ProductName
                }).ToList()
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
    }
}
