namespace Endurance.Endpoints.Security;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenPost
{
    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager)
    {
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if (user == null)
            Results.BadRequest("Invalid email or password");

        if (!userManager.CheckPasswordAsync(user, loginRequest.Password).Result)
            Results.BadRequest();

        var key = Encoding.ASCII.GetBytes("JwtBearerTokenSettings:SecretKey");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = "Endurance",
            Issuer = "Endurance",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });

    }
}