using Microsoft.EntityFrameworkCore;
using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.Interface;

namespace Recipe.Repositories.Implementation
{
    public class RecipeTypeRepository : IRecipeTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public RecipeTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<RecipeType?> GetByIdAsync(int id)
        {
            return await _db.RecipeTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<RecipeType>?> GetAllTypes()
        {
            return await _db.RecipeTypes.ToListAsync();
        }
    }
}
