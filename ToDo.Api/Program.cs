using Microsoft.EntityFrameworkCore;
using ToDoApp.Data.DataContext;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories;
using ToDoApp.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var connectionString = builder.Configuration.GetConnectionString("ToDoDb");
builder.Services.AddDbContext<ToDoDataContext>(options => options.UseSqlServer(connectionString),ServiceLifetime.Scoped);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
