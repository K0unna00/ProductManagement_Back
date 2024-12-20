﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestProj.API.DTOs;
using TestProj.Core.Common;
using TestProj.Core.Entities;
using TestProj.Core.Exceptions;
using TestProj.Core.Interfaces;
using TestProj.Core.Services;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductController> _logger;
    private readonly IMapper _mapper;
    private readonly IFileService _fileUtility;
    private readonly IProductService _productService;

    public ProductController(IProductRepository repository, ILogger<ProductController> logger, IMapper mapper,
        IFileService fileUtility, IProductService productService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _fileUtility = fileUtility;
        _productService = productService;
    }

    /// <summary>
    /// Gets a list of all products.
    /// </summary>
    /// <returns>A list of products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
    public async Task<IActionResult> Get()
    {
        var response = await _productService.GetAllProductsAsync();

        _logger.LogInformation("Gets a list of all products.");
        return Ok(new ApiResponse<IEnumerable<ProductDto>>(response));
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
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            _logger.LogInformation("Product not found");
            throw new ProductNotFoundException();
        }

        _logger.LogInformation("Gets a product by its ID.");
        return Ok(new ApiResponse<ProductDto>(product));
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
        await _productService.CreateProduct(productDto);

        var response = CreatedAtAction(nameof(Get), new { id = productDto.Id }, productDto);

        _logger.LogInformation("Adds a new product to the system.");
        return Ok(new ApiResponse<CreatedAtActionResult>(response));

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
    public async Task<IActionResult> Put(string id, [FromForm] ProductDto productDto)
    {
        await _productService.UpdateProduct(id, productDto);

        _logger.LogInformation("Updates an existing product.");
        return Ok(new ApiResponse<IActionResult>());
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
        await _productService.DeleteProductAsync(id);

        _logger.LogInformation("Deletes a product by its ID.");
        return Ok(new ApiResponse<IActionResult>());
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
        var response = await _repository.GetByIdsAsync(ids);

        _logger.LogInformation(" Gets a products by IDs.");
        return Ok(new ApiResponse<IEnumerable<Product>>(response));
    }


}
