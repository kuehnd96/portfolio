var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DavidKuehn_Portfolio_WebApi>("portfolio-webapi");

builder.Build().Run();
