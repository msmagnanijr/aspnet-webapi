namespace Endurance.Endpoints.Categories;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Endurance.Domain.Products;
using Endurance.Infrastructure.Data;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(CategoryRequest categoryRequest, HttpContext httpContext, EFContext eFContext)
    {
        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = new Category(categoryRequest.Name, userId, userId);

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        await eFContext.Categories.AddAsync(category);
        await eFContext.SaveChangesAsync();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}