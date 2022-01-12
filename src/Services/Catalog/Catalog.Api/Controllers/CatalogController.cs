using Catalog.Api.Entities;
using Catalog.Api.Infrastructure.Interfaces;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ILoggerAdapter<Controller> logger;

        public CatalogController(IProductRepository productRepository, ILoggerAdapter<Controller> logger)
        {
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productRepository.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await productRepository.GetAsycn(id);

            if (product == null)
            {
                logger.LogError(message: "Product with id: {0}, not found", id);
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("[action]/{category}", Name = "GetProductsByCategory")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await productRepository.GetProductsByCategoryAsync(category);

            if (products == null)
            {
                logger.LogError(message: "Products with category: {0}, not found", category);
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("[action]/{name}", Name = "GetProductsByName")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductsByName(string name)
        {
            var products = await productRepository.GetProductsByNameAsync(name);

            if (products == null)
            {
                logger.LogError(message: "Products with name: {0}, not found", name);
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            await productRepository.CreateAsync(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut()]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var productUpdated = await productRepository.UpdateAsync(product);

            return Ok(productUpdated);
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var productDeleted = await productRepository.DeleteAsync(id);

            return Ok(productDeleted);
        }
    }
}
