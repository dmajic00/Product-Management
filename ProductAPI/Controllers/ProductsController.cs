using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(ProductRepository repository) : ControllerBase
    {
        private readonly ProductRepository _repository = repository;

        // GET: api/products
        // Retrieves a paginated list of products with optional search, sorting, and ordering.
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, string? search = null, string? sortBy = "name", bool ascending = true)
        {
            // Fetch the paginated list of products and the total count from the repository
            var (products, total) = await _repository.GetProductsAsync(page, pageSize, search, sortBy, ascending);

            // Add the total count as a custom header in the response
            Response.Headers["X-Total-Count"] = total.ToString();

            // Return the list of products
            return Ok(products);
        }

        // GET: api/products/{id}
        // Retrieves a product by its ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Fetch the product by ID
            var product = await _repository.GetProductByIdAsync(id);

            // Return 404 if not found, otherwise return the product
            return product == null ? NotFound() : Ok(product);
        }

        // POST: api/products
        // Creates a new product.
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // Validate that the product name is at least 3 characters long
            if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length < 3)
                return BadRequest("Product name must be at least 3 characters long.");

            // Validate that the product price is greater than zero
            if (product.Price <= 0)
                return BadRequest("Product price must be greater than zero.");

            // Check for duplicate product name
            var existingProduct = await _repository.GetProductByNameAsync(product.Name);
            if (existingProduct != null)
            {
                // Return 409 Conflict if a product with the same name exists
                return Conflict(new { message = "A product with this name already exists." });
            }

            // Create the product and get the new product ID
            var id = await _repository.CreateProductAsync(product);

            // Return 201 Created with the newly created product
            return CreatedAtAction(nameof(GetById), new { id }, product);
        }

        // PUT: api/products/{id}
        // Updates an existing product.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // Ensure the ID in the route matches the product ID in the request body
            if (id != product.ProductId)
            {
                return BadRequest(new { message = "Product ID mismatch." });
            }

            // Check for duplicate product name with a different ID
            var existingProduct = await _repository.GetProductByNameAsync(product.Name);
            if (existingProduct != null && existingProduct.ProductId != id)
            {
                // Return 409 Conflict if a product with the same name exists
                return Conflict(new { message = "A product with this name already exists." });
            }

            // Attempt to update the product
            var updated = await _repository.UpdateProductAsync(product);
            if (updated > 0)
            {
                // Return success message if the update was successful
                return Ok(new { message = "Product updated successfully." });
            }

            // Return 404 if the product was not found
            return NotFound(new { message = "Product not found." });
        }

        // DELETE: api/products/{id}
        // Deletes a product by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if the product exists
            var existingProduct = await _repository.GetProductByIdAsync(id);
            if (existingProduct == null) return NotFound();

            // Delete the product
            await _repository.DeleteProductAsync(id);

            // Return 204 No Content to indicate successful deletion
            return NoContent();
        }
    }
}
