using EShoppingBusinessLibrary.ProductModels;

namespace EShoppingAPI.IRepository
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetProductList();
        public Task<Product> GetProductByName(string productName);
    }
}
