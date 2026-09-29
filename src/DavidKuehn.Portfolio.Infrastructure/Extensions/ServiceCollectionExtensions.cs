
using DavidKuehn.Portfolio.Core.WorkExperience.Interfaces;
using DavidKuehn.Portfolio.Infrastructure.WorkExperience.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DavidKuehn.Portfolio.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IWorkExperienceData, WorkExperienceData>();

        return services;
    }
}