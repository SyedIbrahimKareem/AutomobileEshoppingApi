using EShoppingAutoMobilesBusinessLibrary.ProductModels;

namespace EShoppingAutoMobiles.IRepository
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetProductList();
        public Task<Product> GetProductByName(string productName);
    }
}
