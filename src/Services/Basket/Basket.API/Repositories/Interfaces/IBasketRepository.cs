using Basket.API.Entities;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetAsync(string userName);
        Task<ShoppingCart?> UpdateAsync(ShoppingCart basket);
        Task DeleteAsync(string userName);
    }
}
