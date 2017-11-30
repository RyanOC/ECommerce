using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Model;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using ECommerce.ProductCatalog;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductCatalogService _catalogService;
        private readonly ITestRepo _someRepo;

        public ProductsController(ITestRepo someRepo)
        {
            _someRepo = someRepo;

            _catalogService = ServiceProxy.Create<IProductCatalogService>(
                new Uri("fabric:/ECommerce/ECommerce.ProductCatalog"),
                new ServicePartitionKey(0));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            try
            {
                var v = _someRepo.GetSomething();

                IEnumerable<Product> allProducts = await _catalogService.GetAllProducts();

                return allProducts.Select(p => new ApiProduct
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    IsAvailable = p.Availability > 0
                });
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }

        [HttpPost]
        public async Task Post([FromBody] ApiProduct product)
        {
            var newProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };

            await _catalogService.AddProduct(newProduct);
        }
    }
}
