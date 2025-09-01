using Aspire.Hosting;

const string ApiKeyEnvironmentName = "PORTFOLIO_API_KEY";

var builder = DistributedApplication.CreateBuilder(args);

#if DEBUG
var apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentName);

builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi")
    .WithEnvironment(ApiKeyEnvironmentName, apiKey);
#else
builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi");
#endif

builder.Build().Run();
