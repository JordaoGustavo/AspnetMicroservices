using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<Controller> logger;

        public CatalogController(IProductRepository productRepository, ILogger<Controller> logger)
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
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await productRepository.GetAsycn(id);

            if (product == null)
            {
                logger.LogError("Product with id: {0}, not found", id);
                return NotFound();
            }

            return Ok(product);
        }
    }
}
