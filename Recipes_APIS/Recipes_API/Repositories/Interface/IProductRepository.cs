using Recipe.Models;

namespace Recipe.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(int id);
    }
}
