using Microsoft.EntityFrameworkCore;
using Recipe.Data;
using Recipe.Models;
using Recipe.Repositories.Interface;
#pragma warning disable CS8604
#pragma warning disable S1125

namespace Recipe.Repositories.Implementation
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> DeleteAsync(int id)
        {
            var existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct != null)
            {
                _db.Products.Remove(existingProduct);
                await _db.SaveChangesAsync();
                return existingProduct;
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.Where(x=> x.IsVerified==true).ToListAsync();
        }
		public async Task<IEnumerable<Product>> GetAllNonVerifiedProducts()
		{
			return await _db.Products.Where(x => x.IsVerified==false).ToListAsync();
		}
		public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await _db.Products.
                FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProduct == null)
            {
                return null;
            }
            _db.Entry(existingProduct).CurrentValues.SetValues(product);
           
            await _db.SaveChangesAsync();
            return product;
        }
        public async Task<Product?> UpdateVerifiedAsync(Product product)
        {
            var existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id==product.Id);
            if (existingProduct == null)
            {
                return null;
            }
            existingProduct.IsVerified= true;
			_db.Entry(existingProduct).CurrentValues.SetValues(product);

			await _db.SaveChangesAsync();
			return existingProduct;
		}

		public async Task<List<Product>> SearchBySelectedProductNamesAsync(List<string> selectedProductNames)
        {
            // Assuming there's a DbSet<Product> in your DbContext named Products
            return await _db.Products
                .Where(p => selectedProductNames.Contains(p.ProductName))
                .ToListAsync();
        }
    }
}
