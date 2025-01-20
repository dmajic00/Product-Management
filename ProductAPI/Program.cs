var builder = WebApplication.CreateBuilder(args);

// Add CORS policy to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173", "https://localhost:5173") // Replace with your frontend's URL
                        .AllowAnyHeader() // Allow all HTTP headers
                        .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                        .WithExposedHeaders("X-Total-Count")); // Expose custom header to the frontend
});

builder.Services.AddControllers(); // Add support for controllers
builder.Services.AddEndpointsApiExplorer(); // Enable endpoint exploration for minimal APIs
builder.Services.AddSwaggerGen(); // Add Swagger for API documentation and testing

// Register repository service
builder.Services.AddScoped<ProductApi.Repositories.ProductRepository>(); // Dependency injection for ProductRepository

var app = builder.Build();

// Use CORS before other middleware to ensure frontend has access
app.UseCors("AllowFrontend");

// Configure middleware pipeline
if (app.Environment.IsDevelopment()) // Enable Swagger only in development environment
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI for API testing
}

// Map controller endpoints
app.MapControllers();

// Run the application
app.Run();
