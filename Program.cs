using FitnessPartner.Data;
using FitnessPartner.Extensions;
using FitnessPartner.Middleware;
using FitnessPartner.Repositories;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Services;
using FitnessPartner.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.Entities;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.RegisterMappers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseLibraryService, ExerciseLibraryService>();



// Mappers
builder.Services.AddScoped<IMapper<User, UserDTO>, UserMapper>();
builder.Services.AddScoped<IMapper<User, UserRegDTO>, UserRegMapper>();


//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IExerciseLibraryRepository, ExerciseLibraryRepository>();



builder.Services.AddTransient<GlobalExcpetionMiddleware>();

//builder.Services.AddFluentValidationAutoValidation(config => config.DisableDataAnnotationsValidation = false);





builder.Services.AddDbContext<FitnessPartnerDbContext>(options =>
{
	options.UseMySql(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		new MySqlServerVersion(new Version(8, 0)));

	options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
});


builder.AddJwtAuthentication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware <JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
