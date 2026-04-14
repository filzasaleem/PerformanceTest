using Microsoft.EntityFrameworkCore;
using PerformanceTest.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PerformanceDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(connectionString));


builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapControllers();
app.Run();


