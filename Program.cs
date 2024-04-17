using FitnessPartner.Data;
using FitnessPartner.Extensions;
using FitnessPartner.Middleware;
using FitnessPartner.Models;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services;
using FitnessPartner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.RegisterMappers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.AddSwaggerWithJWTBearerAuthentication();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseLibraryService, ExerciseLibraryService>();
builder.Services.AddScoped<IExerciseSessionService, ExerciseSessionService>();
builder.Services.AddScoped<IFitnessGoalsService, FitnessGoalsService>();
builder.Services.AddScoped<INutritionLogService, NutritionLogService>();
builder.Services.AddScoped<INutritionPlanService, NutritionPlanService>();
builder.Services.AddScoped<INutritionResourceService, NutritionResourceService>();



//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExerciseLibraryRepository, ExerciseLibraryRepository>();
builder.Services.AddScoped<IExersiceSessionRepository, ExerciseSessionRepository>();
builder.Services.AddScoped<IFitnessGoalsRepository, FitnessGoalsRepository>();
builder.Services.AddScoped<INutritionLogRepository, NutritionLogRepository>();
builder.Services.AddScoped<INutritionPlansRepository, NutritionPlansRepository>();
builder.Services.AddScoped<INutritionResourcesRepository, NutritionResourcesRepository>();



builder.Services.AddTransient<GlobalExcpetionMiddleware>();

//builder.Services.AddFluentValidationAutoValidation(config => config.DisableDataAnnotationsValidation = false);





builder.Services.AddDbContext<FitnessPartnerDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0)));

    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddHttpContextAccessor();



//registreringen av sriloggen som ble skrevet inn i config filen skjer her 
builder.Host.UseSerilog((context, configuration) =>
{
    //her sier vi at vi vil bruke serilog og hente konfigurasjonen fra config filen
    //husk også	legge det til i DI som ligger i personDBHandler
    //da er vi i mål
    configuration.ReadFrom.Configuration(context.Configuration);
});



builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


builder.AddJwtAuthentication();


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
