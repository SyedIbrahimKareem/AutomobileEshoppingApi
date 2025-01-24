using EShoppingBusinessLibrary.ProductModels;
using EShoppingAPI.IRepository;
using EShoppingAPI.DataAccess;
using Dapper;
using EShoppingAPI.Helpers;
namespace EShoppingAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly ILog _log;
        public ProductRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
            _log = Log.GetInstance();
        }

        public async Task<IEnumerable<Product>> GetProductList()
        {
            try
            {
                var query = "Select * FROM Products";
                using (var connection = _dapperContext.CreateConnection())
                {
                    var products = await connection.QueryAsync<Product>(query);
                    return products;
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex.Message.ToString() + " " + ex.StackTrace.ToString());
                return null;
            }
        }
        public async Task<Product> GetProductByName(string productName)
        {

            var productList = await GetProductList();
            var ProductDetail = productList.Where(x => x.productName.Contains(productName)).First();
            return ProductDetail;
        }
    }
}
