using TestProj.API.DTOs;

namespace TestProj.Core.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();

    Task<ProductDto> GetProductByIdAsync(string id);

    Task CreateProduct(ProductDto productDto);

    Task UpdateProduct(string id, ProductDto productDto);

    Task DeleteProductAsync(string id);
}
