using Aspire.Hosting;

const string ApiKeyEnvironmentName = "PORTFOLIO_API_KEY";

var builder = DistributedApplication.CreateBuilder(args);

var webApi = builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi");

#if DEBUG
var apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentName);

webApi.WithEnvironment(ApiKeyEnvironmentName, apiKey);
#else
builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi");
#endif

builder.AddProject<Projects.DavidKuehn_Portfolio_UI_Web>("portfolio-ui-web")
    .WithReference(webApi);

builder.Build().Run();