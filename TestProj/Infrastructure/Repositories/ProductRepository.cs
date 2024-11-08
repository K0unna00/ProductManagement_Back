using MongoDB.Bson;
using MongoDB.Driver;
using TestProj.Core.Entities;
using TestProj.Core.Exceptions;
using TestProj.Core.Interfaces;
using TestProj.Infrastructure.Data;
using TestProj.Infrastructure.Utilities;

namespace TestProj.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MongoContext _context;
    private readonly IFileUtility _fileUtility;

    public ProductRepository(MongoContext context, IFileUtility fileUtility)
    {
        _context = context;
        _fileUtility = fileUtility;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }
        
    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        await CreateIndexesAsync();

        product.CreatedAt = DateTime.UtcNow;

        await _context.Products.InsertOneAsync(product);
    }

    public async Task UpdateProductAsync(string id, Product product)
    {
        await _context.Products.ReplaceOneAsync(p => p.Id == id , product);
    }

    public async Task DeleteProductAsync(string id)
    {
        await _context.Products.DeleteOneAsync(p => p.Id == id);
    }

    public async Task CreateIndexesAsync()
    {
        var keys = Builders<Product>.IndexKeys
            .Ascending(p => p.Name)
            .Descending(p => p.Price);
        await _context.Products.Indexes.CreateOneAsync(new CreateIndexModel<Product>(keys));
    }

    public async Task<List<Product>> GetProductByIdsAsync(List<string> ids)
    {
        var filter = Builders<Product>.Filter.In("Id", ids);

        var products = await _context.Products.Find(filter).ToListAsync();

        return products;
    }


    public async Task<string> SaveImgAsync(IFormFile img)
    {
        return await _fileUtility.SaveImageAsync(img);
    }
}
