using BUGET.TRACKER.API.Data;
using CASH.FLOW.TRACKER.API.Middleware.Exceptions;
using CASH.FLOW.TRACKER.API.Repositories;
using CASH.FLOW.TRACKER.API.Repositories.Interface;
using CASH.FLOW.TRACKER.API.Services;
using CASH.FLOW.TRACKER.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//cors var
var DevCorsPolicy = "DevPolicy";
var ProdSpecificOrigin = "ProdPolicy";

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

//cors-policy
builder.Services.AddCors(options =>
{
    //Production set up
    options.AddPolicy(name: ProdSpecificOrigin,
        policy => policy.WithOrigins("https://sample.com")
              .WithMethods("GET", "POST", "PATCH", "DELETE")// restrict to supported methods
              .WithHeaders("Content-Type", "Authorization")); // restrict to needed headers

    //Development: allow local host testing
    options.AddPolicy(name: DevCorsPolicy,
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

//repo
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//service
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors(DevCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
