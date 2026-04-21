using Microsoft.EntityFrameworkCore;
using PerformanceTest.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PerformanceDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(connectionString)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
        .LogTo(Console.WriteLine, LogLevel.Information));


builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PerformanceDbContext>();

    await db.Database.MigrateAsync();

    await SeedData.InitializeAsync(db);
}
app.UseHttpsRedirection();


app.MapControllers();
app.Run();


