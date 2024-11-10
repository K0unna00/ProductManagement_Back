using TestProj.API.DTOs;
using TestProj.Core.Exceptions;
using TestProj.Core.Interfaces;
using TestProj.Core.Services;

namespace TestProj.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IFileService _fileService;

    public ProductService(IProductRepository repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImgBase64 = _fileService.ConvertImageToBase64(product.ImgName)
        });
    }

    public async Task<ProductDto> GetProductByIdAsync(string id)
    {
        var product = await _repository.GetByIdAsync(id) ?? throw new ProductNotFoundException();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImgBase64 = _fileService.ConvertImageToBase64(product.ImgName)
        };
    }
}
