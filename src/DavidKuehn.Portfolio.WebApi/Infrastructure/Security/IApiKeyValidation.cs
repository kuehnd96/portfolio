namespace DavidKuehn.Portfolio.WebApi.Infrastructure.Security
{
    /// <summary>
    /// Surface for API key validation.
    /// </summary>
    public interface IApiKeyValidation
    {
        /// <summary>
        /// Determines if the provided API key is valid.
        /// </summary>
        /// <param name="userApiKey">The API key to validate. Cannot be null or empty.</param>
        /// <returns>True if the key is valid; Otherwise false.</returns>
        bool IsApiKeyValid(string userApiKey);
    }
}
