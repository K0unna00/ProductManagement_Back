using TestProj.Core.Entities;

namespace TestProj.Core.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<Product> GetByIdAsync(string id);

    Task AddAsync(Product product);

    Task UpdateAsync(string id, Product product);

    Task DeleteAsync(string id);

    Task<List<Product>> GetByIdsAsync(List<string> ids);
}
