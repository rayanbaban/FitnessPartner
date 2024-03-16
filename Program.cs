using FitnessPartner.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
// Add database context configuration
builder.Services.AddDbContext<FitnessPartnerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0))));
=======

builder.AddJwtAuthentication();

>>>>>>> BranchV2

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

<<<<<<< HEAD
=======

>>>>>>> BranchV2
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Database migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FitnessPartnerDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log any errors here
        Console.WriteLine("An error occurred while migrating the database: " + ex.Message);
    }
}

app.Run();
