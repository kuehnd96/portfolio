
using DavidKuehn.Portfolio.Core.WorkExperience.Models;
using DavidKuehn.Portfolio.UseCases.General;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Handlers;
using DavidKuehn.Portfolio.UseCases.WorkExperience.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace DavidKuehn.Portfolio.UseCases.General.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        // Register the QueryInvoker
        services.AddScoped<IQueryInvoker, QueryInvoker>();
        
        // Register query handlers
        services.AddScoped<IQueryHandler<JobByIdQuery, Job>, JobByIdQueryHandler>();
        services.AddScoped<IQueryHandler<JobListQuery, IEnumerable<ListJob>>, JobListQueryHandler>();

        return services;
    }
}