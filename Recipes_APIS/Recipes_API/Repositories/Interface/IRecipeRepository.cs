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

        Task AddRecipeToFavoritesAsync(string userId, int recipeId);
        Task RemoveRecipeFromFavoritesAsync(string userId, int recipeId);
        Task<List<Recipee>> GetFavoriteRecipesForUserAsync(string userId); // Assuming you want Recipee objects
        Task<List<Recipee>> SearchBySelectedProductNamesAsync(List<string> selectedProductNames);
        Task AddRatingAsync(string userId, int recipeId, float rating);
        Task<float> GetAverageRatingAsync(string userId, int recipeId);
        Task AddCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByRecipeIdAsync(int recipeId);
        Task<IEnumerable<Recipee>> GetAllNonVerifiedRecipes();
        Task<Recipee?> UpdateVerifiedAsync(Recipee recipe);
    }
}
