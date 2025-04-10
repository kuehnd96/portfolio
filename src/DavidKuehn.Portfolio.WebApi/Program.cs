using DavidKuehn.Portfolio.Core.Constants;
using DavidKuehn.Portfolio.WebApi.Models.Career;
using DavidKuehn.Portfolio.WebApi.Models.IdentityProvider;
using DavidKuehn.Portfolio.WebApi.Services.IdentityProvider.Core_RBS_Tokens.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var securityConnectionString = Environment.GetEnvironmentVariable(Infrastructure.SecurityDatabaseConnectionStringName);

// Add services to the container.
builder.Services.AddDbContext<SecurityDbContext>(options =>
{
    options.UseSqlServer(securityConnectionString);
});

var careerConnectionString = Environment.GetEnvironmentVariable(Infrastructure.CareerDatabaseConnectionStringName);

builder.Services.AddDbContext<CareerDbContext>(options =>
{
    options.UseSqlServer(careerConnectionString);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    // Use the EF Core for Creating, Managing Users and Roles
    .AddEntityFrameworkStores<SecurityDbContext>().AddDefaultTokenProviders();

#region The CORS
builder.Services.AddCors(options => // TODO: Tweak this after tracer bullets
{
    options.AddPolicy("cors", (policy) =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
#endregion

builder.Services.AddScoped<SecurityServices>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin");
    })
    .AddPolicy("ReaderPolicy", policy =>
    {
        policy.RequireRole("Reader");
    });

#region Token Validation
// Read the Secret Key from the appsettings.json
var secretKeyString = Environment.GetEnvironmentVariable(Infrastructure.JwtSecretKey);
if (string.IsNullOrEmpty(secretKeyString))
{
    throw new InvalidOperationException("Secret key is not configured properly.");
}
byte[] secretKey = Convert.FromBase64String(secretKeyString);

// set the Authentication Scheme
builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = null,
            ValidAudience = null,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString))
        };
    });
#endregion

#region The JSON Serialization
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null; // Use Pascal case
    options.SerializerOptions.DictionaryKeyPolicy = null; // Use Pascal case
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
#endregion

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("cors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
