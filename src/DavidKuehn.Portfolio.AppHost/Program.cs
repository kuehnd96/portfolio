using Aspire.Hosting;

const string ApiKeyEnvironmentName = "PORTFOLIO_API_KEY";

var builder = DistributedApplication.CreateBuilder(args);
IResourceBuilder<ProjectResource> portFolioApiProject;

#if DEBUG
var apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentName);

portFolioApiProject = builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi")
    .WithEnvironment(ApiKeyEnvironmentName, apiKey);
#else
portFolioApiProject = builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi");
#endif

builder.AddProject<Projects.DavidKuehn_Portfolio_UI>("davidkuehn-portfolio-ui")
    .WithReference(portFolioApiProject);

builder.AddProject<Projects.DavidKuehn_Portfolio_UI_Web>("davidkuehn-portfolio-ui-web")
    .WithReference(portFolioApiProject);

builder.Build().Run();
