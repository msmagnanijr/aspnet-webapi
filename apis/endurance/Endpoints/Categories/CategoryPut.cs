namespace Endurance.Endpoints.Categories;

using Microsoft.AspNetCore.Mvc;
using Endurance.Domain.Products;
using Endurance.Infrastructure.Data;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, EFContext eFContext)
    {
        var category = eFContext.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
            return Results.NotFound();

        category.Update(categoryRequest.Name, categoryRequest.Active);

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        eFContext.SaveChanges();
        return Results.Ok();
    }
}