namespace Endurance.Endpoints.Employees;

using Microsoft.Data.SqlClient;
using Dapper;

public class EmployeeGetAll
{
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, IConfiguration configuration)
    {
        var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

        var query = @"select Email, ClaimValue as Name
            from AspNetUsers u inner
            join AspNetUserClaims c
            on u.id = c.UserId and claimtype = 'Name'
            order by name
            OFFSET (@page -1 ) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        var employee = connection.Query<EmployeeResponse>(
            query,
            new { page, rows }
        );

        return Results.Ok(employee);
    }
}