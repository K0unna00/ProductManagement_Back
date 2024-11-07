using TestProj.Core.Entities;

namespace TestProj.Core.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();

    Task<Product> GetProductByIdAsync(string id);

    Task AddProductAsync(Product product);

    Task UpdateProductAsync(string id, Product product);

    Task DeleteProductAsync(string id);
}
