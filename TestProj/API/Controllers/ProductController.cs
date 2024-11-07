using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TestProj.Core.Entities;
using TestProj.Core.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
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
            return NotFound();
        }
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
    public async Task<IActionResult> Post([FromBody] Product product)
    {
        await _repository.AddProductAsync(product);

        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
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

        return NoContent();
    }
}
