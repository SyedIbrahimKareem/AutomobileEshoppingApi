using EShoppingAPI.Repository;
using EShoppingBusinessLibrary.ProductModels;
using Microsoft.AspNetCore.Mvc;
using EShoppingAPI.IRepository;
using EShoppingAPI.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace EShoppingAPI.Controllers
{
    [Authorize(Roles ="User")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductsControllers : ControllerBase
    {
        private ILog _log;
        public readonly IProductRepository _productServices;
        private readonly ILogger<ProductsControllers> _logger;
        public ProductsControllers(IProductRepository productServices, ILogger<ProductsControllers> logger)
        {
            _productServices = productServices;
            _log = Log.GetInstance();
            _logger = logger;
        }
        // GET: ProductControllercs

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductList()
        {
            try
            {
                var productList = await _productServices.GetProductList();
                if (productList == null)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No Product found with ID = {0}")),
                        ReasonPhrase = "Product Not Found",
                        
                    };
                    _logger.LogInformation("Product Not Found from GetProductList");
                    //throw new HttpResponseException(response);
                }
                return productList;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Exception has been thrown :", ex.Message);
                _log.LogException(ex.StackTrace.ToString());
                return Enumerable.Empty<Product>();
            }


        }

        // GET: ProductControllercs/Details/5
        [HttpGet]
        public async Task<Product> GetProductDetailByName(string name)
        {
            _logger.LogInformation("Product name method calling...");
            var productList = await _productServices.GetProductByName(name);
            _logger.LogInformation("returning the list of products from name related:"+" "+name);
            return productList;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductListSearch(string searchValue)
        {
            _logger.LogInformation("Product search method calling...");
            var productList = await _productServices.GetProductList();
            _logger.LogInformation("Fetching product details from the DB...");
            productList =productList.Where(x => x.productName.Contains(searchValue)).ToList();
            _logger.LogInformation("Product filtering and returning to response...");
            return productList;
        }
    }
}
