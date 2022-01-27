using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache ?? throw new ArgumentException(nameof(distributedCache));
        }

        public async Task<ShoppingCart?> Get(string userName)
        {
            var basket = await distributedCache.GetStringAsync(userName);
            
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);  
        }

        public async Task<ShoppingCart?> Update(ShoppingCart basket)
        {
            var basketJson = JsonConvert.SerializeObject(basket);
            await distributedCache.SetStringAsync(basket.UserName, basketJson);

            return await Get(basket.UserName);
        }

        public async Task Delete(string userName)
        {
            await distributedCache.RemoveAsync(userName);
        }
    }
}
