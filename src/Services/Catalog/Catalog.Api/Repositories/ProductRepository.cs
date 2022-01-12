using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Product product)
        {
            await context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            
            var deleteResult = await context
                                        .Products
                                        .DeleteOneAsync(filter);

            var moreThanOneDeleted = deleteResult.DeletedCount > 0;

            return deleteResult.IsAcknowledged &&
                   moreThanOneDeleted;
        }

        public async Task<Product> GetAsycn(string id)
        {
            return await context
                           .Products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await context
                           .Products
                           .Find(p => true)
                           .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var updateResult = await context
                                        .Products
                                        .ReplaceOneAsync(filter: f => f.Id == product.Id, replacement: product);

            var moreThanZeroModified = updateResult.ModifiedCount > 0;

            return updateResult.IsAcknowledged &&
                moreThanZeroModified;
        }
    }
}
