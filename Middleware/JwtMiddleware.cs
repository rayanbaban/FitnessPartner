using FitnessPartner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FitnessPartner.Middleware
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly JwtSettings _jwtSettings;

		public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
		{
			_next = next;
			_jwtSettings = jwtSettings.Value;
		}

		public async Task Invoke(HttpContext context)
		{
			string authHeader = context.Request.Headers["Authorization"];

			if (authHeader != null && authHeader.StartsWith("Bearer "))
			{
				string token = authHeader.Substring("Bearer ".Length).Trim();

				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = _jwtSettings.Issuer,
					ValidAudience = _jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(key)
				}, out SecurityToken validatedToken);



				var jwtToken = (JwtSecurityToken)validatedToken;
				var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
				var userRole = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;

				var claims = new[]
				{
				new Claim(ClaimTypes.Name, userName),
				new Claim(ClaimTypes.Role, userRole)
			};

				var identity = new ClaimsIdentity(claims, "jwt");
				context.User = new ClaimsPrincipal(identity);
			}

			await _next(context);

		}
	}
}
