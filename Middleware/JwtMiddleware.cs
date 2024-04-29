using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FitnessPartner.Middleware;

public class JwtMiddleware : IMiddleware
{
    private readonly IConfiguration _configuration;
	private readonly ILogger<JwtMiddleware> _logger;

	public JwtMiddleware(IConfiguration configuration, ILogger<JwtMiddleware> logger)
    {
        _configuration = configuration;
		_logger = logger;
	}
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            //var identity = context.User.Identity as ClaimsIdentity;

            string? userId = ValidateAccessToken(token);
            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["UserId"] = userId;
            }
        }

        await next(context);
    }

    private string ValidateAccessToken(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = key,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;

            var userId = jwtToken?.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
            return userId ?? string.Empty;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.ToString());
            return string.Empty;
        }
    }
}