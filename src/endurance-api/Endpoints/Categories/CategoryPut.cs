namespace Endurance.Endpoints.Categories;
public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(
        [FromRoute] Guid id, HttpContext http, CategoryRequest categoryRequest, EFContext eFContext)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = eFContext.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
            return Results.NotFound();

        category.Update(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        eFContext.SaveChanges();

        return Results.Ok();
    }
}