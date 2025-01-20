using Dapper;
using Microsoft.Data.SqlClient;
using ProductApi.Models;

namespace ProductApi.Repositories
{
    // Repository for performing CRUD operations on the Product table in the database
    public class ProductRepository
    {
        private readonly string _connectionString;

        // Constructor initializes the repository with a connection string from configuration
        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException(nameof(configuration));
        }

        // Creates and returns a new SQL connection using the connection string
        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        // Retrieves a paginated list of products with optional search, sorting, and filtering
        public async Task<(IEnumerable<Product>, int)> GetProductsAsync(
            int page, int pageSize, string? search, string? sortBy, bool ascending)
        {
            const string baseQuery = "SELECT * FROM Products"; // Base query to fetch products
            const string countQuery = "SELECT COUNT(*) FROM Products"; // Query to get total product count

            // Add a WHERE clause if search is provided (case-insensitive matching)
            var whereClause = string.IsNullOrEmpty(search)
                ? ""
                : " WHERE LOWER(Name) LIKE LOWER(@Search) OR CAST(Price AS NVARCHAR) LIKE @Search";

            // Determine the ORDER BY clause based on the sortBy parameter
            var orderByClause = sortBy switch
            {
                "name" => " ORDER BY Name",
                "price" => " ORDER BY Price",
                _ => " ORDER BY ProductId" // Default sort by ProductId
            };

            // Append DESC for descending order if ascending is false
            if (!ascending)
            {
                orderByClause += " DESC";
            }

            // Offset and limit for pagination
            var offset = (page - 1) * pageSize;
            var limitClause = " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using var connection = CreateConnection();

            // Fetch the paginated products
            var products = await connection.QueryAsync<Product>(
                $"{baseQuery}{whereClause}{orderByClause}{limitClause}",
                new { Search = $"%{search}%", Offset = offset, PageSize = pageSize }
            );

            // Fetch the total count of products matching the search criteria
            var total = await connection.ExecuteScalarAsync<int>(
                $"{countQuery}{whereClause}",
                new { Search = $"%{search}%" }
            );

            return (products, total);
        }

        // Retrieves a single product by its ID
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            const string query = "SELECT * FROM Products WHERE ProductId = @Id";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product?>(query, new { Id = id });
        }

        // Retrieves a single product by its name (case-insensitive)
        public async Task<Product?> GetProductByNameAsync(string name)
        {
            const string query = "SELECT * FROM Products WHERE LOWER(Name) = LOWER(@Name)";
            Console.WriteLine($"Checking for existing product with name: {name}");
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product?>(query, new { Name = name });
        }

        // Creates a new product and returns the generated ProductId
        public async Task<int> CreateProductAsync(Product product)
        {
            const string query = @"
                INSERT INTO Products (Name, Price, Description, Quantity, CreatedAt) 
                VALUES (@Name, @Price, @Description, @Quantity, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            // Set the CreatedAt timestamp to the current UTC time
            product.CreatedAt = DateTime.UtcNow;

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, product);
        }

        // Updates an existing product and returns the number of affected rows
        public async Task<int> UpdateProductAsync(Product product)
        {
            const string query = @"
                UPDATE Products 
                SET Name = @Name, Price = @Price, Description = @Description, 
                    Quantity = @Quantity, UpdatedAt = @UpdatedAt 
                WHERE ProductId = @ProductId";

            // Set the UpdatedAt timestamp to the current UTC time
            product.UpdatedAt = DateTime.UtcNow;

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(query, product);
        }

        // Deletes a product by its ID and returns the number of affected rows
        public async Task<int> DeleteProductAsync(int id)
        {
            const string query = "DELETE FROM Products WHERE ProductId = @Id";
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
