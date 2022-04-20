using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello from my first ASP.NET Core Web API");

app.MapGet("/user", () => new { Name = "Mauricio Magnani", Age = 33 });

app.MapGet("/addheader", (HttpResponse reponse) =>
{
    reponse.Headers.Add("X-MyHeader", "MyValue");
    return "Updated header";
});

/*app.MapPost("/saveproduct", (Product product) =>
{
    return $"Saved product: {product.Code} - {product.Name}";
});*/

//api.app.com/users?datastart={date}&dataend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return $"Get product between {dateStart} and {dateEnd}";
});

//api.app.com/users/{code}
/*app.MapGet("/getproduct/{code}", ([FromRoute] string code) =>
{
    return $"Get product with code {code}";
});*/

//header
app.MapGet("/getproducts", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});

// ******* CRUD API *******

app.MapPost("/products", (Product product) =>
{
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

app.MapGet("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetById(code);
    if (product != null)
        return Results.Ok(product);
    return Results.NotFound("Product not found");
});

app.MapPut("/products", (Product product) =>
{
    var productSave = ProductRepository.GetById(product.Code);
    productSave.Name = product.Name;
    return Results.Ok(productSave);
});

app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetById(code);
    ProductRepository.Remove(product);
    return Results.Ok();
});

app.Run();


