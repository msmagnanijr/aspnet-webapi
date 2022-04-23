namespace Endurance.Endpoints.Categories;

using Endurance.Domain.Products;
using Endurance.Infrastructure.Data;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, EFContext eFContext)
    {
        var category = new Category(categoryRequest.Name, "Thunder", "Thunder");

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        eFContext.Categories.Add(category);
        eFContext.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}