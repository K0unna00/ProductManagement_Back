using TestProj.API.DTOs;

namespace TestProj.Core.Services;

public interface IProductService
{
    public Task<IEnumerable<ProductDto>> GetAllProductsAsync();

    public Task<ProductDto> GetProductByIdAsync(string id);
}
