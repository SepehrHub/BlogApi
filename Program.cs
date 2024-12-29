// Program.cs file for setting up the WebApplication and services
using BlogApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (Controllers, DbContext, etc.)
builder.Services.AddControllers();

// Add DbContext with in-memory database for simplicity during testing
builder.Services.AddDbContext<BlogContext>(options =>
    options.UseInMemoryDatabase("BlogDb"));

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();  // Map controller actions to their routes

app.Run();  // Run the application


