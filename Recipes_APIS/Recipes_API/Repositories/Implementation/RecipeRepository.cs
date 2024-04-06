using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.OpenApi.Services;
using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.Interface;

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
            //hahahahah
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
    }
}
