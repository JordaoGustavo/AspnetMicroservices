using Basket.API.Entities;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> Get(string userName);
        Task<ShoppingCart?> Update(ShoppingCart basket);
        Task Delete(string userName);
    }
}
