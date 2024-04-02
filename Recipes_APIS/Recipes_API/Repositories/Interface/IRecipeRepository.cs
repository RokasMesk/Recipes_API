using Recipe.Models;

namespace Recipe.Repositories.Interface
{
    public interface IRecipeRepository
    {
        Task<Recipee>CreateAsync(Recipee recipee);
        Task<IEnumerable<Recipee>> GetAllAsync();
        Task<Recipee?> GetByIdAsync(int id);
        Task<Recipee?> UpdateAsync(Recipee recipee);
        Task<Recipee?> DeleteAsync(int id);
        Task<IEnumerable<Recipee?>> SearchByTitleAsync(string title);
        Task<IEnumerable<Recipee?>> GetRecipesByUserId(string id);
    }
}
