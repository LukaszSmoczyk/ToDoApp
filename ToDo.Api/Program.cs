using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDoApp.Data.DataContext;
using ToDoApp.Data.Models;
using ToDoApp.Data.Repositories;
using ToDoApp.Data.Repositories.Interfaces;
using ToDoApp.Data.Services;
using ToDoApp.Data.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Include XML comments from controllers for Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//Repositories
builder.Services.AddScoped<IItemRepository, ItemRepository>();
//Services
builder.Services.AddScoped<IItemService, ItemService>();
//Db
var connectionString = builder.Configuration.GetConnectionString("ToDoDb");
builder.Services.AddDbContext<ToDoDataContext>(options => options.UseSqlServer(connectionString),ServiceLifetime.Scoped);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoApp.API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
