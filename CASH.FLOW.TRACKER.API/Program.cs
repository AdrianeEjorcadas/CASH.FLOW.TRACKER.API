using BUGET.TRACKER.API.Data;
using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Repositories;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using CASH.FLOW.TRACKER.API.Services;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//exception
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//repo
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//service
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
