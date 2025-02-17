using DavidKuehn.Portfolio.Core.Constants;
using DavidKuehn.Portfolio.WebApi.Models.IdentityProvider;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = Environment.GetEnvironmentVariable(Infrastructure.SecurityDatabaseConnectionStringName);

// Add services to the container.
builder.Services.AddDbContext<SecurityDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
