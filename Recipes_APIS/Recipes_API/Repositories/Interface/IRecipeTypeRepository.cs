using Recipe.Models;

namespace Recipe.Repositories.Interface
{
    public interface IRecipeTypeRepository
    {
        Task<RecipeType?> GetByIdAsync(int id);
    }
}
