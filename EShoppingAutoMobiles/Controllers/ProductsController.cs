using EShoppingAutoMobiles.Repository;
using EShoppingAutoMobilesBusinessLibrary.ProductModels;
using Microsoft.AspNetCore.Mvc;
using EShoppingAutoMobiles.IRepository;
using EShoppingAutoMobiles.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace EShoppingAutoMobiles.Controllers
{
    [Authorize(Roles ="User")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductsControllers : ControllerBase
    {
        private ILog _log;
        public readonly IProductRepository _productServices;
        public ProductsControllers(IProductRepository productServices)
        {
            _productServices = productServices;
            _log = Log.GetInstance();
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
                        ReasonPhrase = "Product Not Found"
                    };

                    //throw new HttpResponseException(response);
                }
                return productList;
            }
            catch (Exception ex)
            {
                _log.LogException(ex.StackTrace.ToString());
                return Enumerable.Empty<Product>();
            }


        }

        // GET: ProductControllercs/Details/5
        [HttpGet]
        public async Task<Product> GetProductDetailByName(string name)
        {
            var productList = await _productServices.GetProductByName(name);
            return productList;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProductListSearch(string searchValue)
        {
            var productList = await _productServices.GetProductList();
            productList=productList.Where(x => x.productName.Contains(searchValue)).ToList();
            return productList;
        }
    }
}
