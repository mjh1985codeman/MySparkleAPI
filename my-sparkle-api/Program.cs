using Microsoft.EntityFrameworkCore;
using my_sparkle_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Register AppDbContext and connect it to SQL Server using our connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
