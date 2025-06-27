using Products.API.ViewModels;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/best-selling-products", () => new List<Product> {
    new("Product 1", 10),
    new("Product 2", 20),
    new("Product 3", 30),
    new("Product 4", 40),
    new("Product 5", 50),
    new("Product 6", 60),
});

app.Run();