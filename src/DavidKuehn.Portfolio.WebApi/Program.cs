using DavidKuehn.Portfolio.WebApi.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using DavidKuehn.Portfolio.Infrastructure.Extensions;
using DavidKuehn.Portfolio.UseCases.General.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure();
builder.Services.AddUseCases();

var databaseConnectionString = Environment.GetEnvironmentVariable("PORTFOLIO_DATABASE_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new InvalidOperationException("PORTFOLIO_DATABASE_CONNECTION_STRING is not set.");
}

builder.Services.AddSingleton(_ => databaseConnectionString);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(new[]
        {
            JwtBearerDefaults.AuthenticationScheme
        });
        policy.Requirements.Add(new ApiKeyRequirement());
    });
});

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
