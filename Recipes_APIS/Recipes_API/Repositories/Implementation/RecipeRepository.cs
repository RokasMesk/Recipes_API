using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi.Services;
using Recipe.Data;
using Recipe.Models;
using Recipe.Models.DTO;
using Recipe.Repositories.Interface;
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8613
#pragma warning disable S1125

namespace Recipe.Repositories.Implementation
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _db;
        public RecipeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Recipee> CreateAsync(Recipee recipee)
        {
            await _db.Recipes.AddAsync(recipee);
            await _db.SaveChangesAsync();
            return recipee;
        }

        public async Task<Recipee?> DeleteAsync(int id)
        {
            var existingRecipe = await _db.Recipes.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRecipe != null)
            {
                _db.Recipes.Remove(existingRecipe);
                await _db.SaveChangesAsync();
                return existingRecipe;
            }
            return null;
        }


        public async Task<IEnumerable<Recipee>> GetAllAsync()
        {
            return await _db.Recipes.Include(x => x.User).Include(x => x.Products).Include(x=> x.Type).ToListAsync();

        }

        public async Task<Recipee?> GetByIdAsync(int id)
        {
            return await _db.Recipes.Include(x => x.User).Include(x=> x.Products).Include(x=> x.Type).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Recipee?> UpdateAsync(Recipee recipee)
        {
            var existingRecipe = await _db.Recipes.Include(x => x.Products).Include(x=> x.Type).
                FirstOrDefaultAsync(x => x.Id == recipee.Id);
            if (existingRecipe == null)
            {
                return null;
            }
            _db.Entry(existingRecipe).CurrentValues.SetValues(recipee);
            existingRecipe.Products = recipee.Products;
            existingRecipe.Type = recipee.Type;
            await _db.SaveChangesAsync();
            return recipee;
        }

        public async Task<IEnumerable<Recipee>> SearchByTitleAsync(string title)
        {
            return await _db.Recipes.Where(recipee => recipee.Title.Contains(title)).ToListAsync();
        }

        public async Task<IEnumerable<Recipee?>> GetRecipesByUserId(string id)
        {
            var recipes = await _db.Recipes.Include(x => x.Products).Include(x => x.Type).Where(r => r.User.Id == id).ToListAsync();
            return recipes;
        }

        public async Task AddRecipeToFavoritesAsync(string userId, int recipeId)
        {
            var favoriteRecord = new UserFavoriteRecipe
            {
                UserId = userId,
                RecipeeId = recipeId
            };

            _db.UserFavoriteRecipes.Add(favoriteRecord);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRecipeFromFavoritesAsync(string userId, int recipeId)
        {
            var favoriteToRemove = await _db.UserFavoriteRecipes
                                           .FirstOrDefaultAsync(ufr => ufr.UserId == userId && ufr.RecipeeId == recipeId);

            if (favoriteToRemove != null)
            {
                _db.UserFavoriteRecipes.Remove(favoriteToRemove);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<Recipee>> GetFavoriteRecipesForUserAsync(string userId)
        {
            return await _db.UserFavoriteRecipes
                .Where(ufr => ufr.UserId == userId)
                .Include(ufr => ufr.Recipee)
                    .ThenInclude(recipe => recipe.Type)  // Include Type
                .Include(ufr => ufr.Recipee)
                    .ThenInclude(recipe => recipe.Products) // Include Products  
                .Include(ufr => ufr.Recipee.User)  // Include User
                .Select(ufr => ufr.Recipee)
                .ToListAsync();

        }
        public async Task<List<Recipee>> SearchBySelectedProductNamesAsync(List<string> selectedProductNames)
        {
            var recipes = await _db.Recipes.Where(recipe => recipe.Products.Any(product => selectedProductNames.Contains(product.ProductName))).Include(x=>x.Type).Include(x=>x.User).Select(x=>x)
                .ToListAsync();


            return recipes;
        }

        public async Task AddRatingAsync(string userId, int recipeId, float rating)
        {
            var recipe = await _db.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                throw new ArgumentException($"Recipe with ID {recipeId} not found.");
            }

            var existingRating = await _db.UserRecipeRatings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeeId == recipeId);

            if (existingRating != null)
            {
                existingRating.RecipeRating = rating;
                var totalRating = recipe.Rating * recipe.RatedPeopleCount;
                totalRating -= recipe.Rating;
                totalRating += rating;
                recipe.Rating = totalRating / recipe.RatedPeopleCount;
            }
            else
            {
                recipe.RatedPeopleCount++;
                var totalRating = recipe.Rating * (recipe.RatedPeopleCount - 1); // Exclude the new rating
                totalRating += rating; // Add new rating
                recipe.Rating = totalRating / recipe.RatedPeopleCount;

                _db.UserRecipeRatings.Add(new UserRecipeRating
                {
                    UserId = userId,
                    RecipeeId = recipeId,
                    RecipeRating = rating
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task<float> GetAverageRatingAsync(string userId, int recipeId)
        {
            var ratings = await _db.UserRecipeRatings
                .Where(r => r.RecipeeId == recipeId)
                .Where(r => r.UserId == userId)
                .FirstOrDefaultAsync();
            if (ratings == null)
                return -1;

            return ratings.RecipeRating;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRecipeIdAsync(int recipeId)
        {
            return await _db.Comments
                .Where(c => c.RecipeId == recipeId)
                .ToListAsync();
        }
        public async Task<Recipee?> UpdateVerifiedAsync(Recipee recipe)
        {
            var existingRecipe = await _db.Recipes.FirstOrDefaultAsync(x => x.Id == recipe.Id);
            if (existingRecipe == null)
            {
                return null;
            }

            existingRecipe.IsVerified = true;
            // Update the verified fields
            existingRecipe.Title = recipe.Title;
            existingRecipe.ShortDescription = recipe.ShortDescription;
            existingRecipe.Description = recipe.Description;
            existingRecipe.ImageUrl = recipe.ImageUrl;
            // Update other properties as needed

            await _db.SaveChangesAsync();
            return existingRecipe;
        }

        public async Task<IEnumerable<Recipee>> GetAllNonVerifiedRecipes()
        {
			return await _db.Recipes.Where(x=> !x.IsVerified).Include(x => x.User).Include(x => x.Products).Include(x => x.Type).ToListAsync();
		}

    }
}
