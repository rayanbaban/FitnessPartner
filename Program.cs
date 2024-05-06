using FitnessPartner.Data;
using FitnessPartner.Extensions;
using FitnessPartner.Middleware;
using FitnessPartner.Models;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services;
using FitnessPartner.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.RegisterMappers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.AddSwaggerWithJWTBearerAuthentication();
builder.Services.AddHttpContextAccessor();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseLibraryService, ExerciseLibraryService>();
builder.Services.AddScoped<IExerciseSessionService, ExerciseSessionService>();
builder.Services.AddScoped<IFitnessGoalsService, FitnessGoalsService>();
builder.Services.AddScoped<INutritionLogService, NutritionLogService>();
builder.Services.AddScoped<INutritionPlanService, NutritionPlanService>();
builder.Services.AddScoped<INutritionResourceService, NutritionResourceService>();
builder.Services.AddScoped<IAuthService, AuthService>();



//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExerciseLibraryRepository, ExerciseLibraryRepository>();
builder.Services.AddScoped<IExersiceSessionRepository, ExerciseSessionRepository>();
builder.Services.AddScoped<IFitnessGoalsRepository, FitnessGoalsRepository>();
builder.Services.AddScoped<INutritionLogRepository, NutritionLogRepository>();
builder.Services.AddScoped<INutritionPlansRepository, NutritionPlansRepository>();
builder.Services.AddScoped<INutritionResourcesRepository, NutritionResourcesRepository>();



builder.Services.AddTransient<GlobalExcpetionMiddleware>();
builder.Services.AddScoped<JwtMiddleware>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation(config => config.DisableDataAnnotationsValidation = false);


builder.Services.AddDbContext<FitnessPartnerDbContext>(options =>
				options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
				new MySqlServerVersion(new Version(8, 0))));



// Add Identity
builder.Services
    .AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<FitnessPartnerDbContext>()
    .AddDefaultTokenProviders();

// config IDentity

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

//registreringen av sriloggen som ble skrevet inn i config filen skjer her 
builder.Host.UseSerilog((context, configuration) =>
{
    //her sier vi at vi vil bruke serilog og hente konfigurasjonen fra config filen
    //husk også	legge det til i DI som ligger i personDBHandler
    //da er vi i mål
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
