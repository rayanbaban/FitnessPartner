using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitnessPartner.Extensions
{
	public static class AuthenticationExtension
	{
		public static void AddJwtAuthentication(this WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication(s =>
			{
				s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).
			AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
					ValidAudience = builder.Configuration["JwtSettings:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true
				};
			});
		}
	}
}
