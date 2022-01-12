using Catalog.Api.Entities;

namespace Catalog.Api.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetAsycn(string id);

        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);

        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);

        Task CreateAsync(Product product);

        Task<bool> UpdateAsync(Product product);

        Task<bool> DeleteAsync(string id);
    }
}
