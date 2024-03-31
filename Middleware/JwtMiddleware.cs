using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace FitnessPartner.Middleware
{
	public class JwtMiddleware
	{

		private readonly RequestDelegate _next;

		// Dependency Injection
		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			//Reading the AuthHeader which is signed with JWT
			string authHeader = context.Request.Headers["Authorization"];

			if (authHeader != null)
			{
				//Reading the JWT middle part           
				int startPoint = authHeader.IndexOf(".") + 1;
				int endPoint = authHeader.LastIndexOf(".");

				var tokenString = authHeader
					.Substring(startPoint, endPoint - startPoint).Split(".");
				var token = tokenString[0].ToString() + "==";

				var credentialString = Encoding.UTF8
					.GetString(Convert.FromBase64String(token));

				// Splitting the data from Jwt
				var credentials = credentialString.Split(new char[] { ':', ',' });

				// Trim this Username and UserRole.
				var userRule = credentials[5].Replace("\"", "");
				var userName = credentials[3].Replace("\"", "");

				// Identity Principal
				var claims = new[]
				{
			   new Claim("name", userName),
			   new Claim(ClaimTypes.Role, userRule),
		   };
				var identity = new ClaimsIdentity(claims, "basic");
				context.User = new ClaimsPrincipal(identity);
			}
			//Pass to the next middleware
			await _next(context);
		}

	}
}
