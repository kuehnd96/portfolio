using Microsoft.AspNetCore.Authorization;

namespace DavidKuehn.Portfolio.WebApi.Infrastructure.Security
{
    /// <summary>
    /// Represents a requirement for API key-based authorization.
    /// </summary>
    /// <remarks>This requirement is used in the context of authorization policies to enforce that a valid API
    /// key is provided by the caller. It is typically used with a custom authorization handler that validates  the
    /// presence and correctness of the API key.</remarks>
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
    }
}
