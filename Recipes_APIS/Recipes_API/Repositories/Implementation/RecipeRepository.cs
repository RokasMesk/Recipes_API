using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

        //public Task<Recipee?> DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<Recipee>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Recipee?> GetByIdAsync(int id)
        {
            return await _db.Recipes.Include(x=> x.Products).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Recipee?> UpdateAsync(Recipee recipee)
        {
            var existingRecipe = await _db.Recipes.Include(x => x.Products).
                FirstOrDefaultAsync(x => x.Id == recipee.Id);
            if (existingRecipe == null)
            {
                return null;
            }
            _db.Entry(existingRecipe).CurrentValues.SetValues(recipee);
            existingRecipe.Products = recipee.Products;
            await _db.SaveChangesAsync();
            return recipee;
        }
    }
}
