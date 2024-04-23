using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FitnessPartner.Extensions
{
	public static class AuthenticationExtension
	{
		

		public static void AddSwaggerWithJWTBearerAuthentication(this WebApplicationBuilder builder)
		{
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Scheme = "bearer",
					Description = "Please insert JWT token into field"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
		 {
			 {
				 new OpenApiSecurityScheme
				 {
					 Reference = new OpenApiReference
					 {
						 Type = ReferenceType.SecurityScheme,
						 Id = "Bearer"
					 }
				 },
				 new string[] { }
			 }
		 });
			});
		}
	}

	
}
