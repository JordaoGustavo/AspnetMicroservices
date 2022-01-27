using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentException(nameof(basketRepository));
        }
        
        [HttpGet("{userName}", Name = "Get")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string userName)
        {
            var basket = await basketRepository.GetAsync(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> Update([FromBody] ShoppingCart basket)
        {
            // TODO : Communicate with Discount.Grpc
            // and Calculate latest prices of product into shopping cart
            // consume Discount Grpc
          
            return Ok(await basketRepository.UpdateAsync(basket));
        }

        [HttpDelete("{userName}", Name = "Delete")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string userName)
        {
            await basketRepository.DeleteAsync(userName);
            return NoContent();
        }
    }
}
