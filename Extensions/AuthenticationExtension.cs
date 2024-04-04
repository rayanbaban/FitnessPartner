using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
