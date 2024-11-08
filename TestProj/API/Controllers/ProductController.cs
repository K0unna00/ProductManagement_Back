﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestProj.API.DTOs;
using TestProj.Core.Entities;
using TestProj.Core.Exceptions;
using TestProj.Core.Interfaces;
using TestProj.Infrastructure.Repositories;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductController> _logger;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository repository, ILogger<ProductController> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a list of all products.
    /// </summary>
    /// <returns>A list of products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Gets a list of all products.");
        return Ok(await _repository.GetAllProductsAsync());
    }

    /// <summary>
    /// Gets a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <returns>The product with the specified ID</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="404">If the product is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(string id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
        {
            _logger.LogInformation("Product not found");
            throw new ProductNotFoundException();
        }

        _logger.LogInformation("Gets a product by its ID.");
        return Ok(product);
    }

    /// <summary>
    /// Adds a new product to the system.
    /// </summary>
    /// <param name="product">The product to be added</param>
    /// <returns>The newly added product</returns>
    /// <response code="201">The product was successfully created</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), 201)]
    public async Task<IActionResult> Post([FromForm] ProductDto productDto)
    {
        var entity = _mapper.Map<Product>(productDto);

        entity.ImgName = await _repository.SaveImgAsync(productDto.Img);
        await _repository.AddProductAsync(entity);

        _logger.LogInformation("Adds a new product to the system.");
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">The ID of the product to be updated</param>
    /// <param name="product">The updated product data</param>
    /// <returns>No content response</returns>
    /// <response code="204">The product was successfully updated</response>
    /// <response code="400">Invalid product data</response>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Put(string id, [FromBody] Product product)
    {
        await _repository.UpdateProductAsync(id, product);

        _logger.LogInformation("Updates an existing product.");
        return NoContent();
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to be deleted</param>
    /// <returns>No content response</returns>
    /// <response code="204">The product was successfully deleted</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(string id)
    {
        await _repository.DeleteProductAsync(id);

        _logger.LogInformation("Deletes a product by its ID.");
        return NoContent();
    }

    /// <summary>
    /// Gets a products by IDs.
    /// </summary>
    /// <param name="ids">The IDs of the product to be filtered</param>
    /// <returns>A list of products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpPost("getByIds")]
    public async Task<IActionResult> GetProductsByIds([FromBody] List<string> ids)
    {
        var products = await _repository.GetProductByIdsAsync(ids);

        _logger.LogInformation(" Gets a products by IDs.");
        return Ok(products);
    }


}
