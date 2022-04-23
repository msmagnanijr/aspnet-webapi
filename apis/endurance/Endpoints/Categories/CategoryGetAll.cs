namespace Endurance.Endpoints.Categories;

using Endurance.Domain.Products;
using Endurance.Infrastructure.Data;

using Microsoft.AspNetCore.Authorization;
public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(EFContext eFContext)
    {
        var categories = eFContext.Categories.ToList();
        var response = categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Active = c.Active
        });
        return Results.Ok(response);
    }
}
