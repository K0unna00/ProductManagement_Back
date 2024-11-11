using AutoMapper;
using TestProj.API.DTOs;
using TestProj.Core.Entities;
using TestProj.Core.Exceptions;
using TestProj.Core.Interfaces;
using TestProj.Core.Services;
using FileNotFoundException = TestProj.Core.Exceptions.FileNotFoundException;

namespace TestProj.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IFileService fileService, IMapper mapper)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
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
            ImgName = product.ImgName
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
            ImgName = product.ImgName
        };
    }

    public async Task DeleteProductAsync(string id)
    {
        var product = await _repository.GetByIdAsync(id) ?? throw new ProductNotFoundException();

        await _fileService.DeleteImageAsync(product.ImgName);

        await _repository.DeleteAsync(id);

    }

    public async Task CreateProduct(ProductDto productDto)
    {
        var entity = _mapper.Map<Product>(productDto);

        if (productDto.Image is not null)
        {
            entity.ImgName = await _fileService.SaveImageAsync(productDto.Image);
        }
        else
        {
            throw new FileNotFoundException();
        }

        await _repository.AddAsync(entity);
    }

    public async Task UpdateProduct(string id, ProductDto productDto)
    {
        var entity = _mapper.Map<Product>(productDto);

        if (entity.ImgName is null)
        {
            throw new FileNotFoundException();
        }
        else if(productDto.Image is not null)
        {
            await _fileService.DeleteImageAsync(entity.ImgName);

            entity.ImgName = await _fileService.SaveImageAsync(productDto.Image);

        }

        await _repository.UpdateAsync(id, entity);
    }
}
