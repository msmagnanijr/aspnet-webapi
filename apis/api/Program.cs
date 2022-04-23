using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EFContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

/*
app.MapGet("/", () => "Hello from my first ASP.NET Core Web API");

app.MapGet("/user", () => new { Name = "Mauricio Magnani", Age = 33 });

app.MapGet("/addheader", (HttpResponse reponse) =>
{
    reponse.Headers.Add("X-MyHeader", "MyValue");
    return "Updated header";
});

app.MapPost("/saveproduct", (Product product) =>
{
    return $"Saved product: {product.Code} - {product.Name}";
});

//api.app.com/users?datastart={date}&dataend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return $"Get product between {dateStart} and {dateEnd}";
});

//api.app.com/users/{code}
app.MapGet("/getproduct/{code}", ([FromRoute] string code) =>
{
    return $"Get product with code {code}";
});

//header
app.MapGet("/getproducts", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});*/

// ******* CRUD API *******

app.MapPost("/products", (ProductRequest productRequest, EFContext eFContext) =>
{
    var category = eFContext.Categories.Where(c => c.Id == productRequest.CategoryId).FirstOrDefault();
    var product = new Product
    {
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Category = category
    };
    if (productRequest.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productRequest.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }
    eFContext.Products.Add(product);
    eFContext.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
});

app.MapGet("/products/{id}", ([FromRoute] int id, EFContext eFContext) =>
{
    var product = eFContext.Products
        .Include(p => p.Category)
        .Include(p => p.Tags)
        .Where(p => p.Id == id).FirstOrDefault();

    if (product != null)
        return Results.Ok(product);
    return Results.NotFound("Product not found");
});

app.MapPut("/products/{id}", ([FromRoute] int id, ProductRequest productRequest, EFContext eFContext) =>
{

    var product = eFContext.Products
            .Include(p => p.Tags)
            .Where(p => p.Id == id).FirstOrDefault();

    var category = eFContext.Categories.Where(c => c.Id == productRequest.CategoryId).FirstOrDefault();


    product.Code = productRequest.Code;
    product.Name = productRequest.Name;
    product.Description = productRequest.Description;
    product.Category = category;
    product.Tags = new List<Tag>();
    if (productRequest.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productRequest.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }
    eFContext.SaveChanges();
    return Results.Ok(product);
});

app.MapDelete("/products/{id}", ([FromRoute] int id, EFContext eFContext) =>
{
    var product = eFContext.Products.Where(p => p.Id == id).FirstOrDefault();
    eFContext.Products.Remove(product);
    eFContext.SaveChanges();
    return Results.Ok();
});


if (app.Environment.IsDevelopment())
{
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
    {
        return Results.Ok($"{configuration["database:type"]} - {configuration["database:port"]}");
    });
}


app.Run();


